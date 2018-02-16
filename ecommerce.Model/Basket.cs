using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Model
{
    public class Basket
    {        
        public Guid BasketId { get; set; }        
        public DateTime date { get; set; }

        public virtual ICollection<BasketItem> BasketItems { get; set; }
    }
}
