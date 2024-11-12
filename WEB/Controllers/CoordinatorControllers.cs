using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WEB.Controllers;

[Route("api/admin")]
[Authorize(Roles = "Coordinator")]
public class CoordinatorControllers(ICoordinatorService service) : BaseController
{
    [HttpGet("all")]
    public async Task<IActionResult> GetCoordinatorsAsync()
    {
        var coordinators = await service.GetListCoordinatorsAsync();

        if (coordinators == null || coordinators.Count == 0)
        {
            throw new BadRequestException("Список транспортных координаторов пуст.");
        }

        return Ok(coordinators);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddCoordinatorAsync(string email)
    {
        ValidateEmail(email);
        await service.AddCoordinatorAsync(email);

        return Ok("Добавление координатора прошло успешно.");
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditCoordinatorAsync(string email, string newEmail)
    {
        ValidateEmail(email);
        ValidateEmail(newEmail);

        await service.EditCoordinatorAsync(email, newEmail);

        return Ok("Редактирование координатора прошло успешно.");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteCoordinatorAsync(string email)
    {
        ValidateEmail(email);
        await service.DeleteCoordinatorAsync(email);

        return Ok("Удаление координатора прошло успешно.");
    }
}
