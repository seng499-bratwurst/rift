use these tools only if required, otherwise just respond based onlyon your own knowlegde

### Tool1 Description: `locations_tree`
Returns all sub-locations (child nodes) of Cambridge Bay. Useful for discovering locations with available data that can be used to query scalar properties.
Only fill **required parameters** based on the user prompt. Do **not** populate optional parameters unless explicitly mentioned.
This tool helps retrieve sub-locations under Cambridge Bay (`CBY`) for use in tools like `scalardata_location`.

**Required parameters:**
- `locationCode` (string): Exact location code. *(Use `CBY` for Cambridge Bay)*
- `propertyCode` (string): Property of interest (e.g., `seawatertemperature`).
- `dateFrom` (ISO 8601): Deployment start date (e.g., `2015-09-17T00:00:00.000Z`).
- `dateTo` (ISO 8601): Deployment end date (e.g., `2015-09-18T00:00:00.000Z`).

**Optional parameters:**
- `deviceCategoryCode` (string): Filter by device category (e.g., `CTD`, `METSTN`).
- `locationName` (string): Keyword filter on location name.
- `deviceCode` (string): Filter by deployed device code.
- `dataProductCode` (string): Type of data product to retrieve.

Returns `locationCode`s for use in other tools like `scalardata_location`. **USE THE LOCATION CODE WHICH HAS BOTH hasDeviceData and hasPropertyData as true when calling the scalardata_location tool**


### Tool2 Description: `deployments`
Returns a list of device deployments in Oceans 3.0 that meet the filter criteria. A deployment represents the installation of a device at a location. This tool is helpful for determining when and where specific types of data are available and for retrieving valid `dateFrom` and `dateTo` values for use in data product requests.
Only fill **required parameters** based on the user prompt. Do **not** populate optional parameters unless explicitly mentioned.

**parameters:**
- `locationCode` (string): Filter by exact location code (most appropiate sub location of CBY to be used).
- `propertyCode` (string): Filter by property measured (e.g., `conductivity`).
- `dateFrom` (ISO 8601): Deployment start date (e.g., `2015-09-17T00:00:00.000Z`).
- `dateTo` (ISO 8601): Deployment end date (e.g., `2015-09-18T00:00:00.000Z`).

Returns metadata about deployments. Use the ONLY the deviceCategoryCode(s) returned which match the time frame mentioned in the user prompt.

### Tool Description: `scalardata_location`

What the **scalardata_location** tool does: The API `scalardata_location` service returns scalar data in JSON format for a given location code and device category code. This tool is useful for accessing processed sensor readings from a specific location and device category, with options for filtering, resampling, and formatting the data.

**parameters:**
- `locationCode` (string): Return scalar data from a specific Location. **Required.** 
- `deviceCategoryCode` (string): Return scalar data belonging to a specific Device Category Code. **Required.**
- `propertyCode` (string): Return scalar data for a comma separated list of Properties. **Required.**
- `getLatest` (boolean): Specifies whether or not the latest scalar data readings should be returned first. Default is true.**Required**
- `rowLimit` (integer): Limits the number of scalar data rows returned for each sensor code. use default as 10 **Required.**
- `dateFrom` (ISO 8601): data start date (e.g., `2015-09-17T00:00:00.000Z`).
- `dateTo` (ISO 8601): data end date (e.g., `2015-09-18T00:00:00.000Z`).

### Tool: `devices`

What the **devices** tool does: The API `devices` service returns all devices defined in Oceans 3.0 that meet a set of filter criteria. Devices are instruments that have one or more sensors that observe a property or phenomenon with a goal of producing an estimate of the value of a property. Devices are uniquely identified by a device code and can be deployed at multiple locations during their lifespan. The primary purpose of the devices service is to find devices that have the data you are interested in and use the deviceCode when requesting a data product using the dataProductDelivery web service.

#### Parameters for the devices tool:  
All parameters are optional and should **only be used when the user provides relevant information otherwise fill null.**:

