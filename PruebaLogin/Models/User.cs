using System;
using System.Collections.Generic;

namespace PruebaLogin.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? RecordDate { get; set; }

    public virtual Password? Password { get; set; }
}
