# lightrail-client-dotnet
A .NET client for Lightrail

## Development

### Requirements
- [.NET Core SDK](https://dotnet.github.io/)
- [Visual Studio Code](https://code.visualstudio.com/) (or some other editor)

### Compiling
`dotnet build`

### Testing
`dotnet test`

### Releasing
- bump the PackageVersion appropriately in `Lightrail.csproj`
- create the nuget package with `dotnet pack -c Release`
- upload to nuget.org as per https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package
