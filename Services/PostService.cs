
using ApiBlogEngine.Repository;

public class PostServices : IPostService
{
    private readonly ILogger<PostsController> _logger;

    private readonly BlogEngineContext _context;

    public PostServices(ILogger<PostsController> logger, BlogEngineContext context)
    {
        _logger = logger;
        _context = context;
    }
    public PostDto CreatePostAsync(String email, PostDto newPost)
    {
        Post post = new Post();
        post.Title = newPost.Title;
        post.Content = newPost.Content;
        post.Author = 1;
        post.CreatedAt = DateTime.Now;
        post.UpdatedAt = DateTime.Now;
        _context.Posts.Add(post);
        _context.SaveChanges();

        return newPost;

    }

    public Task DeletePostAsync(int postId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PostDto> GetAllPostsAsyn()
    {
       IEnumerable<Post> listPost = _context.Posts.ToList(); 
       List<PostDto> listPostDto = new List<PostDto>();
       _logger.LogInformation($"GetPosts: {listPost.Count()}");
       
       foreach(Post post in listPost){
           PostDto postDto = new PostDto();
           postDto.Title = post.Title;
           postDto.Content = post.Content;
           listPostDto.Add(postDto);
       }
       _logger.LogInformation($"Final get Post: {listPostDto.Count()}");
       
       return listPostDto;
    }

    public IEnumerable<PostDto> GetPostByStatus(Enum status)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PostDto> GetPostsByAuthor(int authorId)
    {
        throw new NotImplementedException();
    }

    public PostDto GetPostsById(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostAsync(PostDto postToBeUpdated, int postId)
    {
        throw new NotImplementedException();
    }
}