using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEbAPiIdentity.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<CategoryViewModel> GetAll();

        Category Get(int id);
        Category Add(Category item);
        void Remove(int id);
        bool Update(Category item);
        Category PatchUpdate(Category item);
        void Save();
    }
}
