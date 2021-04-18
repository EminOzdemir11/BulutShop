using System.Collections.Generic;
using System.Linq;

namespace shopapp.webui.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories = null;

        static CategoryRepository(){

            _categories = new List<Category>{
                new Category {CategoryId = 1,Name = "Telefon", Description = "Telefonlar hakkında"},
                new Category {CategoryId = 2,Name = "Bilgisayar", Description = "Bilgisayarlar hakkında"},
                new Category {CategoryId = 3,Name = "Beyaz Eşya", Description = "Beyaz Eşyalar hakkında"},
                new Category {CategoryId = 4,Name = "Kitap", Description = "Kitaplar hakkında"}
            };
        }
        public static List<Category> Categories{
            get{
                return _categories;
            }

        }
        public static void AddCategory(Category category){
            _categories.Add(category);
        }
        public static Category GetCategoryById(int id){
            return _categories.FirstOrDefault(c=>c.CategoryId==id);
        }
    }
}