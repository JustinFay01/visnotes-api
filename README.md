
# DevOps

## Linux Steps

**Create network**

```
docker network create --driver overlay ocr-net
```

1. init swarm

```
docker swarm init
```

2. Create postgres secret

```
printf <PASSWORD HERE> | docker secret create POSTGRES_PASSWORD -
```

3. Deploy
```
docker stack deploy -c swarm.yml ocr-api
```

4. Check
```
docker stack services ocr-api
```

***Cleanup***
```
 docker stack rm ocr-api
```
**Restart docker**
```
sudo systemctl restart docker
```

**View logs**
```
docker service logs --follow <service_name>
```


**IF RUNNING MIGARTIONS MANUALLY**
Need to change appsettings.json postgres -> localhost

1. Create VLAN for Bucephalus
2. Purchase a domain and point to Bucephalus
3. Create Github Actions to deploy to Bucephalus using Docker Stack Deploy
4. Create a reverse proxy to route traffic to the correct service
5. Create a CI/CD pipeline to deploy to Bucephalus
6. Add Grafana and Prometheus to monitor the services

# TODO

# Front End
- [x] Add framer motion animated list to cloud upload
- [x] Be able to delete files from cloud
- [x] Upload multiple files at once

- [ ] Save options using Zustand
- [ ] Create a mobile friendly Notes Section
- [ ] Add a warning when analyzing a note that has already been analyzed
- [ ] Add a button to generate a new word cloud instead of automatically generating it
- [ ] Save users column widths as a preference
- [ ] Save selected columns as a preference
- [ ] Add a button to download the word cloud as an image
- [ ] Add different Layout options
- [ ] Update the Analysis view when selecting a note

- [ ] Fix weird null condtion checks for analysis
- [ ] Clean up how updating files int he table connects to the word cloud

# Back End

## DB Architecture
**Minimum Viable Product Schema**

- [x] Note
    - id
    - name
    - size
    - type
    - url (or path)
    - created_at

- [x] Analysis
    - id
    - note_id
    - created_at
    - raw_value



