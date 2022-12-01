using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            _catalogService = catalogService ?? throw new ArgumentException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentException(nameof(orderService));
        }

        [HttpGet]
        public async Task<IActionResult> GetShopping(string userName)
        {
            var basket = await _basketService.GetBasket(userName);
            foreach (var item in basket.Items)
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            var orders = await _orderService.GetOrdersByUserName(userName);
            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
            return Ok(shoppingModel);
        }
    }
}
