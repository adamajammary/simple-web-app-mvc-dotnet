#!/bin/bash

ENV=Release
DOTNET=net9.0
OUT=/var/www/simple-web-app-mvc-dotnet
TARGET=linux-x64

cd SimpleWebAppMVC

dotnet clean
dotnet build -c ${ENV} -r ${TARGET} --no-self-contained
dotnet publish -c ${ENV} -r ${TARGET} --no-self-contained

rm -rf ${OUT}/*
cp -rf bin/${ENV}/${DOTNET}/${TARGET}/publish/* ${OUT}/

cd ..
