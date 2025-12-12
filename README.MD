
---

# SIGEJ — Sistema de Informação de Gestão de Jardinagem e Manutenção

Projeto desenvolvido para apoiar a gestão de **jardinagem** e **manutenção** no **Campus Maracanaú — IFCE**, permitindo o controle de equipes, materiais, áreas do campus e ordens de serviço.

---

## 📌 Visão Geral

O **SIGEJ** é uma API desenvolvida em **C# (.NET 8)** utilizando **PostgreSQL** como banco de dados.
A aplicação oferece endpoints REST para gerenciamento completo de:

* Pessoas e equipes
* Áreas do campus
* Produtos, variações e estoque
* Ordens de serviço
* Relatórios operacionais
* Seeders e utilitários administrativos

O projeto utiliza **Dapper** para acesso a dados com SQL manual e **Swagger** para documentação interativa.

---

## 🚀 Tecnologias Utilizadas

* **.NET 8** (C#)
* **PostgreSQL**
* **Npgsql** (driver)
* **Dapper**
* **Swagger / Swashbuckle**
* **Bogus** (geração de dados de teste)

---

## 🗂 Estrutura do Projeto

```
/sigej
  /api               # Controllers da API
  /db                # Conexão, repositórios e seeders
  /domain            # Entidades, DTOs e modelos
  sigej.csproj
  sigej.sln
```

---

## 🛠 Pré-requisitos

Antes de executar o projeto, instale:

* .NET 8 SDK
* PostgreSQL
* Ferramentas opcionais: Postman, Insomnia ou Swagger via navegador

---

## 🗄 Configuração do Banco de Dados

A conexão padrão definida no código é:

```
Host=localhost;Username=sigej_admin;Password=sigejadmin123;Database=sigej_db
```

### Criando usuário e banco no PostgreSQL

```sql
CREATE ROLE sigej_admin WITH LOGIN PASSWORD 'sigejadmin123';
CREATE DATABASE sigej_db OWNER sigej_admin;
GRANT ALL PRIVILEGES ON DATABASE sigej_db TO sigej_admin;
```

### Criação das tabelas

O script SQL do modelo relacional está descrito no documento do projeto (PDF anexado).
Basta executá-lo após criar o banco.

> O sistema também possui *seeders* para popular dados automaticamente.

---

## ▶ Como Executar

1. No terminal:

```bash
cd sigej
dotnet restore
dotnet build
dotnet run --project ./sigej.csproj
```

2. A API iniciará localmente, exibindo a porta no console.
3. Acesse o Swagger:

```
https://localhost:<porta>/swagger
```

---

## 📡 Endpoints Principais

| Recurso          | Endpoint                 |
| ---------------- | ------------------------ |
| Áreas do campus  | `GET /api/areaCampus`    |
| Pessoas          | `GET /api/pessoas`       |
| Produtos         | `GET /api/produtos`      |
| Estoque          | `GET /api/estoque`       |
| Ordem de Serviço | `POST /api/ordemServico` |
| Relatórios       | `GET /api/relatorios`    |

### Endpoints Administrativos

⚠ Para uso em desenvolvimento:

* `POST /api/admin/populate` — Popular banco com dados falsos
* `DELETE /api/admin/clean-database` — Limpar completamente o banco

---

## 🌱 Seed (População de Dados)

O seeder se encontra em:

```
sigej/db/seed/DbSeeder.cs
```

Execute via endpoint:

```
POST /api/admin/populate
```

## ✍️ Desenvolvido por

* **Matheus Castelo**
* **Breno Vinicius**
* **Sofia Prata**
* **Vinicius Kainan**

---

## 🎓 Trabalho Acadêmico

Este projeto foi desenvolvido como **Trabalho da disciplina de Banco de Dados**
do **Instituto Federal de Educação, Ciência e Tecnologia do Ceará — Campus Maracanaú**.

---
