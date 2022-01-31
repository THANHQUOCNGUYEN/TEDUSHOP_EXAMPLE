using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;
namespace TeduShop.Data.Repositories
{
    public interface IProductCategoryRepository: IRepository<ProductCategory>  //viet gop chung luon
    {
        IEnumerable<ProductCategory> GetByAlias(string alias);
    }
    public class ProductCategoryRepository:RepositoryBase<ProductCategory>,IProductCategoryRepository
    {
        public ProductCategoryRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
        public IEnumerable<ProductCategory> GetByAlias(string alias) //ke thua interface
        {
            return this.DbContext.ProductCategories.Where(x => x.Alias == alias);
        }
    }
}
