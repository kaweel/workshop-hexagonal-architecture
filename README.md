# workshop-hexagonal-architecture

# 1. สร้าง Solution
dotnet new sln -n Order

# 2. สร้างแต่ละ Layer (Project)
dotnet new webapi -o API              # API Layer
dotnet new classlib -o Application     # Application Layer
dotnet new classlib -o Domain          # Domain Layer
dotnet new classlib -o Infrastructure  # Infrastructure Layer
dotnet new xunit -n MyTestProject

# 3. เพิ่มโปรเจกต์เข้า Solution
dotnet sln add API Application Domain Infrastructure

# 1. MyAPI ต้องอ้างอิง MyApplication
cd API
dotnet add reference ../Application/Application.csproj

# 2. Application ต้องอ้างอิง Domain
cd ../Application
dotnet add reference ../Domain/Domain.csproj

# 3. Infrastructure ต้องอ้างอิง Domain
cd ../Infrastructure
dotnet add reference ../Domain/Domain.csproj

