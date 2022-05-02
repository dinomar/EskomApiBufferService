# Eskom Buffer Api

#### A Dockerized buffer api for replacing Eskom's load shedding api, build on Asp.Net Core 3.1

Unfortunately Eskom's load shedding api is very unpredictable, and not always available when it is under significant load. This project aim's to remedy this problem by placing a buffer between Eskom api service and the applications using the api.

This can be used as a drop in replacement for Eskom's api by simply pointing a application to this api service instead of the Eskom api. It also has some extra
functionality and can be run from a Docker container.

### Download and run:

Download the latest release [here](https://github.com/dinomar/EskomApiBufferService/releases/tag/1.0.1).

`docker build -t eskombuffer . `

`docker run -d -p 8080:80 --restart=always --name eskom eskombuffer`

`http://localhost:8080/api/eskom/getstatus`

### Configuration:

Edit `appsettings.json` to configure the service.

DelayInMinutes - Time between getting a new status update from Eskom's loadsheadding api.  
Retries - Number of times this service should try the get a new status update from the Eskom's loadsheadding api.  
StatusMinRange - Used for validating a acceptable response from Eskom's loadsheadding api.  
StatusMaxRange - Used for validating a acceptable response from Eskom's loadsheadding api.  
MaxLogs - Maximum number of statuses to keep cached. The clean up run when this ammount is exceeded.  

```

"EskomBufferService": {
  "DelayInMinutes": 60,
  "Retries": 3, 
  "StatusMinRange": 0,
  "StatusMaxRange": 10,
  "MaxLogs": 1000
}
```

### Endpoints:

#### [GET] api/eskom/GetStatus  
```
2  
```

#### [GET] api/eskom/GetStatusLog  
```
{"level":1,"updated":"2022-05-01T00:44:22.9797845+02:00"}  
```

#### [GET] api/eskom/GetAllStatusLogs  
```
[{"level":1,"updated":"2022-05-01T00:54:23.2562919+02:00"},{"level":1,"updated":"2022-05-01T00:49:23.1071896+02:00"},{"level":1,"updated":"2022-05-01T00:44:22.9797845+02:00"}]  
```

#### [GET] api/eskom/GetLastUpdateTime  
```
{"updated":"2022-05-01T00:44:22.9797845+02:00"}  
```
