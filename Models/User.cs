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

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(250)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("refresh_token")]
    [StringLength(250)]
    [Unicode(false)]
    public string? RefreshToken { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Role { get; set; }
}
