#!/bin/bash 

NAME=kickerapp_db
PORT=3306
 
echo "> Stoppe Docker-Container ..."
docker stop $NAME

echo "> Entferne Docker-Container ..."
docker rm $NAME 

echo "> Erzeuge neuen Docker-Container ..."
docker run \
        -p $PORT:3306 \
        --name $NAME \
        -e MYSQL_ROOT_PASSWORD=root \
        -e MYSQL_DATABASE=$NAME \
        -e MYSQL_USER=$NAME \
        -e MYSQL_PASSWORD=$NAME \
        -d mariadb:10.4 \
        --character-set-server=utf8mb4 \
        --collation-server=utf8mb4_unicode_ci
echo "> Starte neuen Container ..."
docker start $NAME