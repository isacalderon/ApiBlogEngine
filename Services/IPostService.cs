using ApiBlogEngine.Dto;
public interface IPostService
{
    IEnumerable<PostDto> GetAllPostsAsyn();
    IEnumerable<PostDto> GetPostsByAuthor(int authorId);
    IEnumerable<PostDto> GetPostByStatus(Enum status);
    PostDto GetPostsById(int id);
    PostDto CreatePostAsync(string email, PostDto newPost);
    Task UpdatePostAsync(PostDto postToBeUpdated, int postId);
    Task DeletePostAsync(int postId);
}