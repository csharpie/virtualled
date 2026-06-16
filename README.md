# Virtual LED
A Blazor application that allows clients to change the color of the "lightbulb."

## Table of Contents
- Getting Started
- Anatomy
- Examples of how connect
  - ServiceNow

## Getting Started
1. Clone the GitHub repository.
```pwsh
git clone https://github.com/csharpie/VirtualLED.git

```
2. Generate an Encryption Key for the JWT Token.
3. Using User Secrets, add the Encryption Key to the secrets.json file.
```json
{
    "JWTSettings": {
        "Key": "ENCRYPTION_KEY_GOES_HERE"
    }
}
```
4. Run the solution.
```pwsh
dotnet run --project src/VirtualLED.csproj
```
5. Navigate to the [LED Color](http://localhost:5050/ledcolor) Razor Page.
6. Get a Token.
```http
HTTP POST /api/token
{
    "UserName": "USER_NAME_HERE",
    "Password": "PASSWORD_HERE
}
```
