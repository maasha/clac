# CLAC

A reverse-Polish notation calculator written in C#


## Tech Stack

- dotnet
- Avalonia UI
- xunit


## Boiler plate

Commands for setting up boiler plate project

```bash
brew install dotnet-sdk
dotnet --version
cd ~/src/clac
dotnet new install Avalonia.Templates
dotnet new classlib -n Clac.Core -o src/Clac.Core
dotnet new avalonia.app -n Clac.UI -o src/Clac.UI
dotnet new sln -n Clac
dotnet sln add src/Clac.Core/Clac.Core.csproj
dotnet sln add src/Clac.UI/Clac.UI.csproj
dotnet add src/Clac.UI/Clac.UI.csproj reference src/Clac.Core/Clac.Core.csproj
mkdir tests
cd tests
dotnet new xunit -n Clac.Tests
dotnet sln ../Clac.sln add Clac.Tests/Clac.Tests.csproj
dotnet add Clac.Tests/Clac.Tests.csproj reference ../src/Clac.Core/Clac.Core.csproj
cd ..
dotnet run --project src/Clac.UI
dotnet test
```


###

For Result<T> instead of throw:

```bash
dotnet add package DotNext
```
