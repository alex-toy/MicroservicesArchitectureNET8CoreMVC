using Microsoft.AspNetCore.Mvc;
using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Data;
using Transactions.Web.Services.Incentives;

namespace Transactions.Web.Controllers;

public class IncentiveController : Controller
{
    private readonly IIncentiveService _incentiveService;

    public IncentiveController(IIncentiveService incentiveService)
    {
        _incentiveService = incentiveService;
    }

    public async Task<IActionResult> GetAll()
    {
        List<IncentiveDto>? incentiveDtos = new();

        ResponseDto<List<IncentiveDto>> response = await _incentiveService.GetAllAsync();

        if (response is not null && response.IsSuccess)
        {
            incentiveDtos = response.Result;
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        return View("Incentives", incentiveDtos);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IncentiveDto incentiveDto)
    {
        if (!ModelState.IsValid) return View(incentiveDto);

        ResponseDto<int>? response = await _incentiveService.CreateAsync(incentiveDto);

        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "Coupon created successfully";
            return RedirectToAction(nameof(GetAll));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
            return View(incentiveDto);
        }
    }

    public async Task<IActionResult> Delete(int incentiveId)
    {
        ResponseDto<bool>? response = await _incentiveService.DeleteAsync(incentiveId);

        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction("GetAll");
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        return NotFound();
    }

    public async Task<IActionResult> DeleteMany()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMany(IncentiveDto incentive)
    {
        if (!ModelState.IsValid) return View(incentive);

        ResponseDto<int>? response = await _incentiveService.CreateAsync(incentive);

        if (response is not null && response.IsSuccess)
        {
            TempData["success"] = "Coupon created successfully";
            return RedirectToAction(nameof(GetAll));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
            return View(incentive);
        }
    }
}
