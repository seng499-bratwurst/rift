#!/bin/bash

# Simple test script to demonstrate the streaming endpoints
# Make sure the Rift application is running before executing this script

echo "Testing Message Streaming Implementation"
echo "========================================"

# Test the guest streaming endpoint
echo ""
echo "Testing Guest Streaming Endpoint..."
echo "Endpoint: POST /api/messages/guest/stream"
echo ""

# Generate a random session ID for the guest
SESSION_ID="test-session-$(date +%s)"

# Test streaming with curl (you'll need to replace localhost:5000 with your actual API URL)
curl -X POST "http://localhost:5000/api/messages/guest/stream" \
  -H "Content-Type: application/json" \
  -d "{\"content\":\"What is Ocean Networks Canada?\",\"sessionId\":\"$SESSION_ID\"}" \
  --no-buffer \
  -v

echo ""
echo ""
echo "Testing Authenticated User Streaming Endpoint..."
echo "Endpoint: POST /api/messages/stream"
echo "Note: You'll need to replace YOUR_JWT_TOKEN with an actual JWT token"
echo ""

# Test authenticated streaming (replace YOUR_JWT_TOKEN with an actual token)
# curl -X POST "http://localhost:5000/api/messages/stream" \
#   -H "Authorization: Bearer YOUR_JWT_TOKEN" \
#   -H "Content-Type: application/json" \
#   -d '{"content":"Tell me about oceanography"}' \
#   --no-buffer \
#   -v

echo "To test the authenticated endpoint, uncomment the curl command above"
echo "and replace YOUR_JWT_TOKEN with a valid JWT token."

echo ""
echo "Test completed!"
echo ""
echo "Expected Response Format:"
echo "========================"
echo "data: {\"type\":\"chunk\",\"data\":\"Ocean\",\"promptMessageId\":\"msg-id\"}"
echo "data: {\"type\":\"chunk\",\"data\":\" Networks\",\"promptMessageId\":\"msg-id\"}"
echo "data: {\"type\":\"chunk\",\"data\":\" Canada\",\"promptMessageId\":\"msg-id\"}"
echo "..."
echo "data: {\"type\":\"complete\",\"data\":{...}}"
echo "data: [DONE]"
