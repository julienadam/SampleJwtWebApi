# Setup

## Database setup

- Install SQL Express on your machine
- Check the database connection string in `appsettings.json` and modify as needed
- Make sure you have the EF Core tools installed with this command (replace `install` with `update` if needed): 

```
dotnet tool install --global dotnet-ef
```

- Open a developper command prompt in the `SampleJwtApp` folder
- Deploy the database and schema (it's the default ASP.NET Core Identity schema) with this command :

```
dotnet ef database update
```

## Secrets setup

- Add an ASP.NET Core secret containing the JWT issuer key to your environment with this command :

```
dotnet user-secrets set "JsonWebTokenKeys:SymmetricKey" "YOUR_KEY"
```

---

# Run the app

- Use the SwaggerUI launch configuration
- Run the Security / Register API to create a user
- Run the Security / Login API to create a token using the login / password you just created
- The response will contain a JWT token, copy it
- Open any Products API method, click the *lock* icon on the top right corner and enter "Bearer" followed by a space, then paste the token
- Run the API method, you should get a valid `200 OK` response. If you get a `401 Unauthorized`, something went wrong in the previous steps