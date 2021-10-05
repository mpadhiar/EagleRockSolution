# EagleRockSolution:
This is a .net web api implementation of a road traffic monitoring system

# Getting Started:
In order to run the solution the following tools are required.

1. **Visual Stuido 2019** which can be downloaded from here https://visualstudio.microsoft.com/downloads/
2. **Docker for Desktop** which can be downloaded from the link https://www.docker.com/products/docker-desktop

# EagelRockHub Api Solution:

The Api solution is a .NET 5.0 api project that can be started from visual studio and tested through the swagger file.
The api project requires api key authentication. The authentication api key is in appSettings.json file. The api project needs to access the redis cache. So
before starting the solution we need to run the following commands in terminal:

docker pull redis
