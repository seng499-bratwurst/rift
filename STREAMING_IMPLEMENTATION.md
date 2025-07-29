# Message Streaming Implementation

This implementation adds real-time message streaming capabilities to the Rift API using Server-Sent Events (SSE). The LLM responses are now streamed in chunks as they are generated, providing a better user experience.

## New Endpoints

### 1. Authenticated User Streaming
- **Endpoint**: `POST /api/messages/stream`
- **Authentication**: Required (JWT Bearer token)
- **Content-Type**: `text/event-stream`

### 2. Guest User Streaming
- **Endpoint**: `POST /api/messages/guest/stream`
- **Authentication**: None required
- **Content-Type**: `text/event-stream`
- **Required**: SessionId in request body

## Request Format

Both endpoints use the same request format as the regular message endpoints:

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

## Response Format

The streaming endpoints return Server-Sent Events with the following event types:

### 1. Chunk Events
Sent for each piece of generated content:
```
data: {"type":"chunk","data":"Hello","promptMessageId":"message-id"}
```

### 2. Completion Event
Sent when the response is complete:
```
data: {"type":"complete","data":{"conversationId":"id","documents":[...],"response":"Full response","promptMessageId":"id","responseMessageId":"id","createdEdges":[...]}}
```

### 3. Error Events
Sent if an error occurs:
```
data: {"type":"error","error":"Error message"}
```

### 4. Done Event
Sent when the stream is finished:
```
data: [DONE]
```

## Client Implementation Examples

### JavaScript (Browser)

```javascript
async function streamMessage(content, isGuest = false, sessionId = null) {
    const endpoint = isGuest ? '/api/messages/guest/stream' : '/api/messages/stream';
    
    const requestBody = {
        content: content,
        ...(isGuest && { sessionId: sessionId })
    };

    const headers = {
        'Content-Type': 'application/json'
    };

    // Add authorization header for authenticated users
    if (!isGuest) {
        headers['Authorization'] = `Bearer ${getJwtToken()}`;
    }

    const response = await fetch(endpoint, {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(requestBody)
    });

    if (!response.body) {
        throw new Error('ReadableStream not supported');
    }

    const reader = response.body.getReader();
    const decoder = new TextDecoder();
    let buffer = '';

    try {
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
                        console.log('Stream completed');
                        return;
                    }

                    try {
                        const event = JSON.parse(data);
                        handleStreamEvent(event);
                    } catch (e) {
                        console.error('Failed to parse event:', e);
                    }
                }
            }
        }
    } finally {
        reader.releaseLock();
    }
}

function handleStreamEvent(event) {
    switch (event.type) {
        case 'chunk':
            // Append chunk to the UI
            appendToMessage(event.data);
            break;
        case 'complete':
            // Handle completion
            console.log('Message complete:', event.data);
            break;
        case 'error':
            // Handle error
            console.error('Stream error:', event.error);
            break;
    }
}
```

### React Hook Example

```javascript
import { useState, useCallback } from 'react';

export function useMessageStreaming() {
    const [isStreaming, setIsStreaming] = useState(false);
    const [streamedContent, setStreamedContent] = useState('');

    const streamMessage = useCallback(async (content, isGuest = false, sessionId = null) => {
        setIsStreaming(true);
        setStreamedContent('');

        const endpoint = isGuest ? '/api/messages/guest/stream' : '/api/messages/stream';
        
        const requestBody = {
            content: content,
            ...(isGuest && { sessionId: sessionId })
        };

        const headers = {
            'Content-Type': 'application/json'
        };

        if (!isGuest) {
            headers['Authorization'] = `Bearer ${getJwtToken()}`;
        }

        try {
            const response = await fetch(endpoint, {
                method: 'POST',
                headers: headers,
                body: JSON.stringify(requestBody)
            });

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
                            const event = JSON.parse(data);
                            if (event.type === 'chunk') {
                                setStreamedContent(prev => prev + event.data);
                            }
                        } catch (e) {
                            console.error('Failed to parse event:', e);
                        }
                    }
                }
            }
        } catch (error) {
            console.error('Streaming error:', error);
            setIsStreaming(false);
        }
    }, []);

    return { streamMessage, isStreaming, streamedContent };
}
```

### curl Example

```bash
# Authenticated user
curl -X POST "http://localhost:5000/api/messages/stream" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"content":"What is the weather like?"}' \
  --no-buffer

# Guest user
curl -X POST "http://localhost:5000/api/messages/guest/stream" \
  -H "Content-Type: application/json" \
  -d '{"content":"What is the weather like?","sessionId":"guest-session-123"}' \
  --no-buffer
```

## Architecture Changes

### 1. LLM Provider Interface
- Added `IAsyncEnumerable<string> StreamFinalResponseRAG(Prompt prompt)`
- Added `IAsyncEnumerable<string> StreamFinalResponse(string prompt, JsonElement onc_api_response)`

### 2. LLM Providers Updated
- **TogetherAI**: Implemented streaming with proper SSE parsing
- **HuggingFace**: Implemented streaming with proper SSE parsing
- **GoogleGemma**: Implemented streaming with proper SSE parsing

### 3. RAG Service
- Added `IAsyncEnumerable<(string contentChunk, List<string> relevantDocTitles)> StreamResponseAsync(...)`
- Maintains the same RAG pipeline but streams the final LLM response

### 4. Message Controller
- Added `/api/messages/stream` for authenticated users
- Added `/api/messages/guest/stream` for guest users
- Both endpoints use Server-Sent Events (SSE) format

## Benefits

1. **Real-time feedback**: Users see the response being generated in real-time
2. **Better UX**: No waiting for the complete response before seeing any content
3. **Reduced perceived latency**: Users can start reading while the response is being generated
4. **Maintained functionality**: All existing features (RAG, document retrieval, message storage) still work
5. **Backwards compatibility**: Original non-streaming endpoints remain unchanged

## Performance Considerations

- Streaming reduces the time to first byte (TTFB) for responses
- Network usage is similar but distributed over time
- Server resources are held for longer but with better user experience
- Client needs to handle partial content rendering

## Error Handling

- Network interruptions are handled gracefully
- JSON parsing errors in streaming data are logged but don't break the stream
- Server errors are sent as error events to the client
- Cancellation tokens are used to handle client disconnections
