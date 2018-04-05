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

    public class SubCategoryController : ApiController
    {
       
        ApplicationDbContext _context;
        private ISubCategoryRepository repository;
    
        public SubCategoryController()
        {
            _context = new ApplicationDbContext();
            this.repository = new SubCategoryRepository(new ApplicationDbContext()); 
        }
        public SubCategoryController(ISubCategoryRepository repository)
        {
            this.repository = repository;
        }
        public IHttpActionResult Get(string sort)
        {
            // Convert data source into IQueryable
            // ApplySort method needs IQueryable data source hence we need to convert it
            // Or we can create ApplySort to work on list itself
            var data = (from prod in _context.SubCategory.
                               OrderBy(a => a.SubCategoryId)
                        select prod).AsQueryable();

            // Apply sorting
            data = data.ApplySort(sort);

            // Return response
            return Ok(data);
        }
        public IEnumerable<SubCategoryViewModel> GetAllSubCategory([FromUri]PagingParameterModel pagingparametermodel)
        {

            // Return List of Customer  
            var source = (from p in _context.SubCategory.
                            OrderBy(a => a.SubCategoryId)
                          select new SubCategoryViewModel { SubCategoryName = p.SubCategoryName, addedby = p.Id }).AsQueryable();

            if (!string.IsNullOrEmpty(pagingparametermodel.QuerySearch))
            {
                source = source.Where(a => a.SubCategoryName.Contains(pagingparametermodel.QuerySearch));
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

        public IHttpActionResult GetSubCategoryByCategory(int id)
        {
            var data = repository.GetSubCategoryByCategory(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        //public SubCategory Get(int id)
        //{
        //    return _context.SubCategory.Find(id);


        //}

        // POST: api/Customer
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostSubCategory(SubCategory item)
        {
          
            try
            {
                var httpRequest = HttpContext.Current.Request;
                item.Id = User.Identity.GetUserId();
                item.SubCategoryName = item.SubCategoryName.ToUpper();
                item = repository.Add(item);
                if (item != null)
                {
                    repository.Save();
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Category name already exist"));
                }
                var response = Request.CreateResponse<SubCategory>(HttpStatusCode.Created, item);

                string uri = Url.Link("DefaultApi", new { id = item.SubCategoryId });
                response.Headers.Location = new Uri(uri);
                return Ok(item);
            }
            catch
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Category Already Exist"));
            }
        }
        [Authorize]
        // PUT: api/Customer/5
        public IHttpActionResult PutSubCategory(int id, SubCategory Subcategory)
        {
           
            if (!repository.Update(Subcategory))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok(Subcategory);
        }
      
        [HttpPatch]
        [Authorize]
        public IHttpActionResult PatchSubCategory(int id , SubCategory Subcategory)
        {
          
            if (!repository.Update(Subcategory))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return Ok("Successful");


        }
        [Authorize]
        public IHttpActionResult DeleteSubCategory(int id)
        {

            SubCategory item = repository.Get(id);
            var data = User.Identity.GetUserId();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            if (item.Id == data)
            {
                var data1 = _context.Products.Where(a => a.SubCategoryId == id);
                if (data1 == null)
                {
                    repository.Remove(id);
                    repository.Save();

                    return Ok();
                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Category is not empty"));

            }

            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "You can not delete data added by someone else"));
            }

       
        }

    }
}
