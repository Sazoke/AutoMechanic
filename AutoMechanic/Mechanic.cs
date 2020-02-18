using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic
{
    public class Mechanic:User
    {
        public Mechanic(string login, int passwordHash) : base(login, passwordHash) { }

        public List<Order> ListOfOrders { get; private set; }
    }
}
