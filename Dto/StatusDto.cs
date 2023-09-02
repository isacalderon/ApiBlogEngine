using System.ComponentModel.DataAnnotations;

public class StatusDto
{
    [Required]
    public int Status { get; set; }

    [Required]
    public int PostId { get; set; }

    [StringLength(100)]
    public string? CommentEditor { get; set; }
}