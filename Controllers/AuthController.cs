using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Todo_API.Models.Dtos;
using Todo_API.Models.Utils;

namespace Todo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<IdentityUser> userManager, 
        ILogger<AuthController> logger,
        IOptions<AppJwtInfo> jwOptions,
        SignInManager<IdentityUser> signInManager) : ControllerBase
    {
        [HttpPost("/Register")]
        public async Task<ActionResult<bool>> Register(RegisterDto dto)
        {
            var identityUser = new IdentityUser()
            {
                Email = dto.Email,
                UserName = dto.Email
            };
            var result = await userManager.CreateAsync(identityUser, dto.Password);

            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                // Todo: send token by email
                return true;
            }
            else return false;
        }
        
        [HttpPost("/Login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            /*var result = await signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                true,
                false
            );
            return result.Succeeded;*/

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user is not null && await userManager.CheckPasswordAsync(user, dto.Password))
            {
                logger.LogInformation(jwOptions.Value.ToString());
                var claims = await userManager.GetClaimsAsync(user);
                var expiresAt = DateTime.UtcNow.AddDays(5);
                var securityToken = new JwtSecurityToken(
                    issuer: jwOptions.Value.Issuer,
                    audience: jwOptions.Value.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiresAt,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwOptions.Value.Secret)),
                        SecurityAlgorithms.HmacSha256
                        )
                    );
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                return Ok(new
                {
                    token = jwtSecurityTokenHandler.WriteToken(securityToken),
                    expiresAt
                });
            }
            else return Unauthorized();
        }
    }
}
