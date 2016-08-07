using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    public class CategoriesController : ApiController
    {

        Category[] categories = new Category[]
        {
            new Category { Id= 1, Name = "Groceries" },
            new Category { Id= 2, Name = "Toys" },
            new Category { Id= 3, Name = "Hardware" }
        };


        public Category[] getCategories() //public get function to get categories value from outside the class.
        {
            return categories;
        }
    }
}
