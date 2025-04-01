#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AppointmentService/Med.Appointment.WebAPI/Med.Appointment.WebAPI.csproj", "src/AppointmentService/Med.Appointment.WebAPI/"]
RUN dotnet restore "./src/AppointmentService/Med.Appointment.WebAPI/Med.Appointment.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/AppointmentService/Med.Appointment.WebAPI"
RUN dotnet build "./Med.Appointment.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Med.Appointment.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Med.Appointment.WebAPI.dll"]