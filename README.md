# 🍴 LazardCantine

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/votre-dépôt/lazardcantine/actions) 
[![Tests](https://img.shields.io/badge/tests-100%25-success)](https://github.com/votre-dépôt/lazardcantine/actions) 
[![Framework](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue)](https://dotnet.microsoft.com/) 
[![License](https://img.shields.io/badge/license-MIT-green)](https://opensource.org/licenses/MIT) 
[![Coverage](https://img.shields.io/badge/coverage-90%25-yellowgreen)](https://github.com/votre-dépôt/lazardcantine/actions)

**LazardCantine** est une application développée en **ASP.NET Core** permettant de gérer les clients d'une cantine d'entreprise, leurs balances, les produits proposés, les plateaux repas, et les règles de paiement. Le projet suit une architecture bien structurée qui inclut des contrôleurs, des services, une logique métier, et des tests unitaires pour garantir la qualité du code.

---

## 📂 Structure du projet

```bash
usrunknow-lazardcantine/
├── 📄 LazardCantine.sln        # Fichier de solution
├── 📄 Program.cs              # Point d'entrée de l'application
├── 📄 appsettings.json        # Fichier de configuration
├── 📂 Controllers/            # Gestion des requêtes API
│   ├── 📄 ClientController.cs
│   ├── 📄 PlateauController.cs
│   └── 📄 ProduitController.cs
├── 📂 Models/                 # Entités et structures principales
│   ├── 📄 Client.cs            # Représentation d'un client
│   ├── 📄 Produit.cs           # Représentation d'un produit
│   └── 📄 Ticket.cs            # Représentation d'un ticket d'achat
├── 📂 Services/               # Logiques métier
│   ├── 📄 ClientService.cs     
│   ├── 📄 PlateauService.cs
│   └── 📄 ProduitService.cs
├── 📂 Tests/                  # Tests unitaires
│   ├── 📄 ClientServiceTests.cs
│   ├── 📄 PlateauServiceTests.cs
│   └── 📄 ProduitServiceTests.cs
```

---

## ✨ Fonctionnalités de l'application

### 👥 Gestion des clients
- Ajouter des clients avec un solde initial et un type (Interne, VIP, etc.).
- Consulter la liste des clients enregistrés.
- Modifier le solde d'un client en l'accréditant.

### 🍽 Gestion des plateaux repas
- Finaliser le paiement des plateaux repas préparés par les clients.
- Calcul du montant total basé sur les réductions selon le type de client.
- Génération de tickets détaillés après paiement.

### 🍎 Gestion des produits
- Ajouter, consulter, ou supprimer les produits proposés par la cantine.
- Catégorisation des produits (Entrée, Plat, Dessert, Boisson, etc.).

---

## 🛠 Installation et utilisation

### Configuration locale
1. Cloner ce dépôt Git :
   ```bash
   git clone https://github.com/votre-dépôt/lazardcantine.git
   ```
2. Naviguer vers le répertoire de la solution :
   ```bash
   cd lazardcantine
   ```
3. Construire la solution :
   ```bash
   dotnet build
   ```

### Lancer l'application
1. Démarrer le serveur avec la commande suivante :
   ```bash
   dotnet run --project LazardCantine
   ```
2. Ouvrir votre navigateur et accéder à l'adresse suivante :
   ```
   http://localhost:5200/swagger
   ```

---

## ✅ Tests

### Exécution des tests unitaires
1. Naviguer vers le répertoire des tests :
   ```bash
   cd Tests
   ```
2. Lancez les tests avec la commande suivante :
   ```bash
   dotnet test
   ```
3. Les tests valideront plusieurs cas :
   - 👤 Ajouter et modifier des clients.
   - 🍽 Paiement des plateaux et gestion des réductions.
   - 🍎 Manipulation des produits (CRUD).

---

## 🛠 Technologies utilisées

- ⚙️ **ASP.NET Core 8.0** : Framework principal de l'application.
- 🐘 **Entity Framework Core** : Base de données en mémoire pour simplifier les tests.
- 🧪 **xUnit** : Framework de test unitaire.
- 🤖 **Moq** : Pour les mocks dans les tests.

---

## 🚀 Améliorations futures

- 🔒 Mise en place d'un système d'authentification pour protéger les endpoints API.
- 📊 Ajouter des statistiques des ventes pour les administrateurs.
- 📦 Permettre l'intégration avec une base de données persistance (par ex. SQL Server).

---

Si vous avez des questions ou des suggestions, créez une issue dans le dépôt. Bon développement ! 🎉
