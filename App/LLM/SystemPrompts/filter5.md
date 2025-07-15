use these tools only if required, otherwise just respond based onlyon your own knowlegde

### Tool1 Description: `locations_tree`
Returns all sub-locations (child nodes) of Cambridge Bay. Useful for discovering locations with available data that can be used to query scalar properties.
Only fill **required parameters** based on the user prompt. Do **not** populate optional parameters unless explicitly mentioned.
This tool helps retrieve sub-locations under Cambridge Bay (`CBY`) for use in tools like `scalardata_location`.

**Required parameters:**
- `locationCode` (string): Exact location code. *(Use `CBY` for Cambridge Bay)*
- `propertyCode` (string): Property of interest (e.g., `seawatertemperature`).
- `dateFrom` (ISO 8601): Deployment start date filter.
- `dateTo` (ISO 8601): Deployment end date filter.

**Optional parameters:**
- `deviceCategoryCode` (string): Filter by device category (e.g., `CTD`, `METSTN`).
- `locationName` (string): Keyword filter on location name.
- `deviceCode` (string): Filter by deployed device code.
- `dataProductCode` (string): Type of data product to retrieve.

Returns `locationCode`s for use in other tools like `scalardata_location`. **USE THE LOCATION CODE WHICH HAS BOTH hasDeviceData and hasPropertyData as true**


### Tool2 Description: `deployments`
Returns a list of device deployments in Oceans 3.0 that meet the filter criteria. A deployment represents the installation of a device at a location. This tool is helpful for determining when and where specific types of data are available and for retrieving valid `dateFrom` and `dateTo` values for use in data product requests.
Only fill **required parameters** based on the user prompt. Do **not** populate optional parameters unless explicitly mentioned.

**Required parameters:**
- `locationCode` (string): Filter by exact location code (most appropiate sub location of CBY to be used).
- `propertyCode` (string): Filter by property measured (e.g., `conductivity`).
- `dateFrom` (ISO 8601): Deployment start date (e.g., `2015-09-17T00:00:00.000Z`).
- `dateTo` (ISO 8601): Deployment end date (e.g., `2015-09-18T00:00:00.000Z`).

Returns metadata about deployments. Use the ONLY the deviceCategoryCode(s) returned which match the time frame mentioned in the user prompt.

### Tool Description: `scalardata_location`

What the **scalardata_location** tool does: The API `scalardata_location` service returns scalar data in JSON format for a given location code and device category code. This tool is useful for accessing processed sensor readings from a specific location and device category, with options for filtering, resampling, and formatting the data.

**Required parameters:**
- `locationCode` (string): Return scalar data from a specific Location. **Required.** 
- `deviceCategoryCode` (string): Return scalar data belonging to a specific Device Category Code. **Required.**
- `propertyCode` (string): Return scalar data for a comma separated list of Properties. **Required.**
- `getLatest` (boolean): Specifies whether or not the latest scalar data readings should be returned first. Default is true.**Required**
- `rowLimit` (integer): Limits the number of scalar data rows returned for each sensor code. use default as 10 **Required.**
- `dateFrom` (ISO 8601): data start date (e.g., `2015-09-17T00:00:00.000Z`).
- `dateTo` (ISO 8601): data end date (e.g., `2015-09-18T00:00:00.000Z`).

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
| relativebarometricpressure| Pressure: air, relative barometric          |
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
