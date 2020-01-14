#!/bin/bash 

NAME=kickerapp_db
PORT=1433
 
echo "> Stoppe Docker-Container ..."
docker stop $NAME

echo "> Entferne Docker-Container ..."
docker rm $NAME 

echo "> Erzeuge neuen Docker-Container ..."
docker run \
        -e ACCEPT_EULA=Y \
        --name $NAME \
        -e SA_PASSWORD=12!$NAME \
        -e MSSQL_PID=Express \
        -p $PORT:1433 \
        -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu\

echo "> Starte neuen Container ..."
docker start $NAME