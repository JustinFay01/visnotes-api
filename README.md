
# DevOps

## Linux Steps

1. init swarm

```
docker swarm init --advertise-addr 192.168.1.121
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
Restart docker
```
sudo systemctl restart docker
```

**IF RUNNING MIGARTIONS MANUALLY**
Need to change appsettings.json postgres -> localhost

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


