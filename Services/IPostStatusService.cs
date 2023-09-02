using ApiBlogEngine.Repository;

public interface IPostStatusService{

    PostStatus CreatePostStatus(int postId);

    PostStatusService.PostStatusEnum GetPostStatus(int postId); 

    IEnumerable<int> GetPostsByStatus(int status);

    PostStatus? UpdatePostStatus(int status, int postId, string commentEditor);

}