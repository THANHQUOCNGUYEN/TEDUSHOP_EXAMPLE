using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Data.Infrastructure
{
    //tu dong huy
    public interface IDbFactory:IDisposable
    {
        TeduShopDbContext Init(); //Phuong thuc khoi tao
    }
}
