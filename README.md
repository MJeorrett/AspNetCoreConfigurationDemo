# Asp.Net Core Configuration
A simple web server with endpoints to explore and explain configuration in Asp.Net Core.

## Intro
This project comprises:
- some simple configuration endpoints that allow you see the active configuration in the project.
- Some configuration and provider code (see `program.cs`) that you can play around with to see the effect on the results from the endpoints.

## Running
This project is F5-able in Visual Studio.
You should also be able to run it with `dotnet run` from the command line.

When the project launches navigate to https://localhost:5001 (if it doesn't automatically open) and
you will get a Swagger UI which you can use to call the endpoints.

## Key Vault
Config is loaded from Key Vault if the `KeyVaultName` config is found in the rest of the configuration.
The project is set up to authenticate to Key Vault as the user signed in to Visual Studio using MSI.
If you want to add configuration from a keyvault:
- Add `KeyVaultName` environment variable containing the name of your key vault.
- Ensure that you have `get` and `list` secret permissions in the access policy for the key vault.