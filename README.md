

# MotoRental

## Resumo:

Sistema de gerenciamento de aluguel de motos e entregadores.

<h4 align="center"> 
 🚧  Status do Projeto: Finalizado 🚧
</h4>

## Como subir o projeto

**IMPORTANTE:** Deve ser solicitado ao autor do projeto a **Chave de Conexão** e o **Nome do Container** para preencher o `appsettings.json` da API. Sem esses dados o sistema de envio de imagem da CNH não funcionará.

Estando na raiz do projeto, dar o comando `docker compose up -d`.
A API subirá ouvindo a porta 5000 (acessível em `http://localhost:5000/swagger/index.html`).

Se preferir subir via código, certifique-se de ter um banco de dados Postgres e mensageria com RabbitMQ ajustados com os dados do `appsettings.json`. Em seguida basta dar o comando `dotnet run` nos diretórios `src/MotoRental.API` para iniciar a API e no diretório `src/MotoRental.Messaging` para iniciar o consumer.

## Rodando os testes:

Basta dar o comando `dotnet test` estando no diretório `src/MotoRental.Test`.

## Observações
Ao subir o projeto, um usuário com permissão de administrador será criado de forma automática. Os dados de acesso para login são:
- Email: admin@email.com
- Senha: teste123

### Se autenticando pelo Swagger:
![recording-2024-12-13-13-26-12-ezgif com-video-to-gif-converter](https://github.com/user-attachments/assets/e6889456-b2c2-42ee-901b-0eab79888fa7)

## Caso de uso:

- Usuário anônimo:
  - Tem acesso ao endpoint de cadastro de usuário
    - Dados necessários para cadastro: nome, email e senha
    - Email precisa ser único
    - Senha deve ter pelo menos 8 caracteres
  - Tem acesso ao endpoint de login
    - Dados necessários para login: email e senha

- Usuário entregador:
  - Tem acesso aos endpoints acima
  - Tem acesso ao endpoint de cadastro de entregador
    - Dados necessários para cadastro: nome, CNPJ, data de nascimento, numero da CNH, tipo da CNH
    - CNPJ precisa ser válido
    - CNPJ e CNH precisam ser únicos
    - Permitido apenas maiores de 18 anos
  - Tem acesso ao endpoint de envio de imagem da CNH
    - Enviar imagem no formato base64
  - Tem acesso ao endpoint de criação de aluguel
    - Dados necessários para criação: id do entregador, id da moto, data de início, data de término, previsão de entrega, plano
    - Planos disponíveis:
      - 7 dias com um custo de R$30,00 por dia
      - 15 dias com um custo de R$28,00 por dia
      - 30 dias com um custo de R$22,00 por dia
      - 45 dias com um custo de R$20,00 por dia
      - 50 dias com um custo de R$18,00 por dia
    - Data de ínicio é obrigatorimente um dia após a criação
    - Data de término varia de acordo com o plano
    - Data de previsão da entrega deve ser maior que a data de início e menor ou igual à data de término
  - Tem acesso ao endpoint de finalização de aluguel
    - A data de finalização deve ser maior que a data de início
    - Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
      - Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
      - Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
  - Tem acesso ao endpoint de busca de aluguel por id
 
- Usuário administrador:
  - Tem acesso aos endpoints acima
  - Tem acesso ao endpoint de dar/remover permissão de outros usuários
  - Tem acesso ao endpoint de cadastro de moto
    - Dados necessários para cadastro: ano, modelo e placa
    - Registros de motos devem ser feito com disparo de eventos para um consumer
    - Placa precisa ser única
    - Ao ser registrado uma moto do ano 2024, uma notificação deve ser lançada (um log no consumer)
  - Tem acesso ao endpoint de atualização da placa de moto já cadastrada
    - Não pode ser uma placa já existente
  - Tem acesso ao endpoint de busca de moto por id
  - Tem acesso ao endpoint de busca de motos pela placa
  - Tem acesso ao endpoint de deleção de moto por id
    - Motos que já foram alugadas não podem ser excluídas

## 🛠 Tecnologias usadas

* .NET 8
* Azure (Blob Storage)
* Postgres

## 🛠 Diferenciais

* Testes unitários e de integração com xUnit
* EntityFramework
* Docker e Docker Compose
* Design Patterns (Factory, CQRS, Adapter...)
* Documentação organizada com Swagger
* Tratamento de erros em todos os endpoints com uma mensagem personalizada para cada
* Arquitetura Limpa
* Código escrito em lingua inglesa
* Código limpo e organizado com boas práticas
* Logs bem estruturados
* Convenções utilizadas pela comunidade

## 🔗 Links
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/vitor-marciano/)
