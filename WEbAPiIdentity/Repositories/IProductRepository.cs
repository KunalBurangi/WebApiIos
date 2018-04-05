using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEbAPiIdentity.Models
{
    public interface IProductRepository
    {
        IEnumerable<Products> GetAll();
        IEnumerable<ProductsViewModel> GetProductsBySubCategory(int id);
        Products Get(int id);
        Products Add(Products item);
        void Remove(int id);
        bool Update(Products item);
        Products PatchUpdate(Products item);
        void Save();
    }
}
