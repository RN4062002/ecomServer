using System;
using System.Collections.Generic;

namespace ecomServer.Models;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string UserTypeDesc { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
