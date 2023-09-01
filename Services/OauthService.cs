using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ApiBlogEngine.Repository;

namespace ApiBlogEngine.Services;
public class OauthService : IOauthService{

    private readonly ILogger<OauthService> _logger;

    private readonly BlogEngineContext _context;

    private readonly IConfiguration _configuration;

    public OauthService(ILogger<OauthService> logger, BlogEngineContext context, IConfiguration configuration){
        _logger = logger;
        _context = context;
        _configuration = configuration;
    }
    public OauthResponse GetToken(string email, string password){
       
        _logger.LogInformation("GetToken");
        //TODO: buscar el usuario 
        User user = _context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
        if(user == null){
            OauthResponse oauthResponse = new OauthResponse();
            oauthResponse.message = "Invalid user name or password";
            oauthResponse.code = "401"; 
            return oauthResponse;
        }
  
       return GenerateToken(user);
    }

    private OauthResponse GenerateToken(User user ){
        OauthResponse oauthResponse = new OauthResponse();

        if(user.Email == null || user.Password == null){
            oauthResponse.message = "Invalid user name or password";
            oauthResponse.code = "401"; 
            return oauthResponse;
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
            );
        var subject = new ClaimsIdentity(new Claim[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

        var expires = DateTime.UtcNow.AddMinutes(10);
        oauthResponse.expires_in = expires; 

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            oauthResponse.access_token = jwtToken;
            oauthResponse.code = "200";
            oauthResponse.message = "Success";

            return oauthResponse; 
    }
}