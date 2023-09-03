using ApiBlogEngine.Repository;

public interface IPostStatusService{

    PostStatus CreatePostStatus(int postId);

    PostStatus GetPostStatus(int postId); 

    IEnumerable<int> GetPostsByStatus(int status);

    Boolean UpdatePostStatus(StatusDto request, string email);

}