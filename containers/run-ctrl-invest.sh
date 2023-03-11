#!/bin/bash

# Trace bash
set -x

ENV_FILE_VALUE=".env.pass.dev.ctrl-invest"

# Default component 
DOCKER_COMPOSE_ELASTIC="docker-compose -f ./Elasticsearch/docker-compose.yaml"
DOCKER_COMPOSE_RabbitMQ="docker-compose -f ./RabbitMq/docker-compose.yaml --env-file ./RabbitMq/.env.rabmq"
DOCKER_COMPOSE_SELENIUM="docker-compose -f ./SeleniumHQ/docker-compose.yaml"

DOCKER_COMPOSE_API="docker-compose -f docker-compose-api-stock-exchange.yaml --env-file $ENV_FILE_VALUE"
DOCKER_COMPOSE_HISTORY="docker-compose -f docker-compose-historical.yaml --env-file $ENV_FILE_VALUE"
DOCKER_COMPOSE_RECEIVE="docker-compose -f docker-compose-receive-data.yaml --env-file $ENV_FILE_VALUE"
DOCKER_COMPOSE_EARNING="docker-compose -f docker-compose-earning.yaml --env-file $ENV_FILE_VALUE"
DOCKER_COMMAND_UP=" up -d"
DOCKER_COMMAND_BUILD=" build"
DOCKER_COMMAND_UP_BUILD=" up -d --build"
DOCKER_COMMAND_DOWN=" down"

function createNetworks(){
	NETWORK_APPS="$(docker network ls | grep ctrl-apps-network | awk '{print $2}')"

	if [[ $NETWORK_APPS == "" ]]; then
		echo "creating create -d bridge ctrl-apps-network"
		docker network create -d bridge ctrl-apps-network
	fi

	NETWORK="$(docker network ls | grep ctrl-invest-network | awk '{print $2}')"

	if [[ $NETWORK == "" ]]; then
		echo "creating -d bridge ctrl-invest-network"
		docker network create -d bridge ctrl-invest-network
	fi	
}

function up_All_Ctrl_Invest() {	
	eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_RabbitMQ $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_SELENIUM $DOCKER_COMMAND_UP)"

	eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_HISTORY $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_RECEIVE $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_EARNING $DOCKER_COMMAND_UP)"	
}

function install_All_Ctrl_Invest() {
	createNetworks
	eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_RabbitMQ $DOCKER_COMMAND_UP)"
	eval "$($DOCKER_COMPOSE_SELENIUM $DOCKER_COMMAND_UP)"

	eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_UP_BUILD)"
	eval "$($DOCKER_COMPOSE_HISTORY $DOCKER_COMMAND_UP_BUILD)"
	eval "$($DOCKER_COMPOSE_RECEIVE $DOCKER_COMMAND_UP_BUILD)"
	eval "$($DOCKER_COMPOSE_EARNING $DOCKER_COMMAND_UP_BUILD)"	
}

function delete_All_Ctrl_Invest() {
	eval "$($DOCKER_COMPOSE_HISTORY $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_RECEIVE $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_RabbitMQ $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_SELENIUM $DOCKER_COMMAND_DOWN)"
	eval "$($DOCKER_COMPOSE_EARNING $DOCKER_COMMAND_DOWN)"

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
		-up-all)
			up_All_Ctrl_Invest
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
			echo "Running Import history price"
			COMMAND="$DOCKER_COMPOSE_HISTORY"
			;;
		-r|--receive)
			echo "Running Receive data"
			COMMAND="$DOCKER_COMPOSE_RECEIVE"
			;;
		-e|--earning)
			echo "Running Earning"
			COMMAND="$DOCKER_COMPOSE_EARNING"
			;;			
		-u|--up)
			echo "Running up -d"
			COMMAND+="${DOCKER_COMMAND_UP}"
			;;
		-b|--build)
			echo "Running --build"
			COMMAND+="$DOCKER_COMMAND_BUILD"
			;;
		-env-file)
			echo "$2"
			exit 0
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
	createNetworks
	eval $COMMAND
	exit 0
fi