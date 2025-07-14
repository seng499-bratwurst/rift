import requests
import json

url = 'https://data.oceannetworks.ca/api/devices'
parameters = {'method':'get',
			'token':'4cf2bbaa-3662-4dd3-8dce-2374a5e4a9fd',
			'locationCode':'CBY',
			'propertyCode':'seawatertemperature',
			'dateFrom':'2010-07-01T00:00:00.000Z',
			'dateTo':'2011-06-30T23:59:59.999Z'}

response = requests.get(url,params=parameters)

if (response.ok):
	devices = json.loads(str(response.content,'utf-8')) # convert the json response to an object
	for device in devices:
		print(device)
else:
	if(response.status_code == 400):
		error = json.loads(str(response.content,'utf-8'))
		print(error) # json response contains a list of errors, with an errorMessage and parameter
	else:
		print ('Error {} - {}'.format(response.status_code,response.reason))