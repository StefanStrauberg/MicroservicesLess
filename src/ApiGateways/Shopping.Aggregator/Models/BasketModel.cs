using System.Collections.Generic;

namespace Shopping.Aggregator.Models
{
    public class BasketModel
    {
        public string UserName { get; set; }
        public List<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();
        public decimal TotalPrice { get; set; }
    }
}
