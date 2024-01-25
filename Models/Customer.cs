using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore7_webapi_authentication.Models;

[Table("customer")]
public partial class Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
}
