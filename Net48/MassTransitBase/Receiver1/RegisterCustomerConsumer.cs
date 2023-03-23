using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains1;
// add
using MassTransit;
using Messaging1;

namespace Receiver1
{
    public class RegisterCustomerConsumer: IConsumer<IRegisterCustomer>
    {
        // 20230320, old
        //public Task Consume(ConsumeContext<IRegisterCustomer> context)
        //{
        //    IRegisterCustomer newCustomer = context.Message;
        //    Console.WriteLine("A new customer has signed up, it's time to register it. Details: ");
        //    Console.WriteLine(newCustomer.Address);
        //    Console.WriteLine(newCustomer.Name);
        //    Console.WriteLine(newCustomer.Id);
        //    Console.WriteLine(newCustomer.Preferred);

        //    context.Publish<ICustomerRegistered>(new
        //    {
        //        Address = newCustomer.Address,
        //        Id = newCustomer.Id,
        //        RegisteredUtc = newCustomer.RegisteredUtc,
        //        Name = newCustomer.Name
        //    });

        //    return Task.FromResult(context.Message);
        //}

        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomerConsumer(ICustomerRepository customerRepository)
        {
            if (customerRepository == null) throw new ArgumentNullException("Customer repository");
            _customerRepository = customerRepository;
        }

        public Task Consume(ConsumeContext<IRegisterCustomer> context)
        {
            IRegisterCustomer newCustomer = context.Message;
            Console.WriteLine("A new customer has signed up, it's time to register it in the command receiver. Details: ");
            Console.WriteLine(newCustomer.Address);
            Console.WriteLine(newCustomer.Name);
            Console.WriteLine(newCustomer.Id);
            Console.WriteLine(newCustomer.Preferred);

            _customerRepository.Save(new Customer(newCustomer.Id, newCustomer.Name, newCustomer.Address)
            {
                DefaultDiscount = newCustomer.DefaultDiscount,
                Preferred = newCustomer.Preferred,
                RegisteredUtc = newCustomer.RegisteredUtc,
                Type = newCustomer.Type
            });

            context.Publish<ICustomerRegistered>(new
            {
                Address = newCustomer.Address,
                Id = newCustomer.Id,
                RegisteredUtc = newCustomer.RegisteredUtc,
                Name = newCustomer.Name
            });

            return Task.FromResult(context.Message);
        }
    }
}
