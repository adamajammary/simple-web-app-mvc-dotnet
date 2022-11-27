#!/bin/bash

ENV=Release
DOTNET=net7.0
OUT=/var/www/simple-web-app-mvc-dotnet
TARGET=linux-x64

cd SimpleWebAppMVC

dotnet clean
dotnet build -c ${ENV} -r ${TARGET} --no-self-contained
dotnet publish -c ${ENV} -r ${TARGET} --no-self-contained

sudo rm -rf ${OUT}/*
sudo cp -rf bin/${ENV}/${DOTNET}/${TARGET}/publish/* ${OUT}/

sudo service apache2 restart
sudo service simplewebappmvcdotnet restart

cd ..
