# Setup

- Install SQL Express on your machine, or modify the connection in appsettings.json
- Add an ASP.NET Core secret containing the JWT issuer key to your environment with this command :

```
dotnet user-secrets set "JsonWebTokenKeys:SymmetricKey" "YOUR_KEY"
```