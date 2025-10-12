# Conductor.Engine

This folder contains the Conductor.Engine project. This is a C# backend that has the all the Domain Rules, Persistence
and API handling. It is the heart of Conductor and is what is called by all frontends (like Conductor.Web).

## Getting Started

You need to have [Docker](https://www.docker.com/)
and [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed.

1. You will need to clone the repository from the master branch.
2. Next you will need to open the Conductor.Engine in an IDE (E.G. JetBrains Rider, Visual Studio, or Visual Studio
   Code)
3. Now, run the ```setup.sh``` script inside the ```./scripts``` folder. This will install the required global dotnet
   tools like efcore.
4. Next you can run the docker compose to spin up the API / CLI application.