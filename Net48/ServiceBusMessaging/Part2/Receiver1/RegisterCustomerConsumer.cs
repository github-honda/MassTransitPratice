using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
// add
using MassTransit;
using Messaging1;

namespace Receiver1
{
    public class RegisterCustomerConsumer : IConsumer<IRegisterCustomer>
    {
        public Task Consume(ConsumeContext<IRegisterCustomer> context)
        {
            IRegisterCustomer newCustomer = context.Message;
            Debug.WriteLine("A new customer has signed up, it's time to register it. Details: ");
            Debug.WriteLine(newCustomer.Address);
            Debug.WriteLine(newCustomer.Name);
            Debug.WriteLine(newCustomer.Id);
            Debug.WriteLine(newCustomer.Preferred);
            return Task.FromResult(context.Message);
        }
    }
}
