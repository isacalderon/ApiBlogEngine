using System;
using System.Collections.Generic;

namespace ApiBlogEngine.Repository;

public partial class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int Author { get; set; }

    public int Post { get; set; }

    public virtual User AuthorNavigation { get; set; } = null!;

    public virtual Post PostNavigation { get; set; } = null!;
}
