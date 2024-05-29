#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
ARG TARGETARCH
WORKDIR /app

COPY . ./
# Restore as distinct layers
RUN dotnet restore -a $TARGETARCH
# Build and publish a release
RUN dotnet publish -c Release -a $TARGETARCH -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine
WORKDIR /ddns
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "aliyun-ddns.dll"]