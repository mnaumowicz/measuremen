*** Settings ***
Suite Setup                   Setup
Suite Teardown                Teardown
Test Setup                    Reset Emulation
Test Teardown                 Test Teardown
Resource                      ${RENODEKEYWORDS}

*** Variables ***
${UART}                       sysbus.uart0
${URI}                        @https://dl.antmicro.com/projects/renode

${BMP280}=     SEPARATOR=
...  """                                         ${\n}
...  using "platforms/boards/arduino_nano_33_ble.repl"        ${\n}
...                                              ${\n}
...  BMP280: Sensors.BMP280 @ twi0 0x77          ${\n}
...  """

*** Keywords ***
Create Machine
	Execute Command          mach create
	Execute Command          machine LoadPlatformDescriptionFromString ${BMP280}
	Execute Command          sysbus LoadELF @/home/debian/zephyrproject/zephyr/build/zephyr/zephyr.elf

*** Test Cases ***
Should Read Temperature
	Create Machine
	Create Terminal Tester    ${UART}
	Execute Command		  ${UART} CreateFileBackend "/home/debian/${TEST NAME}.txt"
	Set Test Variable         ${WAIT_PERIOD}             2
	Execute Command           sysbus.twi0.BMP280 Temperature 30.0
	Start Emulation
	Wait For Line On Uart     no;date;device id;data;status
	FOR    ${item}    IN    30    31    32
		Execute Command           sysbus.twi0.BMP280 Temperature ${item}
		#Wait For Next Line On Uart	\\d+;\\s+;\\d+;${item};\\s+	treatAsRegex=true
		${ts}=  Wait For Line On Uart	${item}
		Should Contain             ${ts.line}     normal
		#Wait For Line On Uart    Temperature: ${item} degC
		#Test If Uart Is Idle      ${WAIT_PERIOD}

	END

Should Read Too High Temperature
	Create Machine
	Create Terminal Tester    ${UART}
	Execute Command		  ${UART} CreateFileBackend "/home/debian/${TEST NAME}.txt"
	Set Test Variable         ${WAIT_PERIOD}             2
	Execute Command           sysbus.twi0.BMP280 Temperature 36.0
	Start Emulation
	Wait For Line On Uart     no;date;device id;data;status
	Wait For Next Line On Uart
	FOR    ${item}    IN    36    37    38
		Execute Command           sysbus.twi0.BMP280 Temperature ${item}
		${ts}=  Wait For Line On Uart	${item}
		Should Contain             ${ts.line}    too high
		#Wait For Line On Uart    Temperature: ${item} degC
		#Test If Uart Is Idle      ${WAIT_PERIOD}

	END

Should Read Too Low Temperature
	Create Machine
	Create Terminal Tester    ${UART}
	Execute Command		  ${UART} CreateFileBackend "/home/debian/${TEST NAME}.txt"
	Set Test Variable         ${WAIT_PERIOD}             2
	Execute Command           sysbus.twi0.BMP280 Temperature 20.0
	Start Emulation
	Wait For Line On Uart     no;date;device id;data;status
	Wait For Next Line On Uart
	FOR    ${item}    IN    20   21    22
		Execute Command           sysbus.twi0.BMP280 Temperature ${item}
		${ts}=  Wait For Line On Uart	${item}
		Should Contain             ${ts.line}      too low
		#Wait For Line On Uart    Temperature: ${item} degC
		#Test If Uart Is Idle      ${WAIT_PERIOD}

	END

Should Read Final Temperature
	Create Machine
	Create Terminal Tester    ${UART}
	Execute Command		  ${UART} CreateFileBackend "/home/debian/${TEST NAME}.txt"
	Set Test Variable         ${WAIT_PERIOD}             2
	Execute Command           sysbus.twi0.BMP280 Temperature 25.0
	Start Emulation
	Wait For Line On Uart     no;date;device id;data;status
	${my_list}	Create List		25    35    35	25	25    35    35	 25	45	45	25
	FOR    ${item}    IN    @{my_list}	
		Execute Command           sysbus.twi0.BMP280 Temperature ${item}
		${ts}=  Wait For Line On Uart	${item}
		${item}=	Convert To Number	${item}
		IF	${item} < ${25}
			Should Contain             ${ts.line}      too low
		ELSE IF	${item} > ${35}
			Should Contain             ${ts.line}      too high
		ELSE
			Should Contain             ${ts.line}      normal
		END
		#Wait For Line On Uart    Temperature: ${item} degC
		#Test If Uart Is Idle      ${WAIT_PERIOD}

	END




