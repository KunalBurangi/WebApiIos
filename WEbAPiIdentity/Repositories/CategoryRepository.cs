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
    public class CategoryRepository : ICategoryRepository,IDisposable
    {
        ApplicationDbContext context;
       

        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CategoryViewModel> GetAll()
        {
            var data = context.Category.ToList();
            var query = from p in data select new CategoryViewModel { CategoryName = p.CategoryName,  addedby = p.Id };
            return query;
        }

        public Category Get(int id)
        {
        
            return context.Category.Find(id);


        }
        
        public  Category Add(Category item)
        {
           
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var data = context.Category.ToList();
            var query = from c in data where c.CategoryName == item.CategoryName select c;
            if (query.Count() == 0)
            {
                context.Category.Add(item);
                
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
            
            Category product = context.Category.Find(id);
            context.Category.Remove(product);
        }

        public bool Update(Category item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }
        public Category PatchUpdate(Category item)
        {

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return new Category()
            {
                CategoryName = item.CategoryName
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