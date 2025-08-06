FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos de projetos (API e libs)
COPY Biblioteca.API/Biblioteca.API.csproj Biblioteca.API/
COPY Biblioteca.Data/Biblioteca.Data.csproj Biblioteca.Data/
COPY Biblioteca.Domain/Biblioteca.Domain.csproj Biblioteca.Domain/
COPY Biblioteca.Repositories/Biblioteca.Repositories.csproj Biblioteca.Repositories/

# Restaurar só o projeto API para trazer as libs
RUN dotnet restore Biblioteca.API/Biblioteca.API.csproj

# Copiar todo código fonte
COPY . .

# Publicar a API
WORKDIR /src/Biblioteca.API
RUN dotnet publish -c Release -o /app/publish

# Instalar a ferramenta 'dotnet-ef' globalmente
RUN dotnet tool install --global dotnet-ef

# Build final para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar os arquivos publicados da etapa build
COPY --from=build /app/publish .

# Adicionar o diretório da ferramenta ao PATH para que o comando 'dotnet ef' funcione
ENV PATH="$PATH:/root/.dotnet/tools"

RUN mkdir -p wwwroot/uploads

# Expor porta 80
EXPOSE 80

# Rodar a API
ENTRYPOINT ["dotnet", "Biblioteca.API.dll"]
