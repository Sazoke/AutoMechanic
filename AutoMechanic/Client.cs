using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic
{
    public class Client:User
    {
        public Client(string name, string surname, string phoneNumber, string login, int passwordHash) : base(login, passwordHash)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string PhoneNumber { get; private set; }
    }
}
