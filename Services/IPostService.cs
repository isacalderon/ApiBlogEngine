using ApiBlogEngine.Dto;
public interface IPostService
{
    IEnumerable<PostDto> GetAllPostsAsyn();
    IEnumerable<PostDto> GetPostsByAuthor(string? email);
    IEnumerable<PostDto> GetPendingPosts(string? email);
    PostDto CreatePostAsync(string? email, PostDto? newPost);
    UpdatePostResponse UpdatePostAsync(PostDto? postToBeUpdated, string? email);
}