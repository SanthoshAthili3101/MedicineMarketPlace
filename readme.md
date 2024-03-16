## MedicineMarketDbContext
- `Add-Migration {Migration Name} -Context MedicineMarketDbContext -OutputDir Migrations -Project  MedicineMarketPlace.Shared -StartupProject MedicineMarketPlace.Admin.Api`
- `Update-Database -Context MedicineMarketDbContext -Project MedicineMarketPlace.Shared -StartupProject MedicineMarketPlace.Admin.Api`
- `Remove-Migration -Context MedicineMarketDbContext -Project MedicineMarketPlace.Shared -StartupProject MedicineMarketPlace.Admin.Api`
- `Script-Migration -Context MedicineMarketDbContext -Project MedicineMarketPlace.Shared -StartupProject MedicineMarketPlace.Admin.Api`
- `Update-Database -Context MedicineMarketDbContext -Project MedicineMarketPlace.Shared -StartupProject MedicineMarketPlace.Admin.Api -Migration {MigrationName}`