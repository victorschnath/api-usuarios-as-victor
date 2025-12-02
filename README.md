# API de Gerenciamento de Usuários

## Descrição

Esta API REST foi desenvolvida para gerenciar usuários de uma plataforma digital, permitindo operações completas de CRUD (Create, Read, Update, Delete). A solução foi implementada seguindo os princípios de Clean Architecture, com separação clara de responsabilidades em camadas bem definidas (Domain, Application e Infrastructure).

A API utiliza ASP.NET Core com Minimal APIs, Entity Framework Core para persistência de dados em SQLite, e FluentValidation para validação rigorosa dos dados de entrada. O projeto implementa padrões de projeto (Design Patterns) como Repository Pattern e Service Pattern, garantindo escalabilidade e facilidade de manutenção.

## Tecnologias Utilizadas
- .NET 8.0 - Framework principal
- ASP.NET Core - Framework web com Minimal APIs
- Entity Framework Core 8.0 - ORM para acesso a dados
- SQLite - Banco de dados relacional
- FluentValidation 11.3 - Validação de dados
- Swashbuckle.AspNetCore - Documentação Swagger/OpenAPI

## Padrões de Projeto Implementados

- Repository Pattern - Abstração da camada de persistência de dados
- Service Pattern - Lógica de negócio e orquestração
- DTO Pattern - Transferência de dados entre camadas
- Dependency Injection - Inversão de controle e injeção de dependências

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK 8.0 ou superior

### Passos

1. Clone o repositório
   ```bash
   git clone <url-do-repositorio>
   cd ASDOPROFESSORFOGAO
   ```

2. Restaure as dependências
   ```bash
   dotnet restore
   ```

3. Compile o projeto
   ```bash
   dotnet build
   ```

4. Execute a aplicação
   ```bash
   dotnet run
   ```
## Exemplos de Requisições

### Criar Usuário (POST /usuarios)

```bash
curl -X POST "https://localhost:5001/usuarios" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao.silva@email.com",
    "senha": "senha123",
    "dataNascimento": "1990-05-15T00:00:00",
    "telefone": "(11) 98765-4321"
  }'
```

Resposta (201 Created):
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao.silva@email.com",
  "dataNascimento": "1990-05-15T00:00:00",
  "telefone": "(11) 98765-4321",
  "ativo": true,
  "dataCriacao": "2025-01-01T10:00:00"
}
```

### Listar Todos os Usuários (GET /usuarios)

```bash
curl -X GET "https://localhost:5001/usuarios"
```

Resposta (200 OK):
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao.silva@email.com",
    "dataNascimento": "1990-05-15T00:00:00",
    "telefone": "(11) 98765-4321",
    "ativo": true,
    "dataCriacao": "2025-01-01T10:00:00"
  }
]
```

### Buscar Usuário por ID (GET /usuarios/{id})

```bash
curl -X GET "https://localhost:5001/usuarios/1"
```

### Atualizar Usuário (PUT /usuarios/{id})

```bash
curl -X PUT "https://localhost:5001/usuarios/1" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva Santos",
    "email": "joao.silva@email.com",
    "dataNascimento": "1990-05-15T00:00:00",
    "telefone": "(11) 98765-4321",
    "ativo": true
  }'
```

### Remover Usuário (DELETE /usuarios/{id})

```bash
curl -X DELETE "https://localhost:5001/usuarios/1"
```

Resposta (204 No Content): Sem conteúdo

## Endpoints da API

| Método | Endpoint | Descrição | Status Code Sucesso |
|--------|----------|-----------|---------------------|
| GET | /usuarios | Lista todos os usuários | 200 OK |
| GET | /usuarios/{id} | Busca usuário por ID | 200 OK |
| POST | /usuarios | Cria novo usuário | 201 Created |
| PUT | /usuarios/{id} | Atualiza usuário completo | 200 OK |
| DELETE | /usuarios/{id} | Remove usuário (soft delete) | 204 No Content |

### Códigos de Erro

- 400 Bad Request - Validação falhou
- 404 Not Found - Usuário não encontrado
- 409 Conflict - Email já cadastrado
- 500 Internal Server Error - Erro no servidor

## Estrutura do Projeto

```
ASDOPROFESSORFOGAO/
├── Domain/
│   └── Entities/
│       └── Usuario.cs               Entidade de domínio
│
├── Application/
│   ├── DTOs/
│   │   ├── UsuarioCreateDto.cs      DTO para criação
│   │   ├── UsuarioReadDto.cs        DTO para leitura
│   │   └── UsuarioUpdateDto.cs      DTO para atualização
│   │
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs     Interface do repositório
│   │   └── IUsuarioService.cs        Interface do serviço
│   │
│   ├── Services/
│   │   └── UsuarioService.cs        Lógica de negócio
│   │
│   └── Validators/
│       ├── UsuarioCreateDtoValidator.cs   Validador de criação
│       └── UsuarioUpdateDtoValidator.cs   Validador de atualização
│
├── Infrastructure/
│   ├── Persistence/
│   │   └── AppDbContext.cs          Contexto do Entity Framework
│   │
│   └── Repositories/
│       └── UsuarioRepository.cs      Implementação do repositório
│
├── Migrations/
│   └── (migrations do Entity Framework)
│
├── Program.cs                        Configuração e endpoints
├── appsettings.json                  Configurações da aplicação
└── ASDOPROFESSORFOGAO.csproj         Arquivo do projeto
```

### Descrição das Camadas

- Domain: Contém as entidades de domínio e regras de negócio puras
- Application: Contém DTOs, interfaces, serviços e validadores (camada de aplicação)
- Infrastructure: Contém implementações concretas (banco de dados, repositórios)

## Autor

Victor Schnath da Silva  
Curso: Análise e Desenvolvimento de Sistemas - Desenvolvimento Back-End
Youtube: https://www.youtube.com/watch?v=au3n0vda-gA




