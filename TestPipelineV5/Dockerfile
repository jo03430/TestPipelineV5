#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestPipelineV5/TestPipelineV5.csproj", "TestPipelineV5/"]
COPY ["TestPipelineV5.Domain/TestPipelineV5.Domain.csproj", "TestPipelineV5.Domain/"]
RUN dotnet restore "TestPipelineV5/TestPipelineV5.csproj"
COPY . .
WORKDIR "/src/TestPipelineV5"
RUN dotnet build "TestPipelineV5.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestPipelineV5.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestPipelineV5.dll"]