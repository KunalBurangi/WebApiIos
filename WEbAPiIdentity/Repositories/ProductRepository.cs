using WEbAPiIdentity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProductStore.Models
{
    public class ProductRepository : IProductRepository,IDisposable
    {
        ApplicationDbContext context;
        

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Products> GetAll()
        {
            return context.Products.ToList();
        }

        public Products Get(int id)
        {
            return context.Products.Find(id);
          
         
        }
        public IEnumerable<ProductsViewModel> GetProductsBySubCategory(int id)
        {
            var data = context.Products.ToList();
            var query = from p in data where p.SubCategoryId == id select new ProductsViewModel {PID = p.PID, Name = p.Name, Price=p.Price,description=p.description,addedBy=p.Id};
            return query;
        }

        public Products Add(Products item)
        {
           
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
          
            context.Products.Add(item);
            return item;
        }

        public void Remove(int id)
        {
           

            Products product = context.Products.Find(id);
           
            context.Products.Remove(product);
        }

        public bool Update(Products item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }
        public Products PatchUpdate(Products item)
        {

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return new Products()
            {
                Name = item.Name,
                SubCategoryId = item.SubCategoryId,
                Price = item.Price,
                description = item.description,
                Id = item.Id
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
        #endregion
    }
}