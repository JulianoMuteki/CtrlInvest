#!/bin/bash

# Trace bash
#set -x

# Default component 
DOCKER_COMPOSE_ELASTIC="docker-compose -f ./Elasticsearch/docker-compose.yaml"
DOCKER_COMPOSE_RabbitMQ="docker-compose -f ./RabbitMq/docker-compose.yaml --env-file ./RabbitMq/.env.rabmq"
DOCKER_COMPOSE_SELENIUM="docker-compose -f ./SeleniumHQ/docker-compose.yaml"
DOCKER_COMPOSE_API="docker-compose -f docker-compose-api-stock-exchange.yaml --env-file .env.pass.dev.ctrl-invest"
DOCKER_COMPOSE_HISTORY="docker-compose -f docker-compose-historical.yaml --env-file .env.pass.dev.ctrl-invest"
DOCKER_COMPOSE_RECEIVE="docker-compose -f docker-compose-receive-data.yaml --env-file .env.pass.dev.ctrl-invest"
DOCKER_COMMAND_UP=" up -d"
DOCKER_COMMAND_BUILD=" --build"
DOCKER_COMMAND_DOWN=" down"

function install_All_Ctrl_Invest() {
  echo "creating create -d bridge ctrl-apps-network"
  docker network create -d bridge ctrl-apps-network
  echo "creating -d bridge ctrl-invest-network"
  docker network create -d bridge ctrl-invest-network
  eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_UP)"
  eval "$($DOCKER_COMPOSE_RabbitMQ $DOCKER_COMMAND_UP)"
  eval "$($DOCKER_COMPOSE_SELENIUM $DOCKER_COMMAND_UP)"

  eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_UP $DOCKER_COMMAND_BUILD)"
  eval "$($DOCKER_COMPOSE_HISTORY $DOCKER_COMMAND_UP $DOCKER_COMMAND_BUILD)"
  eval "$($DOCKER_COMPOSE_RECEIVE $DOCKER_COMMAND_UP $DOCKER_COMMAND_BUILD)"
}

function delete_All_Ctrl_Invest() {
  eval "$($DOCKER_COMPOSE_HISTORY $DOCKER_COMMAND_DOWN)"
  eval "$($DOCKER_COMPOSE_RECEIVE $DOCKER_COMMAND_DOWN)"
  eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_DOWN)"
  eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_DOWN)"
  eval "$($DOCKER_COMPOSE_RabbitMQ $DOCKER_COMMAND_DOWN)"
  eval "$($DOCKER_COMPOSE_SELENIUM $DOCKER_COMMAND_DOWN)"
  docker network rm ctrl-apps-network
  docker network rm ctrl-invest-network
}

COMMAND=""


if [[ $# -eq 0 ]]; then
  usage
  exit 1
fi

while [ "$1" != "" ]; do
  case $1 in
    -i|--install-all)
      install_All_Ctrl_Invest
      exit 0
      ;;
    -d|--delete-all)
      delete_All_Ctrl_Invest
      exit 0
      ;;
    -a|--api)
        echo "Running DOCKER_COMPOSE_API"
        COMMAND="$DOCKER_COMPOSE_API"
        ;;
    -p|--history-price)
        echo "Running DOCKER_COMPOSE_HISTORY"
        COMMAND="$DOCKER_COMPOSE_HISTORY"
        ;;
    -r|--receive)
        echo "Running DOCKER_COMPOSE_RECEIVE"
        COMMAND="$DOCKER_COMPOSE_RECEIVE"
        ;;
    -u|--up)
        echo "Running up -d"
        COMMAND+="${DOCKER_COMMAND_UP}"
        ;;
    -b|--build)
        echo "Running --build"
        COMMAND+="$DOCKER_COMMAND_BUILD"
        ;;
    -h|--help)
        echo -e "-h|--help: Display available options\n"
        exit 0
        ;;
    *)
    echo -e "Invalid option, please check --help option."
    exit 1
    ;;
  esac
  shift
done

if [[ $COMMAND != "" ]]; then
  eval $COMMAND
  exit 0
fi