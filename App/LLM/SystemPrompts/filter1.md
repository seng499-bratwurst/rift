to get the scalar data we need to have both hasPropertyData and hasDeviceData

Properties which can be used to get the real time data based on location
- **location CBYIP**
    - "description": "Chlorophyll","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "chlorophyll","propertyName": "Chlorophyll","uom": "ug/l"
        - deviceCategoryCode: FLUOROMETER 
            - deviceCode: WETLABSECOFLRT6106
        - deviceCategoryCode: FLNTU 
            - deviceCode: WETLABSFLNTU2586, WETLABSFLNTU3441, WETLABSFLNTU3923
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Conductivity: siemens per metre","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "conductivity","propertyName": "Conductivity","uom": "S/m"
        - deviceCategoryCode: CTD  
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Density","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "density","propertyName": "Density","uom": "kg/m3"
        - deviceCategoryCode: CTD  
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Ice Draft","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "icedraft","propertyName": "Ice Draft","uom": "m"
        - deviceCategoryCode: ICEPROFILER  
            - deviceCode: ASLSWIP53019, ASLSWIP53029, ASLSWIP53038 

    - "description": "pH: internal or reference pH","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "internalph","propertyName": "Internal pH","uom": "pH"
        - deviceCategoryCode: PHSENSOR  
            - deviceCode: PHSENSOR246, PHSENSOR452 

    - "description": "Concentration: nitrate","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "nitrateconcentration","propertyName": "Nitrate Concentration","uom": "umol/l"
        - deviceCategoryCode: NITRATESENSOR 
            - deviceCode: ISUSX0116 

    - "description": "Oxygen: optode sensor","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "oxygen","propertyName": "Oxygen","uom": "ml/l"
        - deviceCategoryCode: OXYSENSOR  
            - deviceCode: SBE63630108, SBE63630225, SBE63630834, SBE63631008, SBE63631500 
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Radiation: par, photon","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "parphotonbased","propertyName": "PAR Photon-based","uom": "umol / m^2 s"
        - deviceCategoryCode: RADIOMETER  
            - deviceCode: WETLABSECOPARS440, WETLABSECOPARS459 (works with device code)
        

    - "description": "pH","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "ph","propertyName": "pH","uom": "pH"
        - deviceCategoryCode: PHSENSOR  (works)
            - deviceCode: PHSENSOR246, PHSENSOR452, SUNSAMIpHP0073

    - "description": "Pressure","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "pressure","propertyName": "Pressure","uom": "decibar"
        - deviceCategoryCode: ADCP1200KHZ  (works but returns no data)
            - deviceCode: RDIADCP1200WH9111
        - deviceCategoryCode: CTD   
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Salinity","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "salinity","propertyName": "Salinity","uom": "psu"
        - deviceCategoryCode: CTD   
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233
    - "description": "Temperature: sea water","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "seawatertemperature","propertyName": "Sea Water Temperature","uom": "C"
        - deviceCategoryCode: ICEPROFILER 
            - deviceCode: ASLSWIP53019, ASLSWIP53029, ASLSWIP53038
        - deviceCategoryCode: PHSENSOR (works)
            - deviceCode: PHSENSOR246, PHSENSOR452, SUNSAMIpHP0073
        - deviceCategoryCode: ADCP1200KHZ 
            - deviceCode: RDIADCP1200WH9111
        - deviceCategoryCode: OXYSENSOR 
            - deviceCode: SBE63630108, SBE63630225, SBE63630834, SBE63631008, SBE63631500
        - deviceCategoryCode: CTD 
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Sigma-t","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "sigmat","propertyName": "Sigma-t","uom": "kg/m3"
        - deviceCategoryCode: CTD 
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Sigma-theta","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "sigmatheta","propertyName": "Sigma-theta","uom": "kg/m3"
         - deviceCategoryCode: CTD 
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233
    - "description": "Sound Speed: sound velocity sensor","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "soundspeed","propertyName": "Sound Speed","uom": "m/s"
        - deviceCategoryCode: ADCP1200KHZ 
            - deviceCode: RDIADCP1200WH9111
        - deviceCategoryCode: CTD 
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Turbidity: nephelometric turbidity units","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "turbidityntu","propertyName": "Turbidity NTU","uom": "NTU"
        - deviceCategoryCode: FLNTU 
            - deviceCode: WETLABSFLNTU2586, WETLABSFLNTUS3441, WETLABSFLNTUS3923
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233
        - deviceCategoryCode: TURBIDITYMETER
            - deviceCode: WLNTUS319


