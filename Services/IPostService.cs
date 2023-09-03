using ApiBlogEngine.Dto;
public interface IPostService
{
    IEnumerable<PostDto> GetAllPostsAsyn();
    PostDto GetPostById(int id);
    IEnumerable<PostDto> GetPostsByAuthor(string? email);
    IEnumerable<PostDto> GetPendingPosts(string? email);
    PostDto CreatePostAsync(string? email, PostDto? newPost);
    UpdatePostResponse UpdatePostAsync(PostDto? postToBeUpdated, string? email);
}