## Pré requisitos configuração do ambiente

- [Docker](https://docs.docker.com/desktop/install/windows-install/)
- [NPM](https://nodejs.org/en/download/package-manager)
- [DotNet](https://dotnet.microsoft.com/pt-br/download)


## Iniciando o ambiente

### Infra

- Na pasta raiz, executar o comando:
    - docker compose up -d

### Frontend

- Abrir pasta ui
- Executar comando para instalar dependências:
  ``` 
  npm install
  ```
- Executar comando para iniciar projeto
  ```
  npm run start
  ```

### Backend
- Abrir pasta api\StockMax
- Executar comando para instalar dependências:
  ``` 
  dotnet restore
  ```
- Executar comando para iniciar projeto
  ```
  dotnet run
  ```
### Testes

- Acessar a URL: http://localhost:4200
- Usuario: etgc3023.life@gmail.com
- Senha: 123qwe