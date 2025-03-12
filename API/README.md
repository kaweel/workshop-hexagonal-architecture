# 1. สร้าง Web API Project
dotnet new webapi -o API
dotnet sln add API

# 2. สร้าง Test Project สำหรับ API
dotnet new xunit -o APITest
dotnet sln add APITest
dotnet add APITest reference API

# 3. ติดตั้ง Dependencies
cd MyAPI
dotnet add package Microsoft.AspNetCore.Mvc.Testing  # ใช้ WebApplicationFactory สำหรับ Integration Test
dotnet add package Microsoft.EntityFrameworkCore.InMemory  # ใช้ Database In-Memory สำหรับทดสอบ
dotnet add package Moq  # ใช้ Mock Object ใน Unit Test

# 4. กลับไปที่โฟลเดอร์หลัก
cd ..
