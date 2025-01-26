*** Settings ***
Test Teardown    Terminate All Processes    kill=True

*** Variables ***
${UART}                             sysbus.uart0
${PROMPT}                           \#${SPACE}

*** Keywords ***
Create Machine
    Execute Command                 include @scripts/single-node/measurement.resc
    Create Terminal Tester          ${UART}

Boot Linux And Login
    Wait For Prompt On Uart         buildroot login:  timeout=50
    Write Line To Uart              root
    Wait For Prompt On Uart         ${PROMPT}

Execute Linux Command
    [Arguments]                     ${command}  ${timeout}=5
    Write Line To Uart              ${command}
    Wait For Prompt On Uart         ${PROMPT}  timeout=${timeout}
    Check Exit Code

*** Test Cases ***

Should Boot And Login
    Create Machine
    Start Emulation

    Boot Linux And Login

Check Temps_35_-5
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":-5.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature 35
    Start Emulation

    Boot Linux And Login
    Write Line To Uart          weather_station
    Wait For Line On Uart       i_temp:35 o_temp:-5
    Assert LED State            false

Check Temps_35_-4
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":-4.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature 35
    Start Emulation

    Boot Linux And Login
    Write Line To Uart          weather_station
    Wait For Line On Uart       i_temp:35 o_temp:-4
    Assert LED State            false

Check Temps_35_9
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":9.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature 35
    Start Emulation

Check Temps_-35_-5
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":-5.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature -35
    Start Emulation

    Boot Linux And Login
    Write Line To Uart          weather_station
    Wait For Line On Uart       i_temp:-35 o_temp:-5
    Assert LED State            true

Check Temps_-35_-4
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":-4.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature -35
    Start Emulation

    Boot Linux And Login
    Write Line To Uart          weather_station
    Wait For Line On Uart       i_temp:-35 o_temp:-4
    Assert LED State            true

Check Temps_-35_9
    Create Machine
    ${handle}=	start process	python3  /home/debian/server/server.py	{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":9.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}
    Create LED Tester           sysbus.gpio.led0  defaultTimeout=2
    Execute Command             env Temperature -35
    Start Emulation

    Boot Linux And Login
    Write Line To Uart          weather_station
    Wait For Line On Uart       i_temp:-35 o_temp:9
    Assert LED State            false
