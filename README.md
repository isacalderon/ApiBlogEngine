# StGenetics

Service in .NET to create and manage a blog.

### To build the services

first download the project

```
git clone https://github.com/isacalderon/ApiBlogEngine.git
dotnet build
```

### Run the service

```
dotnet run
```

### Json Collections

There is a json collection in the root of the project.

```
BlogEngine.postman_collection.json
```

# Database

I used a local database with docker

```
docker run -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=Test@1234" -e "MSSQL_PID=Developer" -e "MSSQL_USER=SA" -p 1433:1433 -d --name=sql mcr.microsoft.com/azure-sql-edge
docker ps
```

The querys are in _Scripts_ folder
