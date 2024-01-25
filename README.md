Libraries

1.  Microsoft.EntityFrameworkCore
2.  Microsoft.EntityFrameworkCore.SqlServer
3.  Microsoft.EntityFrameworkCore.Tools
4.  Microsoft.AspNetCore.Authentication.JwtBearer
5.  BCrypt.Net-Next

Database First Command
-> dotnet ef dbcontext scaffold "Name=ConnectionStrings:Mssql" Microsoft.EntityFrameworkCore.SqlServer  --context-dir Data --output-dir Models --data-annotations --force

Database script

    -> Customer table

    CREATE TABLE [dbo].[customer](
        [id] [int] IDENTITY(1,1) NOT NULL,
        [name] [varchar](50) NOT NULL,
    CONSTRAINT [PK_customer] PRIMARY KEY CLUSTERED 
    (
        [id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO

    -> User table
    
    CREATE TABLE [dbo].[user](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [username] [varchar](50) NOT NULL,
        [password] [varchar](250) NOT NULL,
        [refresh_token] [varchar](250) NULL,
        [Role] [varchar](50) NULL,
    CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO