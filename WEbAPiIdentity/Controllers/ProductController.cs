using WEbAPiIdentity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ProductStore.Models;
using WebApplication_Product.Helpers;
using System.IO;
using WEbAPiIdentity;
using Microsoft.AspNet.Identity;
using System.Web.Http.Cors;

namespace Api1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ProductController : ApiController
    {
       
        ApplicationDbContext _context;
        private IProductRepository repository;
    
        public ProductController()
        {
            _context = new ApplicationDbContext();
            this.repository = new ProductRepository(new ApplicationDbContext()); 
        }
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public IHttpActionResult Get(string sort)
        {
            // Convert data source into IQueryable
            // ApplySort method needs IQueryable data source hence we need to convert it
            // Or we can create ApplySort to work on list itself
            var data = (from prod in _context.Products.
                               OrderBy(a => a.PID)
                        select prod).AsQueryable();

            // Apply sorting
            data = data.ApplySort(sort);

            // Return response
            return Ok(data);
        }
        public IEnumerable<ProductsViewModel> GetAllProducts([FromUri]PagingParameterModel pagingparametermodel)
        {

            // Return List of Customer  
            var source = (from p in _context.Products.
                            OrderBy(a => a.PID)
                          select new ProductsViewModel {PID = p.PID, Name = p.Name, Price = p.Price, description = p.description, addedBy = p.Id }
           ).AsQueryable();

            if (!string.IsNullOrEmpty(pagingparametermodel.QuerySearch))
            {
                source = source.Where(a => a.Name.Contains(pagingparametermodel.QuerySearch));
            }

            // Get's No of Rows Count   
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,
                QuerySearch = string.IsNullOrEmpty(pagingparametermodel.QuerySearch) ?
                      "No Parameter Passed" : pagingparametermodel.QuerySearch
            };

            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return items;

        }
      
        public IHttpActionResult GetProductsBySubCategory(int id)
        {
            var data = repository.GetProductsBySubCategory(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }


        // POST: api/Customer
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostProduct(Products item)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                item.Id = User.Identity.GetUserId();

                var image = _context.Image.ToList().LastOrDefault();
               
               item.imageId = image.Id ;
                item = repository.Add(item);
                repository.Save();
                var response = Request.CreateResponse<Products>(HttpStatusCode.Created, item);

                string uri = Url.Link("DefaultApi", new { id = item.PID });
                response.Headers.Location = new Uri(uri);
                return Ok(item);
            }
            catch
            {
                return InternalServerError();
            }
        }
        [Authorize]
        // PUT: api/Customer/5
        public IHttpActionResult PutProduct(int id, Products product)
        {
            product.Id = User.Identity.GetUserId();
            product.PID = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(product);
        }
        [Authorize]
        [HttpPatch]
        public IHttpActionResult PatchProducts(int id , Products product)
        {
            product.PID = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok("Successful");


        }
        [Authorize]
        public IHttpActionResult DeleteProduct(int id)
        {
           
                Products item = repository.Get(id);
            var data = User.Identity.GetUserId();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (item.Id==data)
            {
                repository.Remove(id);
                repository.Save();
                return Ok();
            }

            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,"You can not delete data added by someone else"));
            }

           
        }
       
    }
}
