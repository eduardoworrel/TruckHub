# 🚛 TruckHub [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=eduardoworrel_TruckHub&metric=coverage)](https://sonarcloud.io/summary/new_code?id=eduardoworrel_TruckHub)

Este projeto implementa o gerenciamento simples de caminhões.

## Executar Projeto

Clone o repositório
```cmd
git clone https://github.com/eduardoworrel/TruckHub
```

Inicialize via docker compose

```cmd
cd TruckHub
docker compose up -d
```

Acesse [http://localhost:3039](http://localhost:3039)

🧪 Testes

```cmd
cd TruckHub/Backend
dotnet test
```


Inicialização sem docker compose
```cmd
cd TruckHub/Backend/src/WebApi
mv appsettings.Example.json appsettings.json
dotnet run // https://localhost:7006/swagger

cd TruckHub/Frontend/
npm install
npm run dev // http://localhost:3039
```


## Funcionalidades Principais

- CRUD completo com adição e deleção em massa.
- Testes unitários segregado por camadas. 
- Integração front-end e back-end via API REST.

## Tecnologias

- .NET 8
 - Code First.
 - Entity Framework
 - Migratins
- SQLite (banco de dados local)
- React & Typescript
 - Material UI
 - Template de dashboard
- Docker & Docker Compose
- Github Actions
- SonarCloud