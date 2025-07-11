 # Monitoring system with prometheus and grafana on docker
 ## In this project we use this tools
 -  ASP.NET Core (Backend API)
 -  Prometheus (TSDB)
 -  Grafana 
 -  Alert Manager
 -  Node Exporter
 -  Google cAdvisor

## Give a Star! :star:

If you like this repo or found it helpful, please give it a star. Thanks!

## Quick start
### 1. [Up docker compose file](https://github.com/naeemaei/MonitoringExample/blob/master/Prometheus/docker-compose.yml)
Run this command from project root directory:
```
docker-compose -f Prometheus/docker-compose.yml up -d --build
```
If you are sure that this command is successfully executed, go to the next step

### 2. Check all services are running
 -  [ASP.NET Core Backend API](http://localhost:8008/swagger/index.html) => http://localhost:8008/swagger/index.html
 -  [Prometheus](http://localhost:9090/)  => http://localhost:9090/
 -  [Grafana](http://localhost:3000/) => http://localhost:3000/
    -  username: admin password: foobar
 -  [Alert Manager](http://localhost:9093/) => http://localhost:9093/
 -  [Node Exporter](http://localhost:9100/) => http://localhost:9100/
 -  [Google cAdvisor](http://localhost:8080/containers/) => http://localhost:8080/containers/

### Grafana Dashboards
For show all dashboards go to this [link](http://localhost:3000/dashboards)


