using System.Net;
using ApiBlogEngine.Repository;

public class UpdatePostResponse
{
    public int Code { get; set; }
    public string? Message { get; set; }

    public PostDto? Post { get; set; }

    public UpdatePostResponse(int code, string? message)
    {
        Code = code;
        Message = message;
    }

    public UpdatePostResponse(int code, string? message, PostDto? post)
    {
        Code = code;
        Message = message;
        Post = post;
    }

}