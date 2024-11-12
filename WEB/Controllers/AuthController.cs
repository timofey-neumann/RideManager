using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers;

[Route("api/auth")]
public class AuthController(IAuthService service) : BaseController
{
    [HttpGet("login")]
    public async Task<IActionResult> AuthorizationLoginAsync(string email)
    {
        ValidateEmail(email);

        var result = await service.GenerateTokenByEmailAsync(email);

        if (result != null)
        {
            var (token, role) = result.Value;
            return Ok(new { token, role });
        }
        else
        {
            throw new BadRequestException("Пассажира с такой электронной почтой не существует.");
        }
    }
}
