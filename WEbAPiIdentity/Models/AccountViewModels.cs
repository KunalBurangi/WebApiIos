using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEbAPiIdentity.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }
        public string UserId { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
    public class Products
    {
        [Key]
        public int PID { get; set; }
        public string Name { get; set; }
     
        public decimal Price { get; set; }
        public string description { get; set; }
        public int imageId { get; set; }
        // Foreign key 
        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategories { get; set; }
        // Foreign key 
        [Display(Name = "addedby")]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser Users { get; set; }
    }
    public class ProductsViewModel
    {
      
        public int PID { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string description { get; set; }
        public string addedBy { get; set; }

    }
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Display(Name = "addedby")]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser Users { get; set; }
    }
    public class SubCategory
    {
        [Key]
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        [Display(Name = "addedby")]
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser Users { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Categories { get; set; }
    }

    public class CategoryViewModel
    {
      
        public string CategoryName { get; set; }
        public string addedby { get; set; }
       
    }
    public class SubCategoryViewModel
    {

        public string SubCategoryName { get; set; }
        public string addedby { get; set; }

    }
    public class ForgotViewModel
    {
        public string Email { get; set; }
    }
    public class PagingParameterModel
    {
        const int maxPageSize = 20;

        public int pageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 5;

        public int pageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string QuerySearch { get; set; }
    }
}

