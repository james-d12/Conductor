#!/usr/bin/sh

cd src/Conductor.Persistence
dotnet ef migrations add $1