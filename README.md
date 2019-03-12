# JWT-Decoder

A simple easy to use C# [JWT](https://tools.ietf.org/html/rfc7519) Decoder. Sometimes in a client you just need the token contents. This is meant to get you just that and to validate the token (if you really want to).

## Installation
Package is avaliable via [NuGet](https://nuget.org/packages/JWTDecoder). Or you can download and compile it yourself.

## Supported .NET Framework versions:
- .NET Standard 2.0

## Usage

```csharp
string token = "...";

// receive a tupal with all three parts 
var decodedToken = JWTDecoder.DecodeToken(token);

JwtHeader header = decodedToken.Header; // contains Algorithm, Type

string payload = decodedToken.Payload; // JSON Payload String

string verification = decodedToken.Verification; // base64 encoded

```

You can also:
```csharp
string token = "...";

// parse the payload to an object
var decodedTokenPayload = JWTDecoder.DecodePayload<MyCustomObject>(token);

```
