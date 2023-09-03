using System.ComponentModel.DataAnnotations;

public class PostDto {

    public int Id { get; set; }
    [Required]
    [MaxLength(50)]    
    public String? Title { get; set; }

    [Required]
    public String? Content { get; set; }
    
}