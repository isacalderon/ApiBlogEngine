using System;
using System.Collections.Generic;

namespace ApiBlogEngine.Repository;

public partial class PostStatus
{
    public int Id { get; set; }

    public int Post { get; set; }

    public string CommentEditor { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int Status { get; set; }

    public virtual Post PostNavigation { get; set; } = null!;

    public virtual Status StatusNavigation { get; set; } = null!;
}
