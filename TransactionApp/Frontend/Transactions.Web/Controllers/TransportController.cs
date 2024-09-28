using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Core.Dtos.Transports;
using Transactions.Core.Services.Transports;
using Transactions.Web.Models;
using Transactions.Web.Utils;

namespace Transactions.Web.Controllers;

public class TransportController : Controller
{
    private readonly ITransportsService _transportService;

    public TransportController(ITransportsService service)
    {
        _transportService = service;
    }

    public async Task<IActionResult> GetAll(FilterTransportDto filter)
    {
        List<TransportDto>? transportDtos = new();

        ResponseDto<List<TransportDto>> response = await _transportService.GetAllAsync(filter);

        if (response is not null && response.IsSuccess)
        {
            transportDtos = response.Result ?? new List<TransportDto>();
        }
        else
        {
            TempData["error"] = response?.ErrorMessage;
        }

        GetAllTransportsVM viewModel = new () { Filter = new(), Transports = transportDtos };
        ViewBag.PriceComparators = ComparatorDropDownList.PopulateComparatorDropDownList();
        ViewBag.DistanceComparators = ComparatorDropDownList.PopulateComparatorDropDownList();

		return View("Transports", viewModel);
    }

    public IActionResult Create()
    {
        return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(TransportDto TransportDto)
	{
		if (!ModelState.IsValid) return View(TransportDto);

		ResponseDto<int>? response = await _transportService.CreateAsync(TransportDto);

		if (response is not null && response.IsSuccess)
		{
			TempData["success"] = "Transport created successfully";
			return RedirectToAction(nameof(GetAll));
		}
		else
		{
			TempData["error"] = response?.ErrorMessage;
			return View(TransportDto);
		}
    }

    public async Task<IActionResult> Update(int transportId)
	{
		ResponseDto<TransportDto> response = await _transportService.GetByIdAsync(transportId);
		return View(response.Result);
    }

    [HttpPost]
	public async Task<IActionResult> Update(int transportId, TransportDto transportDto)
	{
        transportDto.TransportId = transportId;

        if (!ModelState.IsValid) return View(transportDto);

		ResponseDto<int>? response = await _transportService.UpdateAsync(transportDto);

		if (response.IsSuccess)
		{
			TempData["success"] = "Transport updated successfully";
			return RedirectToAction(nameof(GetAll));
		}
		else
		{
			TempData["error"] = response?.ErrorMessage;
			return View(transportDto);
		}
	}

	public async Task<IActionResult> Delete(int transportId)
    {
        ResponseDto<bool>? response = await _transportService.DeleteAsync(transportId);

        if (response.IsSuccess)
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
    public async Task<IActionResult> DeleteMany(DeleteTransportDto incentive)
    {
        if (!ModelState.IsValid) return View(incentive);

        ResponseDto<bool>? response = await _transportService.DeleteManyAsync(incentive);

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
