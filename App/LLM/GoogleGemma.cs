using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Rift.App.Models;

namespace Rift.LLM
{
    /// <summary>
    /// LLM provider for Google's Gemma 3n model, implementing ILlmProvider.
    /// </summary>
    public class GoogleGemma : ILlmProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _oncModelName;
        private readonly string _finalModelName;
        private readonly OncFunctionParser _parser;
        // private readonly OncAPI _oncApiClient;

        /// <summary>
        /// Constructor reads values from appsettings.json
        /// </summary>
        public GoogleGemma(IConfiguration config, OncFunctionParser parser)
        {
            _httpClient = new HttpClient();

            // Example config section:
            // "LLMSettings": {
            //   "GoogleGemma": {
            //     "ApiKey": "",
            //     "Endpoint": "https://generativelanguage.googleapis.com/v1beta/openai/chat/completions",
            //     "ModelName": "gemma-3n"
            //   }
            // }
            _apiKey = config["LLMSettings:GoogleGemma:ApiKey"] ?? throw new ArgumentNullException("GoogleGemma ApiKey missing in config");
            _endpoint = config["LLMSettings:GoogleGemma:Endpoint"] ?? throw new ArgumentNullException("GoogleGemma Endpoint missing in config");
            _oncModelName = config["LLMSettings:GoogleGemma:ONCModelName"] ?? "gemma-3n-e4b-it";
            _finalModelName = config["LLMSettings:GoogleGemma:FinalModelName"] ?? "gemini-2.5-flash";
            _parser = parser;
        }

