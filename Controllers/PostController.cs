using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Net;

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
        try{
        _logger.LogInformation("GetPosts");
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
       _logger.LogInformation(userEmail);
        var posts = _postService.GetAllPostsAsyn();
        _logger.LogInformation($"GetPosts: {posts.Count()}");
        return Ok(posts); 
        }catch(Exception e){
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    [HttpGet("posts/author", Name = "GetPost")]
    [Authorize]
    public IActionResult GetOwnPost()
    {
        try{
            _logger.LogInformation("GetPost by Author");
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(_postService.GetPostsByAuthor(userEmail));
        }catch(Exception e){
            return Unauthorized(e.Message);
        }
    }

    [HttpGet("posts/pending", Name = "GetPendingPost")]
    [Authorize]
    public IActionResult GetPendingPost()
    {
        _logger.LogInformation("GetPendingPost");
         var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
         try{
            return Ok(_postService.GetPendingPosts(userEmail));
        }catch(Exception e){
            return Unauthorized(e.Message);
        }
        
    }

    [HttpPost("posts", Name = "CreatePost")]
    [Authorize]
    public IActionResult Create(PostDto post)
    {
        try{
            _logger.LogInformation("CreatePost");
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(_postService.CreatePostAsync(userEmail, post));
        }catch(Exception e){
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }


    [HttpPut("posts/edit", Name = "UpdatePost")]
    [Authorize]
    public IActionResult Update(PostDto post)
    {
        try{
            _logger.LogInformation("UpdatePost");
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            UpdatePostResponse response = _postService.UpdatePostAsync(post, userEmail);
            if(response.Code != 200){
                return StatusCode((int)HttpStatusCode.Unauthorized, response);
            }
            _logger.LogInformation("UpdatePost: OK");
            
          return Ok();
            
        }catch(Exception e){
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message); 
        }
    }
}