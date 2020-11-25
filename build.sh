#!/bin/bash

ENV=Release
OUT=/var/www/simple-web-app-mvc-dotnet
TARGET=ubuntu.18.04-x64

cd SimpleWebAppMVC

dotnet clean
dotnet build -c ${ENV} -r ${TARGET}
dotnet publish -c ${ENV} -r ${TARGET}

sudo rm -rf ${OUT}/*
sudo cp -rf bin/${ENV}/netcoreapp3.1/${TARGET}/publish/* ${OUT}/

sudo service apache2 restart
sudo service simplewebappmvcdotnet restart

cd ..
