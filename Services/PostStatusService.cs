using ApiBlogEngine.Repository;
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
        _logger.LogInformation("CreatePostStatus");
        var postStatus = new PostStatus();
        postStatus.Post = postId;
        postStatus.Status = (int)PostStatusEnum.Draft;
        postStatus.CreatedAt = DateTime.Now;
        postStatus.Locked = 0;
        _context.PostStatuses.Add(postStatus);
        _context.SaveChanges();
        return postStatus;
    }

    public PostStatusEnum GetPostStatus(int postId)
    {
        _logger.LogInformation("GetPostStatus");
        var result = _context.PostStatuses.Where(p => p.Post == postId).FirstOrDefault();
        return result != null ? (PostStatusEnum)result.Status : PostStatusEnum.NotFound;
    }

    public IEnumerable<int> GetPostsByStatus(int status)
    {
        _logger.LogInformation("GetPostsByStatus");
        return _context.PostStatuses.Where(p => p.Status == status).Select(p => p.Post);
    }

    public PostStatus? UpdatePostStatus(int status, int postId, string commentEditor)
    {
        _logger.LogInformation("UpdatePostStatus");
        var postStatus = _context.PostStatuses.Where(p => p.Post == postId).FirstOrDefault();
        if (postStatus != null)
        {
            postStatus.Status = status;
            postStatus.CommentEditor = commentEditor;
            postStatus.CreatedAt = DateTime.Now;
            if(status == (int)PostStatusEnum.Submitted || status == (int)PostStatusEnum.ApprovedPublished)
                postStatus.Locked = 1;
            else if(status == (int)PostStatusEnum.Rejected)
                postStatus.Locked = 0;
            _context.SaveChanges();
             return postStatus;
        }
       return null; 
    }
}
