--PORTUGUES
# Projeto de Avaliação de Desenvolvedor

`LEIA COM ATENÇÃO`

## Instruções

**O teste abaixo deverá ser entregue em até 7 dias corridos a partir da data de recebimento deste manual.**

* O código deve ser versionado em um repositório público do Github e o link deve ser enviado para avaliação assim que estiver concluído.
* Faça o upload deste template no seu repositório e comece a trabalhar a partir dele.
* Leia as instruções com atenção e certifique-se de que todos os requisitos estão sendo atendidos.
* O repositório deve fornecer instruções sobre como configurar, executar e testar o projeto.
* A documentação e a organização geral também serão levadas em consideração.

## Caso de Uso

**Você é um desenvolvedor na equipe da DeveloperStore. Agora, precisamos implementar os protótipos da API.**

Como trabalhamos com `DDD`, para referenciar entidades de outros domínios, utilizamos o padrão `External Identities` com desnormalização das descrições das entidades.

Portanto, você irá escrever uma API (CRUD completo) que gerencia registros de vendas. A API precisa ser capaz de informar:

* Número da venda
* Data em que a venda foi realizada
* Cliente
* Valor total da venda
* Filial onde a venda foi realizada
* Produtos
* Quantidades
* Preços unitários
* Descontos
* Valor total de cada item
* Cancelada/Não Cancelada

Não é obrigatório, mas seria um diferencial construir código para publicar eventos de:

* VendaCriada
* VendaModificada
* VendaCancelada
* ItemCancelado

Caso você escreva o código, **não é necessário** publicar para nenhum Message Broker. Você pode registrar uma mensagem no log da aplicação ou de qualquer outra forma que achar mais conveniente.

### Regras de Negócio

* Compras acima de 4 itens idênticos têm 10% de desconto
* Compras entre 10 e 20 itens idênticos têm 20% de desconto
* Não é possível vender mais de 20 itens idênticos
* Compras abaixo de 4 itens não podem ter desconto

Essas regras de negócio definem faixas de desconto baseadas na quantidade e limitações:

1. Faixas de Desconto:

   * 4+ itens: 10% de desconto
   * 10-20 itens: 20% de desconto

2. Restrições:

   * Limite máximo: 20 itens por produto
   * Não são permitidos descontos para quantidades abaixo de 4 itens

## Visão Geral

Esta seção fornece uma visão geral de alto nível do projeto e as diversas habilidades e competências que ele visa avaliar para os candidatos a desenvolvedor.

Veja [Visão Geral](/.doc/overview.md)

## Tecnologia Utilizada

Esta seção lista as principais tecnologias usadas no projeto, incluindo os componentes de backend, testes, frontend e banco de dados.

Veja [Tecnologia Utilizada](/.doc/tech-stack.md)

## Frameworks

Esta seção descreve os frameworks e bibliotecas que são utilizados no projeto para melhorar a produtividade e a manutenibilidade do desenvolvimento.

Veja [Frameworks](/.doc/frameworks.md)

<!-- 
## Estrutura da API
Esta seção inclui links para a documentação detalhada dos diferentes recursos da API:
- [API Geral](./docs/general-api.md)
- [API de Produtos](/.doc/products-api.md)
- [API de Carrinhos](/.doc/carts-api.md)
- [API de Usuários](/.doc/users-api.md)
- [API de Autenticação](/.doc/auth-api.md)
-->

## Estrutura do Projeto

Esta seção descreve a estrutura geral e a organização dos arquivos e diretórios do projeto.

Veja [Estrutura do Projeto](/.doc/project-structure.md)

--ENGLISH
# Developer Evaluation Project

`READ CAREFULLY`

## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**

- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)