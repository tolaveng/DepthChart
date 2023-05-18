# Project Structure
# Set Up
- SQL server
	- User: SqlUser
	- Password: access123
	- Server role: dbCreator
	Configuration can be updated in appsettings.Development.json
	
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
## Start Console client
```sh
> dotnet run --project Client/Client.csproj
```

### Assumptions
- a player can play for only one team
### Questions
- Who is a player in team A at positon LT?

### Add migration
> dotnet ef migrations add InitDataTables -c AppDbContext -s Server -p Infrastructure


