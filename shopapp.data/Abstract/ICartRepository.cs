using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICartRepository: IRepository<Cart>
    {
         Cart GetByUserId(string userId);
    }
}