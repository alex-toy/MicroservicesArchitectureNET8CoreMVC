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

        return View(incentiveDtos);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IncentiveDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto<int>? response = await _incentiveService.CreateAsync(model);

            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Coupon created successfully";
                return RedirectToAction(nameof(GetAll));
            }
            else
            {
                TempData["error"] = response?.ErrorMessage;
            }
        }

        return View(model);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int couponId)
    {
        ResponseDto<bool>? response = await _incentiveService.DeleteAsync(couponId);

        if (response != null && response.IsSuccess)
        {
            return View();
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(IncentiveDto incentiveDto)
    {
        ResponseDto<bool> response = await _incentiveService.DeleteAsync(incentiveDto.IncentiveId);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Coupon deleted successfully";
            return RedirectToAction(nameof(GetAll));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }
        return View(incentiveDto);
    }
}
