using System.Text.Json.Nodes;

namespace App.LLM
{
    public static class FunctionSchemas
    {
        // /deviceCategories endpoint schema
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

        // /deployments endpoint function schema
        public static JsonObject deployments => new JsonObject
        {
            ["name"] = "deployments",
            ["description"] = "The deployments discovery web service returns all deployments defined in Oceans 3.0 which meet the filter criteria, where a deployment is the installation of a device at a location. The deployments service assists in knowing when and where specific types of data are available. The primary purpose for the deployments service is to find the dates and locations of deployments and use the dateFrom and dateTo datetimes when requesting a data product using the dataProductDelivery web service.",
            ["parameters"] = new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject
                {
                    ["deviceCategoryCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all Deployments that have devices belonging to a specific Device Category. DeviceCategoryCodes can be found through the deviceCategory discovery service."
                    },
                    ["locationCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all Deployments at a specific Location Code. Location Code must be valid. Specific Location Codes can be found by simply running the service without this parameter to get a list of all locations."
                    },
                    ["deviceCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all Deployments of a specific Device."
                    },
                    ["propertyCode"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["description"] = "Return all Deployments that have devices with a sensor for a specific Property. Specific Property Codes can be obtained using the properties service."
                    },
                    ["dateFrom"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["format"] = "date-time",
                        ["description"] = "Return all deployments that have a deployment ending on or after a specific date/time. Acceptable dates/times conform to the ISO 8601 standard"
                    },
                    ["dateTo"] = new JsonObject
                    {
                        ["type"] = "string",
                        ["format"] = "date-time",
                        ["description"] = "Return all deployments that have a deployment ending on or after a specific date/time. Acceptable dates/times conform to the ISO 8601 standard"
                    }
                },
            }
        };
    }
}
