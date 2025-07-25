# Company API Documentation

## Overview

The Company API provides external companies with access to our LLM (Large Language Model) and RAG (Retrieval-Augmented Generation) services. This endpoint allows companies to integrate our AI-powered question-answering capabilities directly into their own applications and workflows.

## Endpoint

**POST** `/api/messages/company`

## Authentication

The endpoint requires a company-specific API token provided in the request header:

```http
X-Company-Token: your-company-token-here
```

**Important**: Company tokens must be pre-registered in our system. Contact our team to obtain your company's API token.

## Rate Limiting

- **Token Bucket Rate Limiting**: 50 requests per hour per company token
- **Bucket Capacity**: 50 tokens
- **Replenishment Rate**: 50 tokens per hour
- Rate limits are applied per company token, allowing each company independent access quotas. If you require more requests please contact our team.

## Request Format

### Headers

```http
Content-Type: application/json
X-Company-Token: your-company-token-here
```

### Request Body

```json
{
  "content": "Your question or prompt here",
  "messageHistory": [
    {
      "role": "user",
      "content": "Previous user message (optional)"
    },
    {
      "role": "assistant", 
      "content": "Previous AI response (optional)"
    }
  ]
}
```

#### Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `content` | string | Yes | The user's message/question to be processed by the LLM |
| `messageHistory` | array | No | Optional conversation history to provide context. Should be ordered chronologically with alternating user/assistant messages |

#### Message History Item

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `role` | string | Yes | Must be either "user" or "assistant" |
| `content` | string | Yes | The content of the message |

## Response Format

### Success Response (200 OK)

```json
{
  "success": true,
  "error": null,
  "data": {
    "response": "The AI-generated response to your question...",
    "documents": [
      {
        "id": 123,
        "title": "Relevant Document Title",
        "filePath": "/path/to/document.pdf",
        "sourceType": "PDF",
        "uploadedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

### Error Responses

#### 400 Bad Request - Empty Content

```json
{
  "success": false,
  "error": "Message content cannot be empty.",
  "data": null
}
```

#### 401 Unauthorized - Missing Token

```json
{
  "success": false,
  "error": "Company token is required in X-Company-Token header.",
  "data": null
}
```

#### 401 Unauthorized - Invalid Token

```json
{
  "success": false,
  "error": "Invalid company token.",
  "data": null
}
```

#### 429 Too Many Requests - Rate Limit Exceeded

```json
{
  "success": false,
  "error": "Rate limit exceeded. Please try again later.",
  "data": null
}
```

#### 500 Internal Server Error

```json
{
  "success": false,
  "error": "An error occurred while processing your request.",
  "data": null
}
```

## Example Usage

### Basic Question (No Context)

**Request:**

```bash
curl -X POST "https://your-api-domain.com/api/messages/company" \
  -H "Content-Type: application/json" \
  -H "X-Company-Token: your-company-token-here" \
  -d '{
    "content": "What are the effects of climate change on Arctic sea ice?"
  }'
```

**Response:**

```json
{
  "success": true,
  "error": null,
  "data": {
    "response": "Climate change has significant impacts on Arctic sea ice, including accelerated melting rates, reduced ice thickness, and shorter ice seasons. Studies have shown that Arctic sea ice extent has been declining at a rate of approximately 13% per decade since 1979...",
    "documents": [
      {
        "id": 456,
        "title": "Arctic Sea Ice Climate Research Paper",
        "filePath": "/documents/arctic_climate_study.pdf",
        "sourceType": "PDF",
        "uploadedAt": "2024-01-10T14:22:00Z"
      },
      {
        "id": 789,
        "title": "NOAA Arctic Report 2023",
        "filePath": "/documents/noaa_arctic_report_2023.pdf", 
        "sourceType": "PDF",
        "uploadedAt": "2024-02-05T09:15:00Z"
      }
    ]
  }
}
```

### Question with Conversation Context

**Request:**

```bash
curl -X POST "https://your-api-domain.com/api/messages/company" \
  -H "Content-Type: application/json" \
  -H "X-Company-Token: your-company-token-here" \
  -d '{
    "content": "How does this relate to ocean temperatures?",
    "messageHistory": [
      {
        "role": "user",
        "content": "What are the effects of climate change on Arctic sea ice?"
      },
      {
        "role": "assistant",
        "content": "Climate change has significant impacts on Arctic sea ice, including accelerated melting rates, reduced ice thickness, and shorter ice seasons..."
      }
    ]
  }'
```

**Response:**

```json
{
  "success": true,
  "error": null,
  "data": {
    "response": "The relationship between Arctic sea ice decline and ocean temperatures is closely interconnected. As ocean temperatures rise, they contribute to ice melting from below, creating a feedback loop where less ice cover allows more solar radiation to be absorbed by darker ocean water, further warming the sea...",
    "documents": [
      {
        "id": 321,
        "title": "Ocean-Ice Interaction Study",
        "filePath": "/documents/ocean_ice_study.pdf",
        "sourceType": "PDF",
        "uploadedAt": "2024-01-20T11:45:00Z"
      }
    ]
  }
}
```

## Integration Guidelines

### Best Practices

1. **Provide Context**: Include relevant conversation history to get more accurate and contextual responses
2. **Handle Rate Limits**: Implement exponential backoff when receiving 429 responses
3. **Error Handling**: Always check the `success` field in responses and handle errors appropriately
4. **Secure Token Storage**: Store your company token securely and never expose it in client-side code

### Message History Guidelines

- Keep message history concise but relevant (last 5-10 exchanges typically sufficient)
- Ensure messages alternate between "user" and "assistant" roles
- Order messages chronologically (oldest first)
- Remove any sensitive information from historical context

### Error Handling Example

```javascript
async function queryCompanyAPI(content, messageHistory = []) {
  try {
    const response = await fetch('/api/messages/company', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-Company-Token': process.env.COMPANY_API_TOKEN
      },
      body: JSON.stringify({
        content,
        messageHistory
      })
    });

    const data = await response.json();
    
    if (!data.success) {
      throw new Error(data.error || 'API request failed');
    }
    
    return data.data;
    
  } catch (error) {
    if (error.status === 429) {
      // Rate limit exceeded - implement backoff
      console.log('Rate limit exceeded, retrying after delay...');
      await new Promise(resolve => setTimeout(resolve, 60000)); // Wait 1 minute
      return queryCompanyAPI(content, messageHistory); // Retry
    }
    
    console.error('API Error:', error.message);
    throw error;
  }
}
```

## Support

For technical support, API token requests, or integration assistance, please contact our development team.

---

**Last Updated:** July 24, 2025  
**API Version:** 1.0
