
using ApiBlogEngine.Repository;

public class PostServices : IPostService
{
    private readonly ILogger<PostsController> _logger;

    private readonly BlogEngineContext _context;

    public enum Status
    {
        Draft = 0,
        Published = 1,
        Deleted = 2
    }

    public enum Role
    {
        Admin = 1,
        Editor =  2,
        Writer = 3, 
        Reader = 4

    }

    public PostServices(ILogger<PostsController> logger, BlogEngineContext context)
    {
        _logger = logger;
        _context = context;
    }
    public PostDto CreatePostAsync(String email, PostDto newPost)
    {
        _logger.LogInformation("CreatePost");
        // validate the user Rol 
        User user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
        if( user != null && user.Role != (int)Role.Writer){
            throw new Exception("User is not a writer");
        }
        // create the post
        Post post = new Post();
        post.Title = newPost.Title;
        post.Content = newPost.Content;
        post.Author = user.Id;
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

    public IEnumerable<PostDto> GetPostsByAuthor(string email)
    {
         var userId = _context.Users.Where(u => u.Email == email).FirstOrDefault()?.Id;
         List<Post> results= _context.Posts.Where(p => p.Author == userId).ToList();
         IEnumerable<PostDto> response = results.Select(p => new PostDto()
         {
             Title = p.Title,
             Content = p.Content
         }).ToList();
         return response;

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