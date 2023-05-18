# Project Structure
# Set Up
- SQL server
	- User: SqlUser
	- Password: access123
	- Server role: dbCreator
	Configuration can be changed in appsettings.Development.json
	
- Create/update database
	- Go to solution directory
	- dotnet restore
	- dotnet build
	- dotnet ef database update -c AppDbContext -s Server -p Infrastructure
	
# Run Project
## Start Api server
```sh
> dotnet run --project Server/Server.csproj
```
- We can test the Api endpoints using OpenApi
https://localhost:5001/Swagger or http://localhost:5000/swagge

## Start Console client
```sh
> dotnet run --project Client/Client.csproj
```

### Assumptions
- all players are add to Offense, (make a default constant Group)
- a player can play for only one team
- No transaction and rollback have been implemented
- No race condition

### Questions
- Who is a player in team A at positon LT?

### Add migration
> dotnet ef migrations add InitDataTables -c AppDbContext -s Server -p Infrastructure


