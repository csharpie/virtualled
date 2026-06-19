# Virtual LED
A Blazor application that allows clients to change the color of the "lightbulb."

## Table of Contents
- [Virtual LED](#virtual-led)
  - [Table of Contents](#table-of-contents)
  - [Getting Started](#getting-started)
  - [Credits](#credits)

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
dotnet run --project src/VirtualLED/VirtualLED.csproj
```
5. Navigate to the [LED Color](http://localhost:5050/ledcolor) Razor Page.
6. Get a Token.
```http
HTTP POST /api/token
{
    "UserName": "USER_NAME_HERE",
    "Password": "PASSWORD_HERE"
}
```
## Credits
- Serilog implementation, API Error Code Handling and JWT implementation
  - Trevoir William's course [The Definitive ASP.NET Core Web API Guide](https://learning.oreilly.com/videos/-/9781807606855/), on O'Reilly
    - [GitHub Repository](https://github.com/trevoirwilliams/HotelListing.Api/)
- Serilog implementation
  - Steve Smith a.k.a Ardalis' [RiverBooks GitHub Repository](https://github.com/ardalis/RiverBooks)