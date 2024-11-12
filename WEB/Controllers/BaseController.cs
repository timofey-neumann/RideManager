using Microsoft.AspNetCore.Mvc;
using Domain.Exceptions;

namespace WEB.Controllers;

public abstract class BaseController : Controller
{
    protected void ValidateEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new BadRequestException("Поле электронной почты не должно оставаться пустым.");
        }
    }
}
