# Code Challenge
    This project demonstrate how to use .Net,
    - a Server is a REST Api, provides services end-points to manipulate data of players and depth chart.
    - a console application to consume the Web API, allows user to add/remove/display the depth chart.
    
# Run Project
	We can run the project in Docker or dotnet CLI
## Using Docker
- This project contains the Dockerfile and docker-compose.yml in the solution directory
- Go to the solution directory, and run command in PowerShell or Shell
```sh
 docker compose up --build
```
- Inspect the WebApi and MsSql containers are running
- Open a new PowerShell
```sh
 docker ps
```
  _See the WebApi and MsSql are running_

- For first time, it need to create a database. Execute this command
```sh
 docker exec mssql sh -c "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P access@123 -i /var/opt/Database-init.sql"
```
    ___If you cannot create the database, it can be done manualy using "Database-init.sql" in the solution directory, using SQL Management Tool or Sqlcmd.___

- Open the OpenApi Swagger to test the application
i.e:
	- http://localhost:5000/swagger/index.html
	- http://localhost:5000/api/getFullDepthChart

## Using dotnet CLI
- The Web Api required SQL Server to manage the database
- SQL server user name and password can be updated in the appsettings[.Development].json
  i.e. SQL server user
	- User: sa
	- Password: access@123
	- Server role: dbCreator or above
- First run, we need to create database "ChartDepth_Db", run command
```sh
 dotnet restore
 dotnet build
 dotnet ef database update -c AppDbContext -s Server -p Infrastructure
```
- Open the SQL server to verify if the database "ChartDepth_Db" is created
- Run the project
```sh
 dotnet run --project Server/Server.csproj
```
- Open the OpenApi Swagger to test the application
i.e: http://localhost:5000/swagger/index.html


# Expose Api End-points
- Get full depth chart
    - Get: http://localhost:5000/api/getFullDepthChart
	- Response text: List of depth chart
	
- Get backup depth chart
    - Get: http://localhost:5000/api/getBackups/[POSITION]/[PLAYER_NUMBER]
    ```sh
		https://localhost:5001/api/getBackups/OLB/1
	```
	- Response as text: List of backup players

- Add a player to depth chart
    - Post: http://localhost:5000/api/addPlayerToDepthChart
    - Body:
    ```sh
    {
        "position": "LT",
        "player": {
            "number": 2,
            "name": "Player 2"
        },
        "depth": 0
    }
    ```
		_depth is optional_
	- Response: 200 OK or 400 BadRequest
    
- Remove a player from depth chart by the player number
    - Delete: http://localhost:5000/api/removePlayerFromDepthChart/[POSTION]/[PLAYER_NUMBER]
    ```sh
    {
        https://localhost:5001/api/removePlayerFromDepthChart/LT/1
    }
    ```
	- Response: removed player

# Start Console client
- Run console application to test the process
```sh
 dotnet run --project Client/Client.csproj
```

# Class Diagram
![Class Diagram](Images/Class-Diagram.png)

# Troubleshoot
### Docker
- Try remove and run it again
```sh
 docker compose down --rmi local
 docker compose up
```
- Cannot connect to SQL server
    - Using Docker should expose a port 1431 (NOT 1433),
	- ie. localhost,1431
	- Username: sa and Password:access@123
		
- Cannot run the console client app, try
```sh
 dotnet restore
 dotnet build Client/Client.csproj
 dotnet run --project Client/Client.csproj
```
		
### dotnet CLI
- Make sure there is no error when issue command, > dotnet build
- Not data, check the SQL can be open at localhost port empty (1433 by default)
	
### App can run on different port
	- https://localhost:5001 (Not run in Docker)
	- http://localhost:5000
	
### Cannot create the database from migration or docker
	- In the solution directory, there is script "Database-init.sql" can be used to restore database.

# Assumptions
- all players are add to Offense, (make a default constant Group)
- a player can play for only one team
- a team belongs to one sport. A sport can have multiple teams.

# Questions
- Can we add more sport?
    yes, a sport has multiple team
- Can we add all team in NFL?
    yes, for now, we only hard-code the team 1 and Offence group

# Useful commands
- create add a new migration file
> dotnet ef migrations add InitDataTables -c AppDbContext -s Server -p Infrastructure

- generate Sql script from migration
> dotnet ef migrations script -s Server -p Infrastructure -o ./database-migration.sql
