#!/bin/bash

# Trace bash
#set -x


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

function deleteNetworks(){
	NETWORK_APPS="$(docker network ls | grep ctrl-apps-network | awk '{print $2}')"

	if [[ $NETWORK_APPS != "" ]]; then
		echo "deleting network ctrl-apps-network"				
		docker network rm ctrl-apps-network	
	fi

	NETWORK="$(docker network ls | grep ctrl-invest-network | awk '{print $2}')"

	if [[ $NETWORK != "" ]]; then
		echo "deleting network ctrl-invest-network"
		docker network rm ctrl-invest-network
	fi	
}

createNetworks