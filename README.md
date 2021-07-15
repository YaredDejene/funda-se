# Funda Technical Assignment for SE Position
A small console application to show which makelaar's in Amsterdam have the most object listed for sale (and top 10 table) from a provided API.

## Technologies
- .NET Core 3.1
- Entity Framework Core 3.1
- Sqlite
- AutoMapper
- XUnit

## Steps to run the application
- cd to `src/Funda` folder
- Run migration `dotnet ef database update`
- Run application `dotnet run`

## Assumptions taken
- Query parameter `/amsterdam/tuin/` is used to filter out object listings with tuin.