using System;
using System.Collections.Generic;
using System.Text;

namespace AvorLight.Data
{
    interface IDataService
    {
        /// <summary>
        /// Return every existing category
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductCategory> GetProductCategory();

        /// <summary>
        /// Returns all products belonging to the given categoryId
        /// </summary>
        /// <param name="categoryId">Category ID for which the products should be returned</param>
        /// <returns></returns>
        IEnumerable<Product> GetProduct(int categoryId);

        void AddCartProduct(Product product);

        void DeleteCartProduct(Product product);

        IEnumerable<CartProduct> GetCartProduct();

        void Checkout();
    }
}
