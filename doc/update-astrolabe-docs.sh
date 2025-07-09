#!/bin/bash

set -e

DOC_DIR="./doc"
YAML_OUT="$DOC_DIR/astrolabe-api-openapi-source.yaml"
JSON_TMP="$DOC_DIR/astrolabe-swagger-tmp.json"
SWAGGER_URL="http://localhost:5000/swagger/v1/swagger.json"

command -v swagger-cli >/dev/null 2>&1 || { echo >&2 "swagger-cli is not installed. Install with: npm install -g swagger-cli"; exit 1; }

echo "Downloading latest swagger.json..."
curl -sSL "$SWAGGER_URL" -o "$JSON_TMP"

echo "Validating swagger.json..."
swagger-cli validate "$JSON_TMP"

echo "Bundling and converting to YAML..."
swagger-cli bundle "$JSON_TMP" --outfile "$YAML_OUT" --type yaml

echo "Cleaning up..."
rm "$JSON_TMP"

echo "Documentation updated and saved to $YAML_OUT."