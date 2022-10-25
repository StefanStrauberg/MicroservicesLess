using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        readonly IDiscountRepository _repository;
        public DiscountController(IDiscountRepository repository)
            => _repository = repository ?? throw new ArgumentException(nameof(repository));

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscount(string productName)
            => Ok(await _repository.GetDiscount(productName));

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCoupon([FromBody] Coupon coupon)
        {
            var result = await _repository.UpdateDiscount(coupon);
            if (result == false)
                return BadRequest($"Coupon with ProductName: {coupon.ProductName}, not found.");
            return Ok(result);
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            var result = await _repository.DeleteDiscount(productName);
            if (result == false)
                return BadRequest($"Coupon with ProductName: {productName}, not found.");
            return Ok(result);
        }
    }
}
