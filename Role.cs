using System;
using System.Collections.Generic;

namespace M2Api;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
