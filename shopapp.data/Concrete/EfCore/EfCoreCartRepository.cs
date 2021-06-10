using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public Cart GetByUserId(string userId)
        {
            using(var context = new ShopContext())
            {
                return context.Carts
                        .Include(i=>i.CartItems)
                        .ThenInclude(i=>i.Product)
                        .FirstOrDefault(i=>i.UserId==userId);
            }
        }
    }
}