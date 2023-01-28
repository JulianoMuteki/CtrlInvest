# docker clean

#docker image prune
#docker container prune

## Execute Network Fixed Common
docker network create -d bridge ctrl-apps-network

## Execute Jenkins
## Execute PgAmind
## Execute RabbitMq
## Execute Selenium
## Execute Elastic Search + Kibana

docker-compose -f ./Elasticsearch/docker-compose.yaml up -d

docker-compose -f ./RabbitMq/docker-compose.yaml --env-file ./RabbitMq/.env.rabmq up -d

docker-compose -f ./SeleniumHQ/docker-compose.yaml up -d


docker-compose -f docker-compose-api-stock-exchange.yaml --env-file .env.pass.dev.ctrl-invest -p ctrl-invest up -d --build
docker-compose -f docker-compose-historical.yaml --env-file .env.pass.dev.ctrl-invest -p ctrl-invest up -d --build
docker-compose -f docker-compose-receive-data.yaml --env-file .env.pass.dev.ctrl-invest -p ctrl-invest up -d --build
