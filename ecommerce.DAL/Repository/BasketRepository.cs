using ecommerce.DAL.Data;
using ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DAL.Repository
{
    public class BasketRepository : RepositoryBase<Basket>
    {
        public BasketRepository(DataContext context) : base(context)
        {
            if (context == null)
                throw new ArgumentNullException();
        }
    }
}
