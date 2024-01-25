Libraries

1.  Microsoft.EntityFrameworkCore
2.  Microsoft.EntityFrameworkCore.SqlServer
3.  Microsoft.EntityFrameworkCore.Tools
4.  Microsoft.AspNetCore.Authentication.JwtBearer

Database First Command
-> dotnet ef dbcontext scaffold "Name=ConnectionStrings:Mssql" Microsoft.EntityFrameworkCore.SqlServer  --context-dir Data --output-dir Models --data-annotations --force