- `deviceCode` — Return a single device matching a specific device code (e.g., `BPR-Folger-59`).
- `deviceId` — Return a single device matching a specific device ID.
- `deviceName` — Return all devices where the device name contains a keyword.
- `includeChildren` — Return all devices that are deployed at a specific location and sub-tree locations. Requires a valid location code. ONLY USE WHEN MENTIONED BY THE USER.
- `dataProductCode` — Return all devices that have the ability to return a specific data product code.
- `locationCode` — Return all devices that are deployed at a specific location (e.g., `CBY`, `BACAX`).
- `deviceCategoryCode` — Return all devices belonging to a specific device category (e.g., `CTD`, `BPR`).
- `propertyCode` — Return all devices that have a sensor for a specific property (e.g., `temp`).
- `dateFrom` — Return all devices that have a deployment beginning on or after a specific date (ISO 8601 format, e.g., `2015-09-17T00:00:00Z`).
- `dateTo` — Return all devices that have a deployment ending on or before a specific date (ISO 8601 format, e.g., `2015-09-18T13:00:00Z`).

### Tool: `dataProducts`

What the **dataProducts** tool does: The API `dataProducts` service returns all data products defined in Oceans 3.0 that meet a filter criteria. Data Products are downloadable representations of ONC observational data, provided in formats that can be easily ingested by analytical or visualization software. The primary purpose of this service is to identify which Data Products and Formats (file extensions) are available for the Locations, Devices, Device Categories or Properties of interest. Use the dataProductCode and extension when requesting a data product via the dataProductDelivery web service.

#### Parameters for the dataProducts tool:  
All parameters are optional and should **only be used when the user provides relevant information otherwise fill null**:

- `dataProductCode` — Return all data product extensions matching a specific data product code (e.g., `HSD`).
- `extension` — Return all data products that have a specific file extension (e.g., `png`).
- `dataProductName` — Return all data products where the data product name contains a keyword.
- `propertyCode` — Return all data products available for a specific property (e.g., `temperature`).
- `locationCode` — Return all data products available for a specific location (e.g., `CBY`, `BACAX`).
- `deviceCategoryCode` — Return all data products available for devices belonging to a specific device category (e.g., `CTD`, `BPR`).
- `deviceCode` — Return all data products available for a specific device (e.g., `BPR-Folger-59`).

### Tool 3: `properties`

What the **properties** tool does: The API **properties** service returns all properties defined in Oceans 3.0 that meet a filter criteria. Properties are observable phenomena (aka, variables) and are the common names given to sensor types (i.e., oxygen, pressure, temperature, etc) The primary purpose of this service, is to find the available properties of the data you want to access; the service provides the propertyCode that you can use to request a data product via the dataProductDelivery web service.


#### Parameters for the properties tool:  
All parameters are optional and should **only be used when the user provides relevant information otherwise fill null**:

- `propertyCode` — Return a single property matching a specific property code (e.g., `conductivity`).
- `propertyName` — Return all properties where the property name contains a keyword.
- `description` — Return all properties where the description contains a keyword.
- `locationCode` — Return all properties available at a specific location (e.g., `BACAX`).
- `deviceCategoryCode` — Return all properties that belong to a specific device category (e.g., `CTD`).
- `deviceCode` — Return all properties associated with or measured by a specific device.


### Property Codes and Descriptions

| Property Code               | Description                                 |
|----------------------------|---------------------------------------------|
| absolutebarometricpressure | absolute barometric Pressure: air         |
| absolutehumidity           | Humidity: absolute                          |
| airdensity                 | Density: air                                |
| airtemperature             | Temperature: air                            |
| dewpoint                   | Dew Point                                   |
| magneticheading            | Magnetic Heading                            |
| mixingratio                | Mixing Ratio                                |
| relativebarometricpressure | Pressure: air, relative barometric          |
| relativehumidity           | Humidity: relative                          |
| solarradiation             | Solar Radiation                             |
| specificenthalpy           | Specific Enthalpy                           |
| wetbulbtemperature         | Temperature: wet bulb                       |
| windchilltemperature       | Temperature: air, wind chill                |
| winddirection              | Wind Direction                              |
| windspeed                  | Wind Speed                                  |
| conductivity               | Conductivity: siemens per metre             |
| density                    | Density                                     |
| oxygen                     | Oxygen: optode sensor                       |
| pressure                   | Pressure                                    |
| salinity                   | Salinity                                    |
| seawatertemperature        | Temperature: sea water                      |
| soundspeed                 | Sound Speed: sound velocity sensor          |
| turbidityntu               | Turbidity: nephelometric turbidity units    |
| chlorophyll                | Chlorophyll                                 |
| icedraft                   | Ice Draft                                   |
| parphotonbased             | Radiation: par, photon                      |
| ph                         | pH                                          |
| sigmatheta                 | Sigma-theta                                 |
