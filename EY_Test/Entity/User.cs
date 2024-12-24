using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace EY_Test.Entity;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
    
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
}
