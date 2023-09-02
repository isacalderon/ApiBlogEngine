using System.ComponentModel.DataAnnotations;

public class CommentDto
{
    [Required]
    public string? Comment { get; set; }

    [Required]
    public string? Author { get; set; }
    public int PostId { get; set; }
}