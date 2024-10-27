# ASP.NET Core API com JWT Authentication e CRUD para Usuários

## Visão Geral

Esta aplicação é uma API RESTful desenvolvida com **ASP.NET Core** que implementa autenticação baseada em **JWT** (JSON Web Token) e oferece operações CRUD (Create, Read, Update, Delete) para gerenciar recursos de usuários. Utilizamos o **Entity Framework Core** com SQLite para persistência de dados, o que facilita o gerenciamento e a consulta de dados de forma eficiente.

## Funcionalidades

- **Autenticação JWT**: Protege os endpoints para acesso seguro.
- **CRUD Completo para Usuários**: Permite criação, leitura, atualização e exclusão de usuários no banco de dados.
- **Banco de Dados SQLite**: Simples e eficaz para armazenamento local e persistência de dados.
- **Hospedagem com Docker**: Configuração completa para executar a aplicação em contêineres Docker.

## Configuração e Pré-Requisitos

### Pré-Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

### Instalação

1. Clone o repositório:

   ```bash
   git clone https://github.com/seu-usuario/sua-aplicacao.git
   cd sua-aplicacao
   ```

2. Configure o banco de dados e instale as dependências com o Entity Framework Core:

   ```bash
   dotnet restore
   dotnet ef database update
   ```

3. Execute o projeto em desenvolvimento:

   ```bash
   dotnet run
   ```

### Executando com Docker

1. Construa a imagem Docker:

   ```bash
   docker-compose build
   ```

2. Inicie o contêiner:

   ```bash
   docker-compose up -d
   ```

3. A aplicação estará disponível em `http://localhost:80` (mapeado para o porto `8080` no contêiner).

## Endpoints

### Autenticação

#### `POST /api/login`

- **Descrição**: Gera um token JWT para autenticação.
- **Parâmetros**:
  - `username`: Nome do usuário (exemplo: `admin`)
  - `password`: Senha do usuário (exemplo: `password`)
- **Exemplo**:

  ```bash
  curl -X POST http://localhost/api/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password"
  }'
  ```

### Usuários

Todos os endpoints abaixo requerem autenticação JWT. Use o token obtido no login para autenticar.

#### `GET /api/users`

- **Descrição**: Retorna a lista de todos os usuários.
- **Exemplo**:

  ```bash
  curl -X GET http://localhost/api/users \
  -H "Authorization: Bearer {seu_token_jwt}"
  ```

#### `POST /api/users`

- **Descrição**: Cria um novo usuário.
- **Parâmetros**:
  - `name`: Nome do usuário.
  - `age`: Idade do usuário.
- **Exemplo**:

  ```bash
  curl -X POST http://localhost/api/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {seu_token_jwt}" \
  -d '{
    "name": "John Doe",
    "age": 30
  }'
  ```

#### `GET /api/users/{id}`

- **Descrição**: Retorna um usuário específico por ID.
- **Parâmetros**:
  - `{id}`: ID do usuário.
- **Exemplo**:

  ```bash
  curl -X GET http://localhost/api/users/{id} \
  -H "Authorization: Bearer {seu_token_jwt}"
  ```

#### `PUT /api/users/{id}`

- **Descrição**: Atualiza os dados de um usuário específico.
- **Parâmetros**:
  - `{id}`: ID do usuário.
  - `name`: Novo nome do usuário.
  - `age`: Nova idade do usuário.
- **Exemplo**:

  ```bash
  curl -X PUT http://localhost/api/users/{id} \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {seu_token_jwt}" \
  -d '{
    "name": "Jane Doe",
    "age": 31
  }'
  ```

#### `DELETE /api/users/{id}`

- **Descrição**: Exclui um usuário específico por ID.
- **Parâmetros**:
  - `{id}`: ID do usuário.
- **Exemplo**:

  ```bash
  curl -X DELETE http://localhost/api/users/{id} \
  -H "Authorization: Bearer {seu_token_jwt}"
  ```

## Estrutura de Diretórios

```plaintext
├── Controllers            # Controladores e rotas da aplicação
├── Data                   # Configuração do banco de dados e contexto
├── Models                 # Definição dos modelos de dados (ex. User, UserLogin)
├── Dockerfile             # Dockerfile para construção do ambiente Docker
├── docker-compose.yml     # Configuração de serviço Docker Compose
└── Program.cs             # Ponto de entrada da aplicação
```

## Segurança e Observações

- **JWT Key**: Certifique-se de que a chave secreta JWT (`BDsRJ3ypxC10vRaGn8/2Zbj11k89zJDU5UqtJl5BprA=`) seja mantida segura e nunca exposta publicamente. Em produção, você deve usar variáveis de ambiente para armazenar a chave de autenticação.
- **Persistência de Dados**: O arquivo `users.db` é utilizado para persistência dos dados localmente. Em um ambiente de produção, considere utilizar um sistema de banco de dados mais robusto, como SQL Server, PostgreSQL ou MySQL.

## Referências

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [JSON Web Token (JWT) Introduction](https://jwt.io/introduction)
