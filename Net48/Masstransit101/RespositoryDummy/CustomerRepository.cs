﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add
using Domains;

namespace RespositoryDummy
{
    public class CustomerRepository: ICustomerRepository
    {
        public void Save(Customer customer)
        {
            Console.WriteLine(string.Concat("The concrete customer repository was called for customer ", customer.Name));
        }
    }
}
