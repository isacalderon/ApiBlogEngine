
using System.Net;
using ApiBlogEngine.Repository;

public class PostServices : IPostService
{
    private readonly ILogger<PostsController> _logger;

    private readonly BlogEngineContext _context;

    private readonly IPostStatusService _postStatusService;

    public enum Status
    {
        Draft = 1,

        SubmittedPending = 2, 

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
    public PostDto CreatePostAsync(String? email, PostDto? newPost)
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

        post = _context.Posts.OrderByDescending(e => e.Id).FirstOrDefault()?? throw new Exception("Post not found");

        _logger.LogInformation($"Post created: {post.Id}");
        // create the post status
        _context.PostStatuses.Add(_postStatusService.CreatePostStatus(post.Id)); 

        return new PostDto(){
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        };

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

    public IEnumerable<PostDto> GetPostsByAuthor(string? email)
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

    public IEnumerable<PostDto> GetPendingPosts(string? email)
    {
        // the user should be a editor 
        User user = _context.Users.Where(u => u.Email == email).FirstOrDefault()?? throw new Exception("User not found");
        if (user.Role != (int)Role.Editor){
            throw new Exception("User is not a editor");
        }
        // Search all the Pending Post 
       IEnumerable<int> pendingPost = _postStatusService.GetPostsByStatus((int)Status.SubmittedPending);
       List<Post> listPost = _context.Posts.Where(p => pendingPost.Contains(p.Id)).ToList();
       _logger.LogInformation($"GetPosts: {listPost.Count()}");
       List<PostDto> listPostDto = new List<PostDto>();
       listPost.ForEach(p => listPostDto.Add(new PostDto(){
           Id = p.Id,
           Title = p.Title,
           Content = p.Content}));
     
       _logger.LogInformation($"Final get Post: {listPostDto.Count()}");
       
       return listPostDto;
    }

    public UpdatePostResponse UpdatePostAsync(PostDto? postToBeUpdated, string? email)
    {
        _logger.LogInformation("UpdatePost");
        // validate the user Rol 
        User user = _context.Users.Where(u => u.Email == email).FirstOrDefault()?? throw new Exception("User not found");
        if( user != null && user.Role != (int)Role.Writer){
            return new UpdatePostResponse(101, "User is not a writer");
        }
        _logger.LogInformation($"User found: {user.Id}");
        // get the post to update
        if (postToBeUpdated == null){
            return new UpdatePostResponse(102, "Post to be updated is null");
        }
        Post post = _context.Posts.Where(p => p.Id == postToBeUpdated.Id).FirstOrDefault()?? throw new Exception("Post not found");
        _logger.LogInformation($"Post found: {post.Id}");
        // get the estatus of the post
        PostStatus postStatus = _postStatusService.GetPostStatus(post.Id);
        if(postStatus.Locked == 1){
            return new UpdatePostResponse(103, "The Post can not be updated because is locked");
        }
        _logger.LogInformation($"Post status found: {postStatus.Status}");
        // validate the user is the author of the post
        if(post.Author != user?.Id){
            return new UpdatePostResponse(104, "User is not the author of the post");
        }

        post.Title = postToBeUpdated.Title?? post.Title;
        post.Content = postToBeUpdated.Content?? post.Content;
        post.UpdatedAt = DateTime.Now;
        try{
            _context.Posts.Update(post);
            _context.SaveChanges();
            _logger.LogInformation($"Post updated: {post.Id}");
             return new UpdatePostResponse(200, "Post updated successfully", postToBeUpdated);
        }catch(Exception e){
            return new UpdatePostResponse(105, e.Message);
        }
       
    }
}