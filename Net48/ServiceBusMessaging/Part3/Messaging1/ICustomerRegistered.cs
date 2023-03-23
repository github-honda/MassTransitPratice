using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging1
{
    public interface ICustomerRegistered
    {
        Guid Id { get; }
        DateTime RegisteredUtc { get; }
        string Name { get; }
        string Address { get; }
    }
}
