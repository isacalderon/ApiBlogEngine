public class OauthResponse{
     public string code { get; set; }=default!;
    public string? message { get; set; }
    public string? access_token { get; set; }
    public DateTime expires_in { get; set; }
}