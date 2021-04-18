Proje kurulumu için gerekli adımlar.


    1. Projeyi git clone veya zip olarak indirin.


    2. Terminal üzerinde her katmana sırası ile geçiş yapıp " dotnet build " komutunu çalıştırın.


    3. WebUI katmanında terminal üzerinden " npm install " komutunu çalıştırın.


    4. WebUI katmanında Startup.cs dosyası içerisinde bulunan Sql Server dizin bilgilerini bilgisayarınıza göre ayarlayın.


    5. Data Access katmanında migration uygulayın.

1. *`dotnet ef migrations add lastCreate --startup-project ../shopapp.webui --context AppDbContext`*

2. *`dotnet ef database update --startup-project ../shopapp.webui  --context AppDbContext`*


    6. MVC katmanında migration uygulayın.


1.  *`dotnet ef migrations add identityMigration`*

2.  *`dotnet ef database update`*


    7. " dotnet run " komutu ile projeyi çalıştırabilirsiniz.