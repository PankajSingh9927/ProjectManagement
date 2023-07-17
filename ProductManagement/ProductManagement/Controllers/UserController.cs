using Applications.Services;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Configration;
using ProductManagement.RequestDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagement.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IDbHelperSevices _dbHelperSevices;
        public IConfiguration _configuration { get; }
        public UserController( IEncryptionService encryptionService, IConfiguration configuration, IDbHelperSevices dbHelperSevices)
        {
            this._encryptionService = encryptionService;
            this._configuration = configuration;
            this._dbHelperSevices = dbHelperSevices;
        }

        [HttpPost]
        [Route("token")]
        public  IActionResult GetUserToken(UserDto user)
        {

            if(string.IsNullOrWhiteSpace(user.UserName))
                return BadRequest("User name required.");

            var userData = _dbHelperSevices.GetCustomer().Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Equals(user.UserName.Trim().ToLower())).FirstOrDefault();
            if (userData == null)
                return BadRequest("User not exist.");

            var pwd = _encryptionService.EncryptText(user.Password, "");

            if (userData?.Password== pwd)
            {
                var jwtOptions = _configuration.GetOptions<JwtSettings>(JwtSettings.SectionName);
                var key = Encoding.ASCII.GetBytes(jwtOptions.Key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", (userData?.Id??0).ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = jwtOptions.Issuer,
                    Audience = jwtOptions.Audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return Ok(stringToken);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddUser(UserDto user)
        {
            try
            {
                var userData = _dbHelperSevices.GetCustomer()?.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Equals(user.UserName.Trim().ToLower()))?.FirstOrDefault();
                if (userData != null)
                    return BadRequest("User alredy exist.");

                var pwd = _encryptionService.EncryptText(user.Password, "");

                var newUser = new Customer()
                {
                    Name = user.UserName,
                    Email = user.Email,
                    Password = pwd,
                    CratedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    Identifier = Guid.NewGuid()
                };
                _dbHelperSevices.AddCustomer(newUser);

                return Ok("User added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex?.Message);
            }
           
        }
    }
}
