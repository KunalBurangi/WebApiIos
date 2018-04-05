using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEbAPiIdentity.Models
{
    public interface ISubCategoryRepository
    {
        IEnumerable<SubCategoryViewModel> GetAll();
        IEnumerable<SubCategoryViewModel> GetSubCategoryByCategory(int id);
        SubCategory Get(int id);
        SubCategory Add(SubCategory item);
        void Remove(int id);
        bool Update(SubCategory item);
        SubCategory PatchUpdate(SubCategory item);
        void Save();
    }
}
