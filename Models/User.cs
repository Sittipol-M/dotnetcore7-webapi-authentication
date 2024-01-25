using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnetcore7_webapi_authentication.Models;

[Table("user")]
public partial class User
{
    [Key]
    public int Id { get; set; }
}
