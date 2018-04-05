using WEbAPiIdentity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductStore.Models
{
    public class SubCategoryRepository : ISubCategoryRepository, IDisposable
    {
        ApplicationDbContext context;
       

        public SubCategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<SubCategoryViewModel> GetAll()
        {
            var data = context.SubCategory.ToList();
            var query = from p in data select new SubCategoryViewModel { SubCategoryName = p.SubCategoryName,  addedby = p.Id };
            return query;
        }
        public IEnumerable<SubCategoryViewModel> GetSubCategoryByCategory(int id)
        {
            var data = context.SubCategory.ToList();
            var query = from p in data where p.CategoryId == id select new SubCategoryViewModel { SubCategoryName = p.SubCategoryName, addedby = p.Id, };
            return query;
        }
        public SubCategory Get(int id)
        {
        
            return context.SubCategory.Find(id);


        }
        
        public SubCategory Add(SubCategory item)
        {
           
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var data = context.SubCategory.ToList();
            var query = from c in data where c.SubCategoryName == item.SubCategoryName select c;
            if (query.Count() == 0)
            {
                context.SubCategory.Add(item);
                
            }
            else
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "Category NAME ALREADY EXIST",
                };
                throw new HttpResponseException(message);
            }
           
            return item ;
        }

        public void Remove(int id)
        {

            SubCategory product = context.SubCategory.Find(id);
            context.SubCategory.Remove(product);
        }

        public bool Update(SubCategory item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }
        public SubCategory PatchUpdate(SubCategory item)
        {

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return new SubCategory()
            {
                SubCategoryName = item.SubCategoryName
            };

        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
                }
                disposedValue = true;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ProductRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        //IEnumerable<Category> ICategoryRepository.GetAll()
        //{
        //    throw new NotImplementedException();
        //}

      
        #endregion
    }
}