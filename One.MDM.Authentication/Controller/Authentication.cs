using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using One.MDM.Authentication.Model;
using One.MDM.Authentication.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace One.MDM.Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Tên đăng nhập hoặc mật khẩu không được để trống");
            }

            var result = await _authService.Login(request.Username, request.Password);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Login error: {ex.Message}");
        }
    }


    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser([FromBody] LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username và Password không được để trống");
            }
            var result = await _authService.CreateUser(request.Username, request.Password);

            if (result.Equals("Success"))
            {
                return Ok("Tạo thành công!");
            }
            return BadRequest($"{result}");
        }
        catch (Exception ex)
        {
            return BadRequest($"CreateUser {ex.Message}");
        }
    }
    [HttpGet("users")]
    public IActionResult GetUsers(int page = 0, int size = 20)
    {
        try
        {
            var result = _authService.GetUser(page, size);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"GetUsers {ex.Message}");
        }
    }

    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        try
        {
            var result = _authService.GetRoles();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"GetRoles {ex.Message}");
        }
    }
    [HttpGet("entities")]
    public IActionResult GetEntities()
    {
        try
        {
            var result = _authService.GetEntities();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"GetRoles {ex.Message}");
        }
    }
    [HttpPost("createEntity")]
    public async Task<IActionResult> CreateEntity([FromBody] CreateEntity request)
    {
        try
        {
            if(request == null)
                return BadRequest($"Vui lòng điền data");
            var result = await _authService.CreateEntity(request);
            if (result.Equals("Success"))
            {
                return Ok("Tạo thành công!");
            }
            return BadRequest($"{result}");
        }
        catch (Exception ex)
        {
            return BadRequest($"GetRoles {ex.Message}");
        }
    }
    [HttpPost("createRolePrivilege")]
    public async Task<IActionResult> CreateRolePrivilege([FromBody] RolePrivilegeRequest request)
    {
        try
        {
            var result = await _authService.CreateRolePrivilege(request);

            if (result.Equals("Success"))
            {
                return Ok("Tạo thành công!");
            }
            return BadRequest($"{result}");
        }
        catch (Exception ex)
        {
            return BadRequest($"CreateUser {ex.Message}");
        }
    }
    [HttpGet("userRoles/{userId}")]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        try
        {
            var result = await _authService.GetUserRoles(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"GetUserRoles {ex.Message}");
        }
    }
    [HttpGet("privilege/{roleId}")]
    public async Task<IActionResult> GetRolePrivileges(Guid roleId)
    {
        try
        {
            var result = await _authService.GetRolePrivileges(roleId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"GetRolePrivileges {ex.Message}");
        }
    }
}

