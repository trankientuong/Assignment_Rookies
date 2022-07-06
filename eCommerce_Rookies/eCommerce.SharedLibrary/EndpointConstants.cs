using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary
{
    public static class EndpointConstants
    {
        public static class AccountService
        {
            public static string LOGIN = "api/Auth/Login";
            public static string REGISTER = "api/Auth/Register";
        }

        public static class CategoryService
        {
            public static string CATEGORIES = "api/Category/GetCategories";
        }

        public static class ProductService
        {
            public static string PRODUCTS = "api/Product/GetProducts";
            public static string PRODUCTS_BY_CATEGORY = "api/Product/GetProductsByCategory";
            public static string DETAILS = "api/Product/GetProductDetails";
            public static string REVIEW_RATING = "api/Product/ReviewAndRatingProduct";
        }

        public static class CartService
        {

        }
    }
}
