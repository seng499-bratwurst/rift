# Frontend Streaming Implementation Fix

Based on your backend implementation, here are the corrections needed for your frontend:

## 1. Backend Endpoint Summary

Your backend has these streaming endpoints:
- `POST /api/messages/stream` - For authenticated users (requires JWT Bearer token)
- `POST /api/messages/guest/stream` - For guest users (requires sessionId)

## 2. Authentication Issue

The frontend was trying to use cookie authentication, but your streaming endpoints expect:
- **Authenticated endpoint**: JWT Bearer token in Authorization header
- **Guest endpoint**: No auth but requires sessionId in request body

## 3. Corrected Frontend Implementation

Here's the corrected streaming API function:

```typescript
// lib/api/create-prompt-streaming.ts
import { env } from "@/env";

export interface StreamingChunk {
  type: 'chunk' | 'complete' | 'error';
  data?: string;
  promptMessageId?: number;
  error?: string;
  // Completion event data
  conversationId?: number;
  documents?: any[];
  response?: string;
  responseMessageId?: number;
  createdEdges?: any[];
}

export interface StreamingCallbacks {
  onChunk?: (chunk: string, promptMessageId?: number) => void;
  onComplete?: (data: any) => void;
  onError?: (error: string) => void;
  onSettled?: () => void;
}

export async function createPromptStreaming(
  prompt: string,
  isGuest: boolean = true, // Default to guest mode
  options: {
    conversationId?: number;
    sessionId?: string;
    authToken?: string;
    xCoordinate?: number;
    yCoordinate?: number;
    responseXCoordinate?: number;
    responseYCoordinate?: number;
  } = {},
  callbacks: StreamingCallbacks = {}
): Promise<void> {
  const { 
    conversationId, 
    sessionId, 
    authToken,
    xCoordinate = 0,
    yCoordinate = 0,
    responseXCoordinate = 0,
    responseYCoordinate = 0
  } = options;
  
  const { onChunk, onComplete, onError, onSettled } = callbacks;

  // Choose endpoint based on authentication mode
  const endpoint = isGuest 
    ? `${env.NEXT_PUBLIC_API_URL}/api/messages/guest/stream`
    : `${env.NEXT_PUBLIC_API_URL}/api/messages/stream`;

  // Build request body
  const requestBody: any = {
    content: prompt,
    xCoordinate,
    yCoordinate,
    responseXCoordinate,
    responseYCoordinate
  };

  // Add required fields based on mode
  if (isGuest) {
    if (!sessionId) {
      throw new Error('sessionId is required for guest streaming');
    }
    requestBody.sessionId = sessionId;
  } else {
    if (conversationId) {
      requestBody.conversationId = conversationId;
    }
  }

  // Build headers
  const headers: Record<string, string> = {
    'Content-Type': 'application/json'
  };

  // Add auth header for authenticated users
  if (!isGuest && authToken) {
    headers['Authorization'] = `Bearer ${authToken}`;
  }

  try {
    const response = await fetch(endpoint, {
      method: 'POST',
      headers,
      body: JSON.stringify(requestBody),
      credentials: 'include' // Include cookies for session management
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

    try {
      while (true) {
        const { done, value } = await reader.read();
        
        if (done) break;
        
        buffer += decoder.decode(value, { stream: true });
        const lines = buffer.split('\n');
        buffer = lines.pop() || '';

        for (const line of lines) {
          if (line.startsWith('data: ')) {
            const data = line.slice(6).trim();
            
            if (data === '[DONE]') {
              return;
            }

            try {
              const event: StreamingChunk = JSON.parse(data);
              
              switch (event.type) {
                case 'chunk':
                  onChunk?.(event.data || '', event.promptMessageId);
                  break;
                case 'complete':
                  onComplete?.(event);
                  break;
                case 'error':
                  onError?.(event.error || 'Unknown error');
                  return;
              }
            } catch (e) {
              console.error('Failed to parse streaming event:', e, 'Data:', data);
            }
          }
        }
      }
    } finally {
      reader.releaseLock();
      onSettled?.();
    }
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'Unknown error';
    onError?.(errorMessage);
    onSettled?.();
  }
}
```

## 4. Updated Component Usage

For your graph chat component, use the guest mode since that's simpler:

```typescript
// In your graph-chat.tsx component
import { createPromptStreaming } from "@/lib/api/create-prompt-streaming";

// Generate a session ID for guest mode (you can store this in localStorage)
const getOrCreateSessionId = () => {
  let sessionId = localStorage.getItem('guestSessionId');
  if (!sessionId) {
    sessionId = 'guest-' + Date.now() + '-' + Math.random().toString(36).substr(2, 9);
    localStorage.setItem('guestSessionId', sessionId);
  }
  return sessionId;
};

// In your onSendPrompt function:
const onSendPrompt = async (prompt: string) => {
  const sessionId = getOrCreateSessionId();
  let streamingContent = '';
  
  // Create response node immediately
  const responseNode = createResponseNode(prompt);
  
  try {
    await createPromptStreaming(
      prompt,
      true, // isGuest = true
      {
        sessionId,
        xCoordinate: 100, // adjust as needed
        yCoordinate: 100,
        responseXCoordinate: 200,
        responseYCoordinate: 200
      },
      {
        onChunk: (chunk) => {
          streamingContent += chunk;
          // Update the response node with streaming content
          updateResponseNode(responseNode.id, streamingContent, true); // true = isStreaming
        },
        onComplete: (data) => {
          // Update with final content and mark as complete
          updateResponseNode(responseNode.id, data.response || streamingContent, false);
          // Handle documents, edges, etc. from data
        },
        onError: (error) => {
          console.error('Streaming error:', error);
          // Fallback to regular API or show error
          updateResponseNode(responseNode.id, `Error: ${error}`, false);
        }
      }
    );
  } catch (error) {
    console.error('Failed to start streaming:', error);
    // Fallback to regular API
  }
};
```

## 5. Key Differences from Previous Implementation

1. **Uses guest endpoint by default**: Simpler, no JWT required
2. **Proper sessionId handling**: Generates and stores a session ID
3. **Better error handling**: More specific error messages
4. **Credential handling**: Uses 'include' for cookies while also supporting Bearer tokens

## 6. Testing Steps

1. Make sure your backend is running on the correct port
2. Check that the streaming endpoints are available
3. Use guest mode first (easier to test)
4. Check browser network tab to see the actual requests being made

This should resolve the 404 errors and authentication issues you were experiencing!
