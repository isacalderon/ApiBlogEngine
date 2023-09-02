using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/")]
public class PostsController : ControllerBase {
    private readonly ILogger<PostsController> _logger;

    private readonly IPostService _postService;

    public PostsController(ILogger<PostsController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [HttpGet("posts", Name = "GetPosts")]
    [Authorize]
    public IActionResult Get()
    {
        _logger.LogInformation("GetPosts");
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
       _logger.LogInformation(userEmail);
        var posts = _postService.GetAllPostsAsyn();
        _logger.LogInformation($"GetPosts: {posts.Count()}");
        return Ok(posts); 
    }

    [HttpGet("posts/author", Name = "GetPost")]
    [Authorize]
    public IActionResult GetOwnPost()
    {
        _logger.LogInformation("GetPost by Author");
         var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        return Ok(_postService.GetPostsByAuthor(userEmail));
    }

    [HttpPost("posts", Name = "CreatePost")]
    [Authorize]
    public PostDto Create(PostDto post)
    {
        _logger.LogInformation("CreatePost");
         string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        _postService.CreatePostAsync(userEmail, post);
        return post;
    }


    [HttpPut("posts/edit", Name = "UpdatePost")]
    [Authorize]
    public IActionResult Update(PostDto post)
    {
        _logger.LogInformation("UpdatePost");
          string userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value; 
        _postService.UpdatePostAsync(post, userEmail);
        return Ok();
    }

    /*

    [HttpDelete("posts/{id}", Name = "DeletePost")]
    public Posts Delete(int id)
    {
       throw new NotImplementedException();
    }*/
}