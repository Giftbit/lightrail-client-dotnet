# Lightrail Client for .NET
Lightrail is a modern platform for digital account credits, gift cards, promotions, and points (to learn more, visit [Lightrail](https://www.lightrail.com/)). This is a basic library for developers to easily connect with the Lightrail API using the .NET platform. If you are looking for specific use cases or other languages, check out the complete list of all [Lightrail libraries and integrations](https://github.com/Giftbit/Lightrail-API-Docs/blob/master/README.md#lightrail-integrations).

## Development

### Requirements
- [.NET Core SDK](https://dotnet.github.io/)
- [Visual Studio Code](https://code.visualstudio.com/) (or some other editor)

### Compiling
`dotnet build`

### Unit Testing
`dotnet test`

### Building Test Packages
- (once) install [nuget](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#cli-tools)
- (once) create a local dir to store packages: `mkdir ~/nuget-packages`
- bump the version number in `Lightrail/Lightrail.csproj`
- create a test nuget package: `dotnet pack -c Debug`
- add the package to a local dir `nuget add Lightrail/bin/Debug/Lightrail-Client.<version>.nupkg -source ~/nuget-packages`
- (optional) if replacing a package with the same version number clear the local cache `nuget locals all -clear`
- use the package in the child project `dotnet add package Lightrail-Client --version <version> -s ~/nuget-packages`

### Releasing
- bump the PackageVersion appropriately in `Lightrail/Lightrail.csproj`
- run the unit tests
- create the nuget package with `dotnet pack -c Release`
- generate and copy a [nuget api key](https://www.nuget.org/account/ApiKeys)
    - expiry `1 day`
    - `Push new packages and package versions`
    - Glob Pattern `*`
- publish to nuget `dotnet nuget push Lightrail/bin/Release/Lightrail-Client.<version>.nupkg -s https://api.nuget.org/v3/index.json -k <apikey>`
- delete the [nuget api key](https://www.nuget.org/account/ApiKeys)
