# DVLD Project

## 🧠 Overview
**DVLD Project** is a multi-layered **C#/.NET** application that follows a clean architecture pattern.  
It’s designed to separate responsibilities across layers — **Data Access**, **Business Logic**, and **Presentation (UI)** — to ensure scalability and maintainability.

---

## 🏗️ Project Structure
├── DVLD_Buisness
├── DVLD_DataAccess
├── DVLD_Project


- **DVLD_Buisness**: Business logic layer  
- **DVLD_DataAccess**: Data access / repository / database layer  
- **DVLD_Project**: The presentation/UI / main app  
- `.gitignore`: Specifies files/folders to be ignored by Git  

---


### Layers Description
- **DVLD_DataAccess** → Handles database operations and connections.  
- **DVLD_Buisness** → Contains core business rules and processing logic.  
- **DVLD_Project** → User interface or main entry point (WinForms / WPF / Console).  

---

## ⚙️ Requirements
- **.NET SDK** (6.0 or later recommended)  
- **Visual Studio 2022** or **VS Code with C# extension**  
- **SQL Server / LocalDB** (if applicable)  

---

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/HAMZALAROUZ/DVLD-Project.git
   cd DVLD-Project
