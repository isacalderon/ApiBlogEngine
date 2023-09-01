using System;
using System.Collections.Generic;

namespace ApiBlogEngine.Repository;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public byte[] Active { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
