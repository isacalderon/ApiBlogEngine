using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/")]
public class CommentController:ControllerBase {

    private readonly ILogger<PostsController> _logger;
    private readonly ICommentService _commentService;

    public CommentController(ILogger<PostsController> logger, ICommentService commentService)
    {
        _logger = logger;
        _commentService = commentService;
    }

    [HttpPost("posts/comments")]
    [Authorize]
    public ActionResult<CommentDto> CreateComment(CommentDto comment)
    {
        string? authorEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (authorEmail == null)
        {
            return Unauthorized();
        }
        return Ok(_commentService.CreateCommentAsync(authorEmail, comment));
    }

    [HttpGet("posts/{postId}/comments")]
    [Authorize]
    public ActionResult<List<CommentDto>> GetComments(int postId)
    {
        return Ok(_commentService.GetComments(postId));
    }



}