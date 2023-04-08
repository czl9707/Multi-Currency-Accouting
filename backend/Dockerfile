FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /backend

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

RUN apt-get update &&\
    apt-get install -y \
        git \
        curl \
        iputils-ping && \
    apt-get clean

RUN dotnet dev-certs https --trust

WORKDIR /backend
# RUN ["dotnet", "restore"]
# COPY backend.csproj ./