        /// <summary>
        /// Sends a prompt to the Gemma 3n model to generate an ONC API call.
        /// </summary>
        public async Task<string> GatherOncAPIData(string prompt)
        {
            string systemPrompt;
            // All property codes from the Cambridge Bay observatory
            // string[] propertyCodes = {
            //     "absolutebarometricpressure",
            //     "absolutehumidity", 
            //     "airdensity",
            //     "airtemperature",
            //     "dewpoint",
            //     "magneticheading",
            //     "mixingratio",
            //     "relativebarometricpressure",
            //     "relativehumidity",
            //     "solarradiation",
            //     "specificenthalpy",
            //     "wetbulbtemperature",
            //     "windchilltemperature",
            //     "winddirection",
            //     "windspeed",
            //     "conductivity",
            //     "density",
            //     "oxygen",
            //     "pressure",
            //     "salinity",
            //     "seawatertemperature",
            //     "soundspeed",
            //     "turbidityntu",
            //     "chlorophyll",
            //     "icedraft",
            //     "parphotonbased",
            //     "ph",
            //     "sigmatheta"
            // };

            // string pattern = @"\b(" + string.Join("|", propertyCodes) + @")\b";
            // Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // bool hasPropertyCode = regex.IsMatch(prompt);

            // systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "filter4.md"));
            systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "filter5.md"));

            // if (hasPropertyCode){
            //     // Console.WriteLine("using filter4.md");
            //     systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "filter4.md"));
            // }else{
            //     // Console.WriteLine("using function_call_required_or_not.md");
            //     systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "filter4.md"));
            //     // systemPrompt = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "LLM/SystemPrompts", "function_call_required_or_not.md"));
            // }

            var messages = new List<object>{
                new { role = "system", content = systemPrompt },
                new { role = "user", content = prompt }
            };


            
            while (true){
                object? generalResponse = null;
                var payload = new
                {
                    model = _oncModelName,
                    messages = messages,
                    tools = new object[]
                    {
                        new {
                            type = "function",
                            function = new {
                                name = "scalardata_location",
                                description = "Returns scalar sensor data for a given location and device category, filtered by property and options like latest data and row limits.",
                                parameters = new {
                                    type = "object",
                                    properties = new {
                                        locationCode = new {
                                            type = "string",
                                            description = "Return scalar data from a specific location."
                                        },
                                        deviceCategoryCode = new {
                                            type = "string",
                                            description = "Return scalar data belonging to a specific device category code."
                                        },
                                        propertyCode = new {
                                            type = "string",
                                            description = "Comma-separated list of property codes to fetch data for."
                                        },
                                        getLatest = new {
                                            type = "boolean",
                                            description = "Return the latest readings first. Default is true."
                                        },
                                        rowLimit = new {
                                            type = "integer",
                                            description = "Number of scalar data rows to return per sensor code. Default is 10."
                                        }
                                    },
                                    required = new string[] { "locationCode", "deviceCategoryCode", "propertyCode", "getLatest", "rowLimit" }
                                }
                            }
                        },
                        new {
                            type = "function",
                            function = new {
                                name = "locations_tree",
                                description = "Returns all sub-locations (child nodes) of Cambridge Bay. Useful for discovering locations with available data that can be used to query scalar properties.",
                                parameters = new {
                                    type = "object",
                                    properties = new {
                                        locationCode = new {
                                            type = "string",
                                            description = "Exact location code to get sub-locations from. Default for Cambridge Bay is CBY."
                                        },
                                        propertyCode = new {
                                            type = "string",
                                            description = "Property code of interest (e.g., seawatertemperature)."
                                        },
                                        dataProductCode = new {
                                            type = "string",
                                            description = "Filter by supported data product code."
                                        },
                                        dateFrom = new {
                                            type = "string",
                                            description = "Deployment start date (ISO 8601)."
                                        },
                                        dateTo = new {
                                            type = "string",
                                            description = "Deployment end date (ISO 8601)."
                                        }
                                    },
                                    required = new string[] { "locationCode", "propertyCode", "dateFrom", "dateTo" }
                                }
                            }
                        },
                        new {
                            type = "function",
                            function = new {
                                name = "deployments",
                                description = "Returns all deployments of devices at specified locations within a time window, useful for checking when and where data is available.",
                                parameters = new {
                                    type = "object",
                                    properties = new {
                                        locationCode = new {
                                            type = "string",
                                            description = "Filter by exact location code (e.g., BACAX)."
                                        },
                                        deviceCategoryCode = new {
                                            type = "string",
                                            description = "Filter by device category (e.g., CTD)."
                                        },
                                        deviceCode = new {
                                            type = "string",
                                            description = "Filter by specific device code."
                                        },
                                        propertyCode = new {
                                            type = "string",
                                            description = "Filter by property measured (e.g., conductivity)."
                                        },
                                        dateFrom = new {
                                            type = "string",
                                            description = "Deployment start date (ISO 8601)."
                                        },
                                        dateTo = new {
                                            type = "string",
                                            description = "Deployment end date (ISO 8601)."
                                        }
                                    },
                                    required = new string[] { "locationCode", "propertyCode", "dateFrom", "dateTo" }
                                }
                            }
                    }, 
                    new{
                       type = "function",
                            function = new {
                                name = "devices",
                                description = "Returns all devices at based on the filter criteria",
                                parameters = new {
                                    type = "object",
                                    properties = new {
                                        locationCode = new {
                                            type = "string",
                                            description = "Filter by exact location code (e.g., BACAX)."
                                        },
                                        deviceCategoryCode = new {
                                            type = "string",
                                            description = "Filter by device category (e.g., CTD)."
                                        },
                                        deviceCode = new {
                                            type = "string",
                                            description = "Filter by specific device code."
                                        },
                                        propertyCode = new {
                                            type = "string",
                                            description = "Filter by property measured (e.g., conductivity)."
                                        },
                                        dateFrom = new {
                                            type = "string",
                                            description = "Deployment start date (ISO 8601)."
                                        },
                                        dateTo = new {
                                            type = "string",
                                            description = "Deployment end date (ISO 8601)."
                                        },
                                        includeChildren = new {
                                            type = "boolean",
                                            description = "Return all devices that are deployed at a specific location and sub-tree locations. always true for Cambridge Bay"
                                        },
                                        dataProductCode = new {
                                            type = "string",
                                            description = "Return all devices that have the ability to return a specific data product code."
                                        },
                                        deviceId = new {
                                            type = "string",
                                            description = "Return a single device matching a specific device ID."
                                        },
                                        deviceName = new {
                                            type = "string",
                                            description = "Return all devices where the device name contains a keyword."
                                    }
                                }
                            }
                        } 
                    },
                    new{
                       type = "function",
                            function = new {
                                name = "properties",
                                description = "returns all properties defined in Oceans 3.0 that meet a filter criteria.",
                                parameters = new {
                                    type = "object",
                                    properties = new {
                                        locationCode = new {
                                            type = "string",
                                            description = "Filter by exact location code (e.g., BACAX)."
                                        },
                                        deviceCategoryCode = new {
                                            type = "string",
                                            description = "Filter by device category (e.g., CTD)."
                                        },
                                        deviceCode = new {
                                            type = "string",
                                            description = "Filter by specific device code."
                                        },
                                        propertyCode = new {
                                            type = "string",
                                            description = "Filter by property measured (e.g., conductivity)."
                                        },
                                        propertyName = new {
                                            type = "string",
                                            description = "Return all properties where the property name contains a keyword."
                                        },
                                        description = new {
                                            type = "string",
                                            description = "Return all properties where the description contains a keyword."
                                        }
                                    }
                                }
                            }
                        } 
                    },
                    tool_choice = "auto"
                };

                var json = JsonSerializer.Serialize(payload);

                var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.SendAsync(request);
                Console.WriteLine($"Response: {response}");
        
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException("Failed to Generate ONC data using small LLM:\n" +
                    $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
                }

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                using var doc = JsonDocument.Parse(responseContent);
                var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");
                string? content;
                string? functionCallName;
                string? functionCallParams;
                

                if (message.TryGetProperty("tool_calls", out var toolCalls) && toolCalls.GetArrayLength() > 0){
                    functionCallName = toolCalls[0].GetProperty("function").GetProperty("name").GetString();
                    if (functionCallName == null)
                    {
                        throw new Exception("Function Call Name is null");
                    }
                    // Console.WriteLine($"Function Call Name: {functionCallName}");
                    functionCallParams = toolCalls[0].GetProperty("function").GetProperty("arguments").GetString();
                    if (functionCallParams == null)
                    {
                        throw new Exception("Function Call Params is null");
                    }
                    messages.Add(new {
                        role = "assistant",
                        tool_calls = new[]
                        {
                            new {
                            function = new {
                                arguments = functionCallParams,
                                name = functionCallName,
                            },
                            id = functionCallName,
                            type = "function"
                            }
                        }
                    });

                    // Console.WriteLine($"Function Call Params: {functionCallParams}");
                    var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(functionCallName, functionCallParams);
                    Console.WriteLine($"Function Name: {functionName}");
                    Console.WriteLine($"Function Params: {functionParams}");
                    var result = await _parser.OncAPICall(functionName, functionParams);
                    messages.Add(new {
                        tool_call_id = functionCallName,
                        role = "tool",
                        name = functionCallName,
                        content = result,
                    });
                 Console.WriteLine($"MESSAGES: {messages}");
                    continue;
                    
                }else if (message.TryGetProperty("content", out var contentElement))
                {
                    content = contentElement.GetString();
                    if (content == null)
                    {
                        throw new Exception("Content is null");
                    }
                    Console.WriteLine($"Content: {content}");
                    generalResponse = new {
                        response = content,
                        message = "ONC API call not required. Answer based on the user prompt."
                    };
                    return JsonSerializer.Serialize(generalResponse);
                }
            };
        }

        /// <summary>
        /// Sends a prompt and ONC API response to the Gemma 3n model to generate a final user-facing answer.
        /// </summary>
        /// !!! DEPRECATED  !!!
        /// The message pipeline from the frontend uses the GenerateFinalResponseRAG method instead.
        /// Just keeping this so Ishan is happy :)
        public async Task<string> GenerateFinalResponse(string prompt, JsonElement onc_api_response)
        {
            string jsonInput = JsonSerializer.Serialize(onc_api_response, new JsonSerializerOptions { WriteIndented = true });
            var systemPrompt = "You are a helpful Ocean Networks Canada assistant that interprets the data given and answers the user prompt with accuracy.";

            string fullPrompt = $"{prompt}\n\nHere is the ONC API response:\n{jsonInput}";

            var payload = new
            {
                model = _finalModelName,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = fullPrompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result ?? "No response from Gemma 3n model.";
        }

        /// <summary>
        /// Sends a prompt, chat history, relevant documents, and ONC API response to the Gemma 3n model to generate a final user-facing answer.
        /// </summary>
        public async Task<string> GenerateFinalResponseRAG(Prompt prompt)
        {

            var payload = new
            {
                model = _finalModelName,
                messages = prompt.Messages,
                stream = false,
                temperature = 0.5 // Answers seem a bit more reliable with lower temperature
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
                throw new HttpRequestException("Failed to Generate response from Large LLM:\n" +
                $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            return result ?? "No response from Gemma model.";
        }

    }
}



// THIS IS OLD CODE FROM THE GatherOncAPIData method WILL BE DELETING IT LATER
// var payload = new
            // {
            //     model = _oncModelName,
            //     messages = new[]
            //     {
            //         new { role = "system", content = systemPrompt },
            //         new { role = "user", content = prompt }
            //     },
            //     tools = new[]
            //     {
            //         new {
            //             type = "function",
            //             function = new {
            //                 name = "scalardata_location",
            //                 description = "Returns scalar sensor data for a given location and device category, filtered by property and options like latest data and row limits.",
            //                 parameters = new {
            //         type = "object",
            //         properties = new {
            //             locationCode = new {
            //                 type = "string",
            //                 description = "Return scalar data from a specific location."
            //             },
            //             deviceCategoryCode = new {
            //                 type = "string",
            //                 description = "Return scalar data belonging to a specific device category code."
            //             },
            //             propertyCode = new {
            //                 type = "string",
            //                 description = "Comma-separated list of property codes to fetch data for."
            //             },
            //             getLatest = new {
            //                 type = "boolean",
            //                 description = "Return the latest readings first. Default is true."
            //             },
            //             rowLimit = new {
            //                 type = "integer",
            //                 description = "Number of scalar data rows to return per sensor code. Default is 10."
            //             }
            //         },
            //         required = new[] { "locationCode", "deviceCategoryCode", "propertyCode", "getLatest", "rowLimit" }
            //                 }
            //             }
            //         }
            //     },
            //     tool_choice = "auto"
            // };

            // var json = JsonSerializer.Serialize(payload);

            // var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            // {
            //     Content = new StringContent(json, Encoding.UTF8, "application/json")
            // };
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            // var response = await _httpClient.SendAsync(request);
            // Console.WriteLine($"Response: {response}");
    
            // if (!response.IsSuccessStatusCode)
            // {
            //     var errorContent = await response.Content.ReadAsStringAsync();
            //     throw new HttpRequestException("Failed to Generate ONC data using small LLM:\n" +
            //     $"Status Code:{response.StatusCode}\nError: {errorContent}\n");
            // }

            // response.EnsureSuccessStatusCode();

            // var responseContent = await response.Content.ReadAsStringAsync();
            // Console.WriteLine($"Response Content: {responseContent}");

            // using var doc = JsonDocument.Parse(responseContent);
            // var message = doc.RootElement.GetProperty("choices")[0].GetProperty("message");
            // string? content;
            // string? functionCallName;
            // string? functionCallParams;
            //  object? generalResponse = null;

            // if (message.TryGetProperty("tool_calls", out var toolCalls) && toolCalls.GetArrayLength() > 0){
            //     functionCallName = toolCalls[0].GetProperty("function").GetProperty("name").GetString();
            //     if (functionCallName == null)
            //     {
            //         throw new Exception("Function Call Name is null");
            //     }
            //     // Console.WriteLine($"Function Call Name: {functionCallName}");
            //     functionCallParams = toolCalls[0].GetProperty("function").GetProperty("arguments").GetString();
            //     if (functionCallParams == null)
            //     {
            //         throw new Exception("Function Call Params is null");
            //     }
            //     // Console.WriteLine($"Function Call Params: {functionCallParams}");
            //     var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(functionCallName, functionCallParams);
            //     Console.WriteLine($"Function Name: {functionName}");
            //     Console.WriteLine($"Function Params: {functionParams}");
            //     return await _parser.OncAPICall(functionName, functionParams);
                
            // }else{
            //     content = message.GetProperty("content").GetString();

            //     if (content == null)
            //     {
            //         throw new Exception("Content is null");
            //     }
            //     Console.WriteLine($"Content: {content}");


            //     // generalResponse = new {
            //     //     response = content,
            //     //     message = "ONC API call not required. Answer based on the user prompt."
            //     // };

            // }

            

    
            // var match = Regex.Match(content!, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))*\}(?(open)(?!))", RegexOptions.Singleline);

            // object? generalResponse = null;

            // generalResponse = new {
            //         response = content,
            //         message = "ONC API call not required. Answer based on the user prompt."
            //     };

            // if (!match.Success)
            // {
            //     generalResponse = new {
            //         response = content,
            //         message = "ONC API call not required. Answer based on the user prompt."
            //     };
            // }else
            // {
            //     String LLMContentFiltered = match.Value;
            //     Console.WriteLine($"Filtered Content: {LLMContentFiltered}");

            //     using var innerDoc = JsonDocument.Parse(LLMContentFiltered);

            //     // bool? useFunction = innerDoc.RootElement.GetProperty("use_function").GetBoolean();

            //     // if (useFunction == null)
            //     // {
            //     //     generalResponse = new {
            //     //         message = "ONC API call not required. Answer based on the user prompt.",
            //     //         response = LLMContentFiltered
            //     //     };
            //     //     return JsonSerializer.Serialize(generalResponse);
            //     // }
            //     // if (!useFunction)
            //     // {
            //     //     generalResponse = new {
            //     //         message = "ONC API call not required. Answer based on the user prompt."
            //     //     };

            //     //     return JsonSerializer.Serialize(generalResponse);
            //     // }
            //     // else if (useFunction)
            //     // {
            //     //     var (functionName, functionParams) = _parser.ExtractFunctionAndQueries(LLMContentFiltered);
            //     //     return await _parser.OncAPICall(functionName, functionParams);
            //     // }
            // }
            
            // return JsonSerializer.Serialize(generalResponse);
