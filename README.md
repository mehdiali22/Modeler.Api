# Modeler05 Relational API (No Versioning + INT IDs)

✅ Versioning حذف شده  
✅ Id ها **INT Identity** هستند (Guid نیست)  
✅ Entity ها جدا جدا در فایل‌های جدا: `Domain/Entities/*.cs`  
✅ DTO ها جدا جدا: `Dtos/*Dto.cs`  
✅ بدون Top-Level Statements: Program.cs + Startup.cs  

## نکته مهم
اگر UI هنوز `id: string` داشته باشد، باید UI را به `number` تغییر بدهی (و همه FK ها هم number شوند).
در غیر این صورت، باید مدل "UiId" جداگانه بسازیم.

## Migration
```bash
cd src/Modeler.Api
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial
dotnet ef database update
dotnet run
```

## Import/Export
- `GET  /api/tools/export`
- `POST /api/tools/import?mode=replace|upsert`

در حالت `replace`، اگر export شامل Id های صریح باشد، API با `SET IDENTITY_INSERT` همان Id ها را حفظ می‌کند.
