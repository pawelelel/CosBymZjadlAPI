FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY ./CosBymZjadlAPI/bin/Debug/net8.0 ./

ENTRYPOINT ["dotnet", "CosBymZjadlAPI.dll"]