using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain1
{
    public interface ICustomerRepository
    {
        void Save(Customer customer);
    }
}
