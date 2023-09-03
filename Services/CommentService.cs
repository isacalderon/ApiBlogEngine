using ApiBlogEngine.Repository; 
using ApiBlogEngine.Dto;
public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> _logger;

    private readonly BlogEngineContext _context;

    public CommentService(ILogger<CommentService> logger, BlogEngineContext context)
    {
        _logger = logger;
        _context = context;
    }
    public CommentDto CreateCommentAsync(string AuthorEmail, CommentDto comment)
    {
        int? authorId = _context.Users.Where(u => u.Email == AuthorEmail).FirstOrDefault()?.Id;
        Comment newComment = new Comment();
        newComment.Author = authorId?? 0;
        newComment.Content = comment.Comment?? "";
        newComment.Post = comment.PostId;
        _context.Comments.Add(newComment);

        _context.SaveChanges(); 
        return comment;
    }

    public IEnumerable<CommentDto> GetComments(int postId)
    {
        var comments = _context.Comments.Where(c => c.Post == postId).ToList();
        List<CommentDto> commentsDto = new List<CommentDto>();
        foreach (var comment in comments)
        {
            commentsDto.Add(new CommentDto(){
                Comment = comment.Content,
                PostId = comment.Post
            });
        }
        return commentsDto;
    }
}