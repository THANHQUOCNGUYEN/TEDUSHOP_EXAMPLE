using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Data.Infrastructure
{
    //Phuong thuc ket noi du lieu
    public class DbFactory:Disposable,IDbFactory
    {
        private TeduShopDbContext dbContext;
        public TeduShopDbContext Init() //Implimemt 
        {
            return dbContext ?? (dbContext = new TeduShopDbContext());
        }

        protected override void DisposeCore() //ghi de phuong thuc con
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
