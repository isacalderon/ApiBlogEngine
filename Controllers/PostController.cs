using Microsoft.AspNetCore.Mvc;
using ApiBlogEngine.Repository;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/")]
public class PostsController : ControllerBase {
    private readonly ILogger<PostsController> _logger;

    private readonly BlogEngineContext _context;

    private readonly IPostService _postService;

    public PostsController(ILogger<PostsController> logger, BlogEngineContext context, IPostService postService)
    {
        _logger = logger;
        _context = context;
        _postService = postService;
    }

    [HttpGet("posts", Name = "GetPosts")]
    [Authorize]
    public IActionResult Get()
    {
        _logger.LogInformation("GetPosts");
        var posts = _postService.GetAllPostsAsyn();
        _logger.LogInformation($"GetPosts: {posts.Count()}");
        return Ok(posts); 
    }

    [HttpGet("posts/{id}", Name = "GetPost")]
    [Authorize]
    public IActionResult Get(int id)
    {
       throw new NotImplementedException();
    }

    [HttpPost("posts", Name = "CreatePost")]
    [Authorize]
    public PostDto Create(PostDto post)
    {
        _logger.LogInformation("CreatePost");
        _postService.CreatePostAsync("", post);
        return post;
    }

/*
    [HttpPut("posts/{id}", Name = "UpdatePost")]
    public Posts Update(int id, Posts post)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("posts/{id}", Name = "DeletePost")]
    public Posts Delete(int id)
    {
       throw new NotImplementedException();
    }*/
}