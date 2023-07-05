dotnet tool install --global dotnet-ef
# dotnet ef migrations add initial --project .\Contacts
dotnet ef migrations script --idempotent --project .\Contacts
