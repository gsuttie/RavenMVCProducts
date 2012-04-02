using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RavenMVCProducts.Controllers;

namespace RavenMVCProducts.ViewModels
{
    public class ProductViewModel
    {
        public bool HasNextPage
        {
            get { return CurrentPage * RavenController.PageSize < ProductsCount; }
        }

        public bool HasPrevPage
        {
            get { return CurrentPage * RavenController.PageSize > RavenController.PageSize * RavenController.DefaultPage; }
        }

        public int CurrentPage { get; set; }
        public int ProductsCount { get; set; }

        public string Id { get; set; }

        public string CategoryId { get; set; }

        public string SupplierId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        public int UnitsOnStock { get; set; }

        public int UnitsOnOrder { get; set; }

        public bool Discontinued { get; set; }

        public string PhotoFile { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}