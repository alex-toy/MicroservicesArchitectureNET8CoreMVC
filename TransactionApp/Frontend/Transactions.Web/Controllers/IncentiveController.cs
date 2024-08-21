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

            if (response != null && response.IsSuccess)
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

    //public async Task<IActionResult> Delete(int couponId)
    //{
    //    ResponseDto? response = await _incentiveService.GetByIdAsync(couponId);

    //    if (response != null && response.IsSuccess)
    //    {
    //        CouponDto? model = JsonConvert.DeserializeObject<IncentiveDto>(Convert.ToString(response.Result));
    //        return View(model);
    //    }
    //    else
    //    {
    //        TempData["error"] = response?.Message;
    //    }
    //    return NotFound();
    //}

    //[HttpPost]
    //public async Task<IActionResult> CouponDelete(CouponDto couponDto)
    //{
    //    ResponseDto? response = await _incentiveService.DeleteCouponsAsync(couponDto.CouponId);

    //    if (response != null && response.IsSuccess)
    //    {
    //        TempData["success"] = "Coupon deleted successfully";
    //        return RedirectToAction(nameof(CouponIndex));
    //    }
    //    else
    //    {
    //        TempData["error"] = response?.Message;
    //    }
    //    return View(couponDto);
    //}
}
