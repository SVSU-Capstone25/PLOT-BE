dotnet sln add ./PlotService/Plot.csproj
dotnet new xunit -o PlotService.Tests
dotnet add ./PlotService.Tests/PlotService.Tests.csproj reference ./PlotService/Plot.csproj
dotnet sln add ./PlotService.Tests/PlotService.Tests.csproj
