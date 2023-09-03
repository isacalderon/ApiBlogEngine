using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

[ApiController]
[Route("api/")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;
    private readonly IPostStatusService _postStatusService;

    public StatusController(ILogger<StatusController> logger, IPostStatusService postStatusService)
    {
        _logger = logger;
        _postStatusService = postStatusService;
    }

    [HttpPost("status", Name = "SubmmitedPost")]
    [Authorize]
    public IActionResult SubmmitedPost(StatusDto request)
    {
        _logger.LogInformation("StatusController.Edit");
        // Deberia enviar el Usuario, si es escritor puede hacer el subbmited de su post 
         var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        var postStatus = _postStatusService.UpdatePostStatus(request, userEmail??string.Empty);
        return Ok(postStatus);
    }
    
}