# TodoApi
TodoListApi MVC Backend in ASP.Net Core

## For Developers
#### Adding certificate for HTTPS on localhost (try if https isn't working after running)
- Add Certificate `dotnet dev-certs https --trust`

#### Building and Running
- Download packages `dotnet restore`
- Build	`dotnet build`
- Run server for development `dotnet run dev --project TodoApi --launch-profile https`
- Run server for release `dotnet run --project TodoApi --launch-profile https`

#### Controller Scaffolding
- Get aspnet-codegenerator
```
dotnet tool uninstall -g dotnet-aspnet-codegenerator
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
```
- Scaffold a controller `dotnet aspnet-codegenerator --project TodoApi controller -name [CONTROLLER_NAME] -async -api -m [MODEL_CLASS_NAME] -dc [DB_CONTEXT_CLASS] -outDir Controllers`
- More information [here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-8.0#controller-options)

#### Run DB migration files:
- (ONLY FOR INITIAL CREATION) Create migration files from model classes `dotnet ef --project todoapi migrations add InitialCreate`
- Download Entity Framework tool `dotnet tool install --global dotnet-ef`
- Build before running migration `dotnet build`
- Run Migration files `dotnet ef --project TodoApi database update`

#### TEMPORARY !!!
Add CORS exception for localhost: https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0#cors-with-named-policy-and-middleware
C# REPL: https://github.com/waf/CSharpRepl 
