using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using dotnetcore7_webapi_authentication.Models;

namespace dotnetcore7_webapi_authentication.Data;

public partial class Dotnetcore7WebapiAuthenticationDbContext : DbContext
{
    public Dotnetcore7WebapiAuthenticationDbContext()
    {
    }

    public Dotnetcore7WebapiAuthenticationDbContext(DbContextOptions<Dotnetcore7WebapiAuthenticationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Mssql");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
