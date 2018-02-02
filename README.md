# lightrail-client-dotnet
A .NET client for Lightrail

## Development

### Requirements
- [.NET Core SDK](https://dotnet.github.io/)
- [Visual Studio Code](https://code.visualstudio.com/) (or some other editor)

### Compiling
`dotnet build`

### Testing
Run unit tests with: `dotnet test`.

Create a test nuget package with `dotnet pack -c Debug`, add to local repo `nuget add Lightrail/bin/Debug/Lightrail-Client.<version>.nupkg -source ~/nuget-packages`, use in child project with `dotnet add package Lightrail-Client --version <version> -s ~/nuget-packages`.

### Releasing
- bump the PackageVersion appropriately in `Lightrail.csproj`
- create the nuget package with `dotnet pack -c Release`
- upload to nuget.org as per https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package
