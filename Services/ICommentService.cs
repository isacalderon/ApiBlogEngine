public interface ICommentService
{
    CommentDto CreateCommentAsync(string AuthorEmail, CommentDto comment);

    IEnumerable<CommentDto> GetComments(int postId);

}