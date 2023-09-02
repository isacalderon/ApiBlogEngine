public interface ICommentService
{
    CommentDto CreateCommentAsync(string AuthorEmail, CommentDto comment);

}