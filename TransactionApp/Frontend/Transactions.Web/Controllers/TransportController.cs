using Microsoft.AspNetCore.Mvc;
using Transactions.Core.Dtos;
using Transactions.Web.Dtos.Transports;
using Transactions.Web.Models;
using Transactions.Web.Services.Transports;
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

        GetAllTransportsVM viewModel = new GetAllTransportsVM() { Filter = new(), Transports = transportDtos };
        ViewBag.PriceComparators = ComparatorDropDownList.PopulateComparatorDropDownList();

        return View("Transports", viewModel);
    }

    public async Task<IActionResult> Create()
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

    public async Task<IActionResult> Delete(int incentiveId)
    {
        ResponseDto<bool>? response = await _transportService.DeleteAsync(incentiveId);

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
