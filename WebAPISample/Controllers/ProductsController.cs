using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPISample.Models;

namespace WebAPISample.Controllers
{
    public class ProductsController : ApiController
    {
        CategoriesController catcon = new CategoriesController(); //Instantiates Categories Controller to be able to access its public classes
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", CategoryId = 1, Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", CategoryId = 2, Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", CategoryId = 3, Price = 16.99M }
        };
/*
        Category[] categories = new Category[]
        {
            new Category { Id= 1, Name = "Groceries" },
            new Category { Id= 2, Name = "Toys" },
            new Category { Id= 3, Name = "Hardware" }
        };
        */
        public IHttpActionResult GetAllProducts()
        {
            Category[] categories = catcon.getCategories(); //Initializes categories array and setting its value by getting the values set on the Cateories controller via its public get function getCategories().
            List<ExpandoObject> productlist = new List<ExpandoObject>(); //Declares a List ExpandoObject, an object with dynamic properties to be able to create an object that includes the category name.
            foreach(Product prod in products)
            {
                dynamic productdetail = new ExpandoObject();  //Declares a dynamic object where we are to initialized or dynamic properties with its designated values.
                productdetail.Id = prod.Id; //Initializes the Id property with the value of the product id.
                productdetail.Name = prod.Name; //Initializes the Name property with the value of the product name.
                Category category = categories.FirstOrDefault((c) => c.Id == prod.CategoryId); //Getting the name of the product's category using the CategoryId on the product object.
                string catName = (category != null ? category.Name : "Unset"); //initializing a string catName with the value of the Category name or "Unset" if the CategoryId is not found on the categories object.
                productdetail.Category = catName; //Initializing the Category property with the value of the catName that had been derived.
                productdetail.Price = prod.Price; //Initializing the Price property with the value set to the value of the product's price.

                productlist.Add(productdetail); //adding the dynamic object to the productlist List object
            }
            return Ok(productlist);
        }
        
        [Route("api/products/{search}")]  //routing to accept string instead of int which is set by default.
        public IHttpActionResult GetProduct(string search)
        {
            int n; 
            var product = new Product(); 
            if(int.TryParse(search, out n)) //Checks if search is intger or not
            {
                product = this.getById(Convert.ToInt32(search)); //find product by id
            }
            else
            {
                product = this.getByCategory(search); //find product by category
            }
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        private Product getById(int id) //function that returns product if found and null if not based on product id
        {
            return products.FirstOrDefault((p) => p.Id == id);
        }

        private Product getByCategory(String category) // function that returns product if found null if not based on category name
        {
            Category[] categories = catcon.getCategories(); //Initializes categories array and setting its value by getting the values set on the Cateories controller via its public get function getCategories().
            Category cat = categories.FirstOrDefault((c) => c.Name.ToLower() == category.ToLower()); //finding category from catrgories array based on category name.
            if(cat == null)
            {
                return null; //returns null if category not found.
            }
            return products.FirstOrDefault((p) => p.CategoryId == cat.Id); //returns product based by category
        }
    }
}
