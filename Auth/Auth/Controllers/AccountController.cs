using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityService _identityService;

        public AccountsController(
            AppDbContext ctx,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IdentityService identityService,
            SignInManager<IdentityUser> signInManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var identity = new IdentityUser
            {
                Email = registerUser.Email,
                UserName = registerUser.Email
            };

            var createdIdentity = await _userManager.CreateAsync(identity, registerUser.Password);
            if (!createdIdentity.Succeeded)
                return BadRequest(createdIdentity.Errors);

            var newClaims = new List<Claim>
            {
                new("FirstName", registerUser.FirstName),
                new("LastName", registerUser.LastName)
            };
            await _userManager.AddClaimsAsync(identity, newClaims);

            var roleName = registerUser.Role.ToString();
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(identity, roleName);
            newClaims.Add(new Claim(ClaimTypes.Role, roleName));

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email ?? throw new InvalidOperationException()),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email ?? throw new InvalidOperationException())
            });

            claimsIdentity.AddClaims(newClaims);

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthenticationResult(_identityService.WriteToken(token));

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
                return BadRequest("Invalid credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return BadRequest("Couldn't sign in.");

            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? throw new InvalidOperationException()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException())
            });

            claimsIdentity.AddClaims(claims);
            foreach (var role in roles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            var response = new AuthenticationResult(_identityService.WriteToken(token));

            return Ok(response);
        }
    }

    public enum Role
    {
        Administrator,
        User
    }

    public record RegisterUser(string Email, string Password, string FirstName, string LastName, Role Role);
    public record LoginUser(string Email, string Password);
    public record AuthenticationResult(string Token);
}