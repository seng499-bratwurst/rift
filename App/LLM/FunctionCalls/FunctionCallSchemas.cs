using System.Text.Json.Nodes;

namespace App.LLM
{
    public static class FunctionSchemas
    {
        public static JsonObject deviceCategories => new JsonObject
        {
            ["name"] = "deviceCategories",
            ["description"] = "The API deviceCategories service returns all device categories defined in Oceans 3.0 that meet a filter criteria. A Device Category represents an instrument type classification such as CTD (Conductivity, Temperature & Depth Instrument) or BPR (Bottom Pressure Recorder). Devices from a category can record data for one or more properties (variables). The primary purpose of this service, is to find device categories that have the data you want to access; the service provides the deviceCategoryCode you can use when requesting a data product via the dataProductDelivery web service.",
            ["parameters"] = new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject
                {
                    ["deviceCategoryCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return a single Device Category matching a specific Device Category Code"
                    },
                    ["deviceCategoryName"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all of the Device Categories where the Device Category Name contains a keyword"
                    },
                    ["locationCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "ONC location code (e.g., 'CBY' for Cambridge Bay)"
                    },
                    ["description"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all of the Device Categories where the Description contains a keyword."
                    },
                    ["propertyCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all Device Categories associated specific Property. Property codes can be found through the discovery service"
                    }
                },
            }
        };
    }
}
