# Python 3 server example
from http.server import BaseHTTPRequestHandler, HTTPServer
import time
import json
import argparse
import sys

hostName = "10.0.2.16"
serverPort = 80
name = []

class MyServer(BaseHTTPRequestHandler):
    def do_GET(self):
        self.send_response(200)
        self.send_header("Content-type", "application/json")
        self.end_headers()
        #self.wfile.write(bytes("<html><head><title>https://pythonbasics.org</title></head>", "utf-8"))
        #self.wfile.write(bytes("<p>Request: %s</p>" % self.path, "utf-8"))
        #self.wfile.write(bytes("<body>", "utf-8"))
        #self.wfile.write(bytes("<p>This is an example web server.</p>", "utf-8"))
        #self.wfile.write(bytes("</body></html>", "utf-8"))
        #self.wfile.write(json.dumps("{"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":9.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}","utf-8"))
        self.wfile.write(bytes(name,"utf-8"));        
        #self.wfile.write(bytes(json.dumps({'location':{'name':'London','region':'City of London, Greater London','country':'United Kingdom','lat':51.52,'lon':-0.11,'tz_id':'Europe/London','localtime_epoch':1708376569,'localtime':'2024-02-19 21:02'},'current':{'last_updated_epoch':1708376400,'last_updated':'2024-02-19 21:00','temp_c':9.0,'temp_f':48.2,'is_day':0,'condition':{'text':'Clear','icon':'//cdn.weatherapi.com/weather/64x64/night/113.png','code':1000},'wind_mph':5.6,'wind_kph':9.0,'wind_degree':250,'wind_dir':'WSW','pressure_mb':1030.0,'pressure_in':30.42,'precip_mm':0.0,'precip_in':0.0,'humidity':71,'cloud':0,'feelslike_c':7.4,'feelslike_f':45.2,'vis_km':10.0,'vis_miles':6.0,'uv':1.0,'gust_mph':10.3,'gust_kph':16.5}}),"utf-8"))
        #self.wfile.write(json.dumps({"location":{"name":"London","region":"City of London, Greater London","country":"United Kingdom","lat":51.52,"lon":-0.11,"tz_id":"Europe/London","localtime_epoch":1708376569,"localtime":"2024-02-19 21:02"},"current":{"last_updated_epoch":1708376400,"last_updated":"2024-02-19 21:00","temp_c":9.0,"temp_f":48.2,"is_day":0,"condition":{"text":"Clear","icon":"//cdn.weatherapi.com/weather/64x64/night/113.png","code":1000},"wind_mph":5.6,"wind_kph":9.0,"wind_degree":250,"wind_dir":"WSW","pressure_mb":1030.0,"pressure_in":30.42,"precip_mm":0.0,"precip_in":0.0,"humidity":71,"cloud":0,"feelslike_c":7.4,"feelslike_f":45.2,"vis_km":10.0,"vis_miles":6.0,"uv":1.0,"gust_mph":10.3,"gust_kph":16.5}}))
	

if __name__ == "__main__":     
    name = sys.argv[1] 
    webServer = HTTPServer((hostName, serverPort), MyServer)
    print("Server started http://%s:%s" % (hostName, serverPort))

    try:
        webServer.serve_forever()
    except KeyboardInterrupt:
        pass

    webServer.server_close()
    print("Server stopped.")
