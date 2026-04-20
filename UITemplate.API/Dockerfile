# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7288


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copiar nuget.config con credenciales antes del restore
COPY nuget.config ./
# Copiar los archivos del proyecto
COPY ["UITemplate.API/UITemplate.API.csproj", "UITemplate.API/"]
COPY ["UITemplate.Application/UITemplate.Application.csproj", "UITemplate.Application/"]
COPY ["UITemplate.Domain/UITemplate.Domain.csproj", "UITemplate.Domain/"]
COPY ["UITemplate.Infrastructure/UITemplate.Infrastructure.csproj", "UITemplate.Infrastructure/"]
RUN dotnet restore "./UITemplate.API/UITemplate.API.csproj"
COPY . .
WORKDIR "/src/UITemplate.API"
RUN dotnet build "./UITemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UITemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app

# Ejecutar apk como root antes de cambiar el usuario
USER root
RUN apk add --no-cache icu-libs

# Volver al usuario de la aplicación si es necesario
USER $APP_UID

# Asegurar que ICU está presente en la imagen final
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0


COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UITemplate.API.dll"]