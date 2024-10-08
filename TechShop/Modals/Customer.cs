using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Modals
{
    public class Customer
    {
        private string email;
        private string phone;

        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

       
        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                {
                    throw new InvalidDataException("Please enter a valid email.");
                }
                email = value;
            }
        }

        // Phone with validation
        public string Phone
        {
            get { return phone; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length != 10)
                {
                    throw new InvalidDataException("phone no must be a 10 digit number .");
                }
                phone = value;
            }
        }

        public string Address { get; set; }

        public Customer(int customerId, string firstName, string lastName, string email, string phone, string address)
        {
            CustomerID = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }

        public Customer()
        {

        }
    }
}
