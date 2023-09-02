using ApiBlogEngine.Dto;
public interface IPostService
{
    IEnumerable<PostDto> GetAllPostsAsyn();
    IEnumerable<PostDto> GetPostsByAuthor(string email);
    PostDto CreatePostAsync(string email, PostDto newPost);
    Boolean UpdatePostAsync(PostDto postToBeUpdated, string email);
}