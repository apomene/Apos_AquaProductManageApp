# WinForms MVP Application (.NET 9 + SQL Server 2019)

## Overview

This is a Windows Forms application built using **.NET 9** that connects to a **SQL Server 2019** database. The application follows the **Model-View-Presenter (MVP)** architectural pattern to promote separation of concerns and improve testability and maintainability.

The solution includes multiple forms, each representing a UI module with its own presenter and model logic.

---

## Features

- 🌐 **.NET 9**-based WinForms UI
- 🗃️ Integration with **SQL Server 2019**
- 🧱 Uses **MVP pattern** for all form-level logic separation
- 🔌 Dependency Injection support
- 🛠️ Configurable form sizing and UI settings
- 📦 Organized project structure (Forms, Presenters, Models, Utilities)

---

## Project Structure


---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server 2019](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022 or newer

---

## Getting Started

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/your-winforms-app.git
   cd your-winforms-app

2.et up the database:

    Create a new database in SQL Server 2019.

    Run the provided SQL scripts (in /FishFarmDB) to initialize tables and seed data.

    Update the connection string in App.config:
    ```xml

    <connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=YOUR_SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
    ```


