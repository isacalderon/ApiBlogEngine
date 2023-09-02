
using ApiBlogEngine.Repository;

public class PostServices : IPostService
{
    private readonly ILogger<PostsController> _logger;

    private readonly BlogEngineContext _context;

    private readonly IPostStatusService _postStatusService;

    public enum Status
    {
        Draft = 1,

        Submitted = 2,

        Pendind = 3, 

        Rejected = 4,

        Published = 5,

    }

    public enum Role
    {
        Admin = 1,
        Editor =  2,
        Writer = 3, 
        Reader = 4

    }

    public PostServices(ILogger<PostsController> logger, BlogEngineContext context, IPostStatusService postStatusService)
    {
        _logger = logger;
        _context = context;
        _postStatusService = postStatusService;
    }
    public PostDto CreatePostAsync(String email, PostDto newPost)
    {
        _logger.LogInformation("CreatePost");
        // validate the user Rol 
        User user = _context.Users.Where(u => u.Email == email).FirstOrDefault()?? throw new Exception("User not found");
        if( user != null && user.Role != (int)Role.Writer){
            throw new Exception("User is not a writer");
        }
        // create the post
        Post post = new Post();
        post.Title = newPost.Title?? throw new Exception("Title is required");
        post.Content = newPost.Content?? throw new Exception("Content is required");
        post.Author = user.Id;
        post.CreatedAt = DateTime.Now;
        post.UpdatedAt = DateTime.Now;
        _context.Posts.Add(post);
        _context.SaveChanges();

        return newPost;

    }

    public IEnumerable<PostDto> GetAllPostsAsyn()
    {
        // Search all the Published post
       IEnumerable<int> publishedPost = _postStatusService.GetPostsByStatus((int)Status.Published);
       List<Post> listPost = _context.Posts.Where(p => publishedPost.Contains(p.Id)).ToList();
       _logger.LogInformation($"GetPosts: {listPost.Count()}");
       List<PostDto> listPostDto = new List<PostDto>();
       listPost.ForEach(p => listPostDto.Add(new PostDto(){
           Id = p.Id,
           Title = p.Title,
           Content = p.Content}));
     
       _logger.LogInformation($"Final get Post: {listPostDto.Count()}");
       
       return listPostDto;
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

    public Boolean UpdatePostAsync(PostDto postToBeUpdated, string email)
    {
        _logger.LogInformation("UpdatePost");
        // validate the user Rol 
        User user = _context.Users.Where(u => u.Email == email).FirstOrDefault()?? throw new Exception("User not found");
        if( user != null && user.Role != (int)Role.Writer){
            throw new Exception("User is not a writer");
        }
        // create the post
        Post post = _context.Posts.Where(p => p.Id == postToBeUpdated.Id).FirstOrDefault()?? throw new Exception("Post not found");
       
        if(post.Author != user?.Id){
            throw new Exception("User is not the author of the post");
        }

        post.Title = postToBeUpdated.Title?? post.Title;
        post.Content = postToBeUpdated.Content?? post.Content;
        post.UpdatedAt = DateTime.Now;
        _context.Posts.Update(post);
        _context.SaveChanges();
        return true;
    }
}