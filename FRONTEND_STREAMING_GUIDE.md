# Frontend Implementation Guide for Message Streaming

This guide shows how to implement the Rift API message streaming in your frontend application.

## API Endpoints Available

### 1. Authenticated User Streaming
- **Endpoint**: `POST /api/messages/stream`
- **Authentication**: Required (JWT Bearer token)
- **Content-Type**: `text/event-stream`

### 2. Guest User Streaming  
- **Endpoint**: `POST /api/messages/guest/stream`
- **Authentication**: None required
- **Required**: `sessionId` in request body

## Request Format

```json
{
  "content": "Your message here",
  "conversationId": "optional-conversation-id",
  "sessionId": "required-for-guest-endpoint",
  "xCoordinate": 100,
  "yCoordinate": 200,
  "responseXCoordinate": 300,
  "responseYCoordinate": 400,
  "sourceHandle": "optional",
  "targetHandle": "optional",
  "sources": ["optional-source-message-ids"]
}
```

## Response Format (Server-Sent Events)

### Chunk Events
```
data: {"type":"chunk","data":"Hello","promptMessageId":"300"}
```

### Completion Event
```
data: {"type":"complete","data":{"conversationId":"167","documents":[...],"response":"Full response","promptMessageId":"300","responseMessageId":"301","createdEdges":[...]}}
```

### Error Events
```
data: {"type":"error","error":"Error message"}
```

### Done Event
```
data: [DONE]
```

## Frontend Implementation Examples

### React Hook for Streaming

```typescript
import { useState, useCallback } from 'react';

interface StreamingResponse {
  type: 'chunk' | 'complete' | 'error';
  data?: string;
  promptMessageId?: string;
  error?: string;
  // Add other fields from completion event as needed
}

export function useMessageStreaming() {
  const [isStreaming, setIsStreaming] = useState(false);
  const [streamedContent, setStreamedContent] = useState('');
  const [error, setError] = useState<string | null>(null);

  const streamMessage = useCallback(async (
    content: string,
    isGuest: boolean = false,
    sessionId?: string,
    conversationId?: number,
    authToken?: string
  ) => {
    setIsStreaming(true);
    setStreamedContent('');
    setError(null);

    const endpoint = isGuest ? '/api/messages/guest/stream' : '/api/messages/stream';
    
    const requestBody: any = {
      content: content,
    };

    if (isGuest && sessionId) {
      requestBody.sessionId = sessionId;
    }
    
    if (conversationId) {
      requestBody.conversationId = conversationId;
    }

    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };

    if (!isGuest && authToken) {
      headers['Authorization'] = `Bearer ${authToken}`;
    }

    try {
      const response = await fetch(endpoint, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(requestBody)
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      if (!response.body) {
        throw new Error('ReadableStream not supported');
      }

      const reader = response.body.getReader();
      const decoder = new TextDecoder();
      let buffer = '';

      while (true) {
        const { done, value } = await reader.read();
        
        if (done) break;
        
        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split('\n');
        buffer = lines.pop() || '';

        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const data = line.slice(6);
            
            if (data === '[DONE]') {
              setIsStreaming(false);
              return;
            }

            try {
              const event: StreamingResponse = JSON.parse(data);
              
              switch (event.type) {
                case 'chunk':
                  setStreamedContent(prev => prev + (event.data || ''));
                  break;
                case 'complete':
                  console.log('Message complete:', event);
                  setIsStreaming(false);
                  break;
                case 'error':
                  setError(event.error || 'Unknown error');
                  setIsStreaming(false);
                  break;
              }
            } catch (e) {
              console.error('Failed to parse event:', e);
            }
          }
        }
      }
    } catch (error) {
      console.error('Streaming error:', error);
      setError(error instanceof Error ? error.message : 'Unknown error');
      setIsStreaming(false);
    }
  }, []);

  return { 
    streamMessage, 
    isStreaming, 
    streamedContent, 
    error,
    resetContent: () => setStreamedContent('')
  };
}
```

### React Component Example

```tsx
import React, { useState } from 'react';
import { useMessageStreaming } from './useMessageStreaming';

interface ChatComponentProps {
  isGuest?: boolean;
  sessionId?: string;
  conversationId?: number;
  authToken?: string;
}

export function ChatComponent({ 
  isGuest = false, 
  sessionId, 
  conversationId, 
  authToken 
}: ChatComponentProps) {
  const [message, setMessage] = useState('');
  const { streamMessage, isStreaming, streamedContent, error, resetContent } = useMessageStreaming();

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!message.trim() || isStreaming) return;

    resetContent();
    await streamMessage(message, isGuest, sessionId, conversationId, authToken);
    setMessage('');
  };

  return (
    <div className="chat-component">
      <div className="messages">
        {/* Render previous messages here */}
        
        {/* Current streaming message */}
        {streamedContent && (
          <div className="message assistant-message">
            <div className="content">
              {streamedContent}
              {isStreaming && <span className="cursor">|</span>}
            </div>
          </div>
        )}
        
        {error && (
          <div className="error-message">
            Error: {error}
          </div>
        )}
      </div>

      <form onSubmit={handleSendMessage} className="message-form">
        <input
          type="text"
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          placeholder="Type your message..."
          disabled={isStreaming}
        />
        <button type="submit" disabled={isStreaming || !message.trim()}>
          {isStreaming ? 'Sending...' : 'Send'}
        </button>
      </form>
    </div>
  );
}
```

