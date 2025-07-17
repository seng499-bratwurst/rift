using System.Collections.Generic;

namespace Rift.LLM.FunctionCalls
{
    public static class Tools
    {
        public static List<object> GetTools()
        {
            return new List<object>
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
                new {
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
                new {
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
            };
        }
    }
}
