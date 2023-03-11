#!/bin/bash

# Trace bash
set -x

ENV_FILE_VALUE=".env.pass.dev.ctrl-invest"

DOCKER_COMPOSE_API="docker-compose -f docker-compose-api-stock-exchange.yaml --env-file $ENV_FILE_VALUE"

DOCKER_COMMAND_UP=" up -d"
DOCKER_COMMAND_BUILD=" build"
DOCKER_COMMAND_UP_BUILD=" up -d --build"
DOCKER_COMMAND_DOWN=" down"


function up_Component() {	
	eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_UP_BUILD)"	
}

function down_Component() {
	eval "$($DOCKER_COMPOSE_API $DOCKER_COMMAND_DOWN)"
}

if [[ $1 != "" ]]; then	
	ENV_FILE_VALUE=$1
fi

up_Build_API
