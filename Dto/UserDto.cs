public class UserDto{
    public string email { get; set; }

    public string password { get; set; }

    public UserDto(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}