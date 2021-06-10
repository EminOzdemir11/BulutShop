using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICartService
    {
         void InitializeCart(string userId);
         Cart GetCartByUserId(string userId);
    }
}