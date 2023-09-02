using ApiBlogEngine.Dto;
public interface IPostService
{
    IEnumerable<PostDto> GetAllPostsAsyn();
    IEnumerable<PostDto> GetPostsByAuthor(string email);
    IEnumerable<PostDto> GetPostByStatus(Enum status);
    PostDto GetPostsById(int id);
    PostDto CreatePostAsync(string email, PostDto newPost);
    Boolean UpdatePostAsync(PostDto postToBeUpdated, string email);
    Task DeletePostAsync(int postId);
}