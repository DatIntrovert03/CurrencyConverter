💱 Currency Converter (WPF + SQL Server)

A desktop currency conversion app built with WPF and backed by SQL Server. Designed for clarity, speed, and a smooth user experience—with enhanced UI layout and robust data handling.

🖼️ UI Preview

The app features a clean interface with:
- Rounded buttons for modern aesthetics
- Readable DataGrid for displaying conversion history
- Currency binding logic for dynamic dropdowns

🧰 Tech Stack

| Layer        | Technology         |
|--------------|--------------------|
| UI           | WPF (.NET, XAML)   |
| Backend      | C#                 |
| Database     | SQL Server         |
| ORM          | Entity Framework   |

## 📁 Project Structure

```
CurrencyConverter/
├── App.config              # SQL Server connection string
├── App.xaml               # Global styles (rounded buttons, bindings)
├── MainWindow.xaml        # UI layout and controls
├── MainWindow.xaml.cs     # Event handling and logic
├── CurrencyConverter.sln  # Solution file
├── Models/                # Entity classes
├── Resources/             # Icons, styles
└── README.md
```

## 🗃️ Database Schema

- **Currencies**: CurrencyCode, Name, Symbol
- **ExchangeRates**: FromCurrency, ToCurrency, Rate, Timestamp
- **Users**: Username, PasswordHash, Role

Foreign keys ensure clean relationships between currencies and rates.

⚙️ Setup Instructions

1. **Clone the repo**
   ```bash
   git clone https://github.com/DatIntrovert03/CurrencyConverter.git
   ```

2. **Open in Visual Studio**
   - Launch `CurrencyConverter.sln`

3. **Configure SQL Server**
   - Update `App.config`:
     ```xml
     <connectionStrings>
       <add name="CurrencyDB" 
            connectionString="Server=YOUR_SERVER;Database=CurrencyDB;Trusted_Connection=True;" 
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

4. **Build and Run**
   - Press `F5` or click **Start**

🔐 Security Notes

- Passwords hashed securely
- Role-based access control
- SQL injection protection via Entity Framework

📦 Data Import

- CSV/Excel support for bulk exchange rate updates
- Validates format and handles duplicates

🤝 Contributing

Pull requests welcome! For major changes, open an issue first to discuss your ideas.
