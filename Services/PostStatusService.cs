using ApiBlogEngine.Repository;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

public class PostStatusService : IPostStatusService
{
    private readonly ILogger<PostStatusService> _logger;
    private readonly BlogEngineContext _context;
    public enum PostStatusEnum
    {
        Draft = 1,
        Submitted = 2,
        Pending = 3,
        Rejected = 4,
        ApprovedPublished = 5,
        NotFound = 404
    }

    public PostStatusService(ILogger<PostStatusService> logger, BlogEngineContext context){
        _logger = logger;
        _context = context;
    }

    public PostStatus CreatePostStatus(int postId)
    {
        try{
        _logger.LogInformation("CreatePostStatus");
        var postStatus = new PostStatus();
        postStatus.Post = postId;
        postStatus.Status = (int)PostStatusEnum.Draft;
        postStatus.CreatedAt = DateTime.Now;
        postStatus.Locked = 0;
        postStatus.CommentEditor = string.Empty;
        _context.PostStatuses.Add(postStatus);
        _context.SaveChanges();
        return postStatus;
        }catch(Exception e){
            throw new Exception(e.Message);
        }
    }

    public PostStatus GetPostStatus(int postId)
    {
        _logger.LogInformation("GetPostStatus");
        var result = _context.PostStatuses.Where(p => p.Post == postId).FirstOrDefault();
        return result != null ? result : throw new Exception("Record not found");
    }

    public IEnumerable<int> GetPostsByStatus(int status)
    {
        _logger.LogInformation("GetPostsByStatus");
        return _context.PostStatuses.Where(p => p.Status == status).Select(p => p.Post);
    }

    public Boolean UpdatePostStatus(StatusDto request, string email)
    {
        _logger.LogInformation("UpdatePostStatus");
        if(request.Status == (int)PostStatusEnum.Submitted){
            _logger.LogInformation("Submitted Status");
            return SubmittedStatus(request, email);
        }
        
        if(request.Status == (int)PostStatusEnum.ApprovedPublished || request.Status == (int)PostStatusEnum.Rejected){
            _logger.LogInformation("approved or Reject Status");
            return ApprovedPublishedStatus(request, email);
        }

       return false; 
    }

    private Boolean SubmittedStatus(StatusDto request, string email){

        var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
        if(user!= null && user.Role == 3){ // Writer
            _logger.LogInformation("The user is the Writer");
            var postStatus = _context.PostStatuses.Where(p => p.Post == request.PostId).FirstOrDefault();
            if (postStatus != null)
            {
                postStatus.Status = request.Status;
                postStatus.CreatedAt = DateTime.Now;
                postStatus.CommentEditor = string.Empty;
                postStatus.Locked = 1;
                _logger.LogInformation($"locked {postStatus.Locked}");
                _logger.LogInformation($"Update the post status {postStatus.Id}");
                _context.PostStatuses.Update(postStatus);
                _context.SaveChanges();
                return true;
            }
        }
        
        return false;
    } 

    private Boolean ApprovedPublishedStatus(StatusDto request, string email){
        var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
        if(user!= null && user.Role == 2){ // Editor
            var postStatus = _context.PostStatuses.Where(p => p.Post == request.PostId).FirstOrDefault();
            if (postStatus != null)
            {
                postStatus.Status = request.Status;
                postStatus.CommentEditor = request.CommentEditor??string.Empty;
                postStatus.CreatedAt = DateTime.Now;
                postStatus.Locked = (request.Status == (int)PostStatusEnum.Rejected )? 0 : 1;
                _context.SaveChanges();
                return true;
            }
        }
        
        return false;
    }

}
