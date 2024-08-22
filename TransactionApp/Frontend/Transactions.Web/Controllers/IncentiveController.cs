using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Transactions.Web.Dtos;
using Transactions.Web.Dtos.Data;
using Transactions.Web.Dtos.Requests;
using Transactions.Web.Models;
using Transactions.Web.Services.Incentives;
using Transactions.Web.Utils;

namespace Transactions.Web.Controllers;

public class IncentiveController : Controller
{
    private readonly IIncentiveService _incentiveService;

    public IncentiveController(IIncentiveService incentiveService)
    {
        _incentiveService = incentiveService;
    }

    public async Task<IActionResult> GetAll(FilterIncentiveDto filter)
    {
        List<IncentiveDto>? incentiveDtos = new();

        ResponseDto<List<IncentiveDto>> response = await _incentiveService.GetAllAsync(filter);

        if (response is not null && response.IsSuccess)
        {
            incentiveDtos = response.Result ?? new List<IncentiveDto>();
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
		}

		GetAllViewModel viewModel = new GetAllViewModel() { Filter = new(), Incentives = incentiveDtos };
		ViewBag.TransportComparators = ComparatorDropDownList.PopulateComparatorDropDownList<TransportComparatorDropDownList>();
		ViewBag.KilometerComparators = ComparatorDropDownList.PopulateComparatorDropDownList<KilometerComparatorDropDownList>();
		ViewBag.BonusComparators = ComparatorDropDownList.PopulateComparatorDropDownList<BonusComparatorDropDownList>();

		return View("Incentives", viewModel);
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
            TempData["success"] = "Incentive created successfully";
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
    public async Task<IActionResult> DeleteMany(DeleteIncentiveDto incentive)
    {
        if (!ModelState.IsValid) return View(incentive);

        ResponseDto<bool>? response = await _incentiveService.DeleteManyAsync(incentive);

        if (response is not null && response.IsSuccess)
        {
            return RedirectToAction(nameof(GetAll));
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
            return View(incentive);
        }
	}
}
