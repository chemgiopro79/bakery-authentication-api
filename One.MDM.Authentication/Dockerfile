# Sử dụng image chính thức của .NET 8 SDK để build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ solution/project vào container
COPY . .

# Restore dependencies
RUN dotnet restore "One.MDM.Authentication.csproj"

# Build project ở chế độ Release
RUN dotnet publish "One.MDM.Authentication.csproj" -c Release -o /app/publish

# Sử dụng image runtime nhẹ để chạy app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (theo launch.json bạn đang dùng 5079 và 7208)
EXPOSE 5079
EXPOSE 7208

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "One.MDM.Authentication.dll"]
