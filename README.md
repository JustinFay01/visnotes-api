
# DevOps

## Linux Steps
[VPS Setup](https://github.com/dreamsofcode-io/zenstats/blob/main/docs/vps-setup.md)

**Create network**

```bash
docker network create --driver overlay ocr-net
```

1. init swarm

```bash
docker swarm init
```

2. Create postgres secret

```bash
printf <PASSWORD HERE> | docker secret create POSTGRES_PASSWORD -
```

3. Deploy
```bash
docker stack deploy -c swarm.yml ocr-api
```

4. Check
```bash
docker stack services ocr-api
```

***Cleanup***
```bash
 docker stack rm ocr-api
```
**Restart docker**
```bash
sudo systemctl restart docker
```

**View logs**
```bash
docker service logs --follow <service_name>
```

## Adding CI/CD Pipeline (With Docker and Github Actions)

### Prerequisites

1. A Dockerfile in the root of the repo
2. A docker-stack.yml file in the root of the repo
3. A docker hub account and a repository for the image
4. A secret in the repo for the docker hub username and a PAT for the docker hub password

### Create a User for Deploying

1. Switch to root and add a new User

```bash
sudo adduser deploy-<container_name/repo>
```

2. Add user to docker group

```bash
sudo usermod -aG docker deploy-<container_name/repo>
```

3. Switch to new user

```bash
su - deploy-<container_name/repo>
```

4. Create an SSH key (Recommend to generate the ssh key on your local machine and create the private key as a secret in the repo)

```bash
# On Remote Machine
mkdir ~/.ssh

  
# On Local Machine
ssh-keygen -t ed25519 -C "deploy-<container_name/repo>@<host>"

# Copy the public key clipboard
## windows
cat ~/.ssh/id_ed25519.pub | clip
## linux
cat ~/.ssh/id_ed25519.pub | xclip -selection clipboard

# On Remote Machine
echo `<public_key>` > ~/.ssh/authorized_keys
```
5. Restrict deploy user to only use the docker command

```bash
nano ~/.ssh/authorized_keys

# Prepend the key with the following
command="docker system dial-stdio"
```
**You should no longer be able to ssh via the deploy user**
*Check pipeline for how to use docker stack to deploy*

## Notes

1. Any **bind** mounts in the docker-compose file will need to be manually created on the host machine. 

# Misc

**IF RUNNING MIGARTIONS MANUALLY**
Need to change appsettings.json postgres -> localhost

1. ~~Create VLAN for Bucephalus~~
~~2. Purchase a domain and point to Bucephalus~~
~~3. Create Github Actions to deploy to Bucephalus using Docker Stack Deploy~~
~~4. Create a reverse proxy to route traffic to the correct service~~
~~5. Create a CI/CD pipeline to deploy to Bucephalus~~
6. Add Grafana and Prometheus to monitor the services

# TODO

# Front End
- [x] Add framer motion animated list to cloud upload
- [x] Be able to delete files from cloud
- [x] Upload multiple files at once

- [ ] Save options using Zustand
- [ ] Move notes to a separate page
- [ ] Create a mobile friendly Notes Section
- [ ] Add a warning when analyzing a note that has already been analyzed
- [ ] Add a button to generate a new word cloud instead of automatically generating it
- [ ] Add a button to download the word cloud as different file types
- [ ] Allow users 'correct' AI analysis of notes and save it as a preference
- [ ] Save users column widths as a preference
- [ ] Save selected columns as a preference
- [ ] Add a button to download the word cloud as an image
- [ ] Add different Layout options
- [ ] Update the Analysis view when selecting a note

- [ ] Fix weird null condtion checks for analysis
- [ ] Clean up how updating files int he table connects to the word cloud

# Back End

## CI/CD

- [ ] 

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



