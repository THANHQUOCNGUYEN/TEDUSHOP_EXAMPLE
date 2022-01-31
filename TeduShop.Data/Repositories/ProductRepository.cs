using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Data.Infrastructure;
using TeduShop.Model.Models;
namespace TeduShop.Data.Repositories
{
    public interface IProductRepository:IRepository<Product>
    {
        IEnumerable<Product> GetListProductBytag(string tagId, int page, int pageSize, out int totalRow);
    }
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
        public IEnumerable<Product> GetByAlias(string alias) //ke thua interface
        {
            return this.DbContext.Products.Where(x => x.Alias == alias);
        }


        public IEnumerable<Product> GetListProductBytag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Products
                        join pt in DbContext.ProductTags
                        on p.ID equals pt.ProductID
                        where pt.TagID == tagId
                        select p;
            totalRow = query.Count();
            return query.OrderByDescending(x=>x.CreateDate).Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
