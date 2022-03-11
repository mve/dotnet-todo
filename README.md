# Todo app.
Todo app with Blazor front-end, ASP.NET Core API and a MongoDB Database.

## Requirements
You must have MongoDB Running. <br>
The default database name that is used is `todo-dotnet` and the collection is called `todo`.
You can change this in the appsettings.json file in the api folder. (`api/appsettings.json`)

## Development
Start the ASP.NET Core API.
```
cd api
dotnet watch
```

Start the Blazor Front-end.
```
cd blazor
dotnet watch
```