### JavaScript (Vanilla) Example

```javascript
class MessageStreamer {
  constructor(baseUrl, isGuest = false) {
    this.baseUrl = baseUrl;
    this.isGuest = isGuest;
    this.isStreaming = false;
  }

  async streamMessage(content, options = {}) {
    const {
      sessionId,
      conversationId,
      authToken,
      onChunk,
      onComplete,
      onError
    } = options;

    this.isStreaming = true;
    
    const endpoint = this.isGuest ? 
      `${this.baseUrl}/api/messages/guest/stream` : 
      `${this.baseUrl}/api/messages/stream`;
    
    const requestBody = { content };
    
    if (this.isGuest && sessionId) {
      requestBody.sessionId = sessionId;
    }
    
    if (conversationId) {
      requestBody.conversationId = conversationId;
    }

    const headers = {
      'Content-Type': 'application/json'
    };

    if (!this.isGuest && authToken) {
      headers['Authorization'] = `Bearer ${authToken}`;
    }

    try {
      const response = await fetch(endpoint, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(requestBody)
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const reader = response.body.getReader();
      const decoder = new TextDecoder();
      let buffer = '';

      while (true) {
        const { done, value } = await reader.read();
        
        if (done) break;
        
        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split('\n');
        buffer = lines.pop() || '';

        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const data = line.slice(6);
            
            if (data === '[DONE]') {
              this.isStreaming = false;
              return;
            }

            try {
              const event = JSON.parse(data);
              
              switch (event.type) {
                case 'chunk':
                  onChunk?.(event.data, event.promptMessageId);
                  break;
                case 'complete':
                  onComplete?.(event.data);
                  break;
                case 'error':
                  onError?.(event.error);
                  break;
              }
            } catch (e) {
              console.error('Failed to parse event:', e);
            }
          }
        }
      }
    } catch (error) {
      this.isStreaming = false;
      onError?.(error.message);
    }
  }
}

// Usage example
const streamer = new MessageStreamer('http://localhost:5000', true); // for guest

streamer.streamMessage('Hello world', {
  sessionId: 'my-session-id',
  onChunk: (chunk, promptMessageId) => {
    // Append chunk to UI
    document.getElementById('response').textContent += chunk;
  },
  onComplete: (data) => {
    console.log('Complete response data:', data);
  },
  onError: (error) => {
    console.error('Streaming error:', error);
  }
});
```

## Next.js API Route Integration

```typescript
// pages/api/chat-proxy.ts or app/api/chat-proxy/route.ts
import { NextRequest, NextResponse } from 'next/server';

export async function POST(request: NextRequest) {
  const body = await request.json();
  const authHeader = request.headers.get('authorization');
  
  const response = await fetch('http://localhost:5000/api/messages/stream', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      ...(authHeader && { 'Authorization': authHeader })
    },
    body: JSON.stringify(body)
  });

  // Return the streaming response
  return new NextResponse(response.body, {
    headers: {
      'Content-Type': 'text/event-stream',
      'Cache-Control': 'no-cache',
      'Connection': 'keep-alive',
    },
  });
}
```

## CSS for Streaming Effect

```css
.message.assistant-message .content {
  position: relative;
}

.cursor {
  animation: blink 1s infinite;
  opacity: 1;
}

@keyframes blink {
  0%, 50% { opacity: 1; }
  51%, 100% { opacity: 0; }
}

.streaming-message {
  border-left: 3px solid #007bff;
  padding-left: 10px;
  background: rgba(0, 123, 255, 0.05);
}
```

## Error Handling Best Practices

1. **Network Issues**: Handle connection drops gracefully
2. **Authentication**: Refresh tokens when they expire
3. **Rate Limiting**: Implement client-side rate limiting
4. **Fallback**: Fall back to non-streaming API if streaming fails
5. **User Feedback**: Show loading states and error messages

## Testing Tips

1. Use `curl` to test endpoints during development
2. Test with network throttling to simulate slow connections
3. Test token expiration scenarios
4. Test with very long responses
5. Test cancellation (user navigating away)

## Performance Considerations

1. **Memory**: Clear old streaming content to prevent memory leaks
2. **DOM Updates**: Batch DOM updates for better performance
3. **Cancellation**: Cancel requests when components unmount
4. **Debouncing**: Debounce rapid user inputs

This implementation provides a solid foundation for integrating message streaming into your frontend application!
