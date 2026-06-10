using System;
using System.Collections.Generic;

namespace PruebaLogin.Models;

public partial class Password
{
    public int UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
