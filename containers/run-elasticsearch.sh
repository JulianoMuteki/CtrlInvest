#!/bin/bash

# Trace bash
#set -x

# Default component 
DOCKER_COMPOSE_ELASTIC="docker-compose -f ./Elasticsearch/docker-compose.yaml"
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

function up_Component() {	
	eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_UP_BUILD)"	
}

function down_Component() {
	eval "$($DOCKER_COMPOSE_ELASTIC $DOCKER_COMMAND_DOWN)"
}

COMMAND=""


if [[ $# -eq 0 ]]; then
	usage
	exit 1
fi

while [ "$1" != "" ]; do
	case $1 in
		-up)
			up_Component
			exit 0
			;;			
		-d|--delete)
			down_Component
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