- **location CBYIP.D4**
    - "description": "Conductivity: siemens per metre","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "conductivity","propertyName": "Conductivity","uom": "S/m"
         - deviceCategoryCode: CTD  (works but test again)
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Density","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "density","propertyName": "Density","uom": "kg/m3"
        - deviceCategoryCode: CTD  (works but test again)
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Oxygen: optode sensor","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "oxygen","propertyName": "Oxygen","uom": "ml/l"
        - deviceCategoryCode: OXYSENSOR  (works but test again)
                - deviceCode: SBE63630108, SBE63630225, SBE63630834, SBE63631008, SBE63631500 
            - deviceCategoryCode: WETLABS_WQM 
                - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Pressure","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "pressure","propertyName": "Pressure","uom": "decibar"
        - deviceCategoryCode: ADCP1200KHZ 
            - deviceCode: RDIADCP1200WH9111
        - deviceCategoryCode: CTD   (works but test again)
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Salinity","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "salinity","propertyName": "Salinity","uom": "psu"
         - deviceCategoryCode: CTD   (works but test again)
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233
            
    - "description": "Temperature: sea water","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "seawatertemperature","propertyName": "Sea Water Temperature","uom": "C"
        - deviceCategoryCode: CTD (works)
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769

    - "description": "Sound Speed: sound velocity sensor","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "soundspeed","propertyName": "Sound Speed","uom": "m/s"
         - deviceCategoryCode: ADCP1200KHZ 
            - deviceCode: RDIADCP1200WH9111
        - deviceCategoryCode: CTD 
            - deviceCode: SBECTD19p7036, SBECTD19p7128, SBECTD19p7518, SBECTD19p7589, SBECTD19p7769
        - deviceCategoryCode: WETLABS_WQM 
            - deviceCode: WETLABSWQM196, WETLABSWQM233

    - "description": "Turbidity: nephelometric turbidity units","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "turbidityntu","propertyName": "Turbidity NTU","uom": "NTU"
        - deviceCategoryCode: CTD  
            - deviceCode: AMLMETRECX50147




- **location CBYSS**
    - "description": "Pressure: air, absolute barometric","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "absolutebarometricpressure","propertyName": "Absolute Barometric Pressure","uom": "hPa"
        - deviceCategoryCode: BARPRESS  
            - deviceCode: VAISALAPTB210J3120006



- **location CBYSS.M2**
    - "description": "Pressure: air, absolute barometric","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "absolutebarometricpressure","propertyName": "Absolute Barometric Pressure","uom": "hPa"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Humidity: absolute","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "absolutehumidity","propertyName": "Absolute Humidity","uom": "g/m^3"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Density: air","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "airdensity","propertyName": "Air Density","uom": "kg/m3"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Temperature: air","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "airtemperature","propertyName": "Air Temperature","uom": "C"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Dew Point","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "dewpoint","propertyName": "Dew Point","uom": "C"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Magnetic Heading","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "magneticheading","propertyName": "Magnetic Heading","uom": "deg"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Mixing Ratio","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "mixingratio","propertyName": "Mixing Ratio","uom": "g/kg"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Pressure: air, relative barometric","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "relativebarometricpressure","propertyName": "Relative Barometric Pressure","uom": "hPa"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Humidity: relative","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "relativehumidity","propertyName": "Relative Humidity","uom": "%"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Solar Radiation","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "solarradiation","propertyName": "Solar Radiation","uom": "W/m^2"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Specific Enthalpy","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "specificenthalpy","propertyName": "Specific Enthalpy","uom": "kJ/kg"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Temperature: wet bulb","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "wetbulbtemperature","propertyName": "Wet Bulb Temperature","uom": "C"
         - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Temperature: air, wind chill","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "windchilltemperature","propertyName": "Wind Chill Temperature","uom": "C"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Wind Direction","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "winddirection","propertyName": "Wind Direction","uom": "deg"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031

    - "description": "Wind Speed","hasDeviceData": "true","hasPropertyData": "true","propertyCode": "windspeed","propertyName": "Wind Speed","uom": "m/s"
        - deviceCategoryCode: METSTN  
            - deviceCode: LUFFTWS501-07209121009031, LUFFTWS501-07209121009031


