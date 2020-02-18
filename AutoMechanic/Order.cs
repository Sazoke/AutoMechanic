using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic
{
    public class Order
    {
        public Order(Client client, string numberOfMachine, string modelOfMachine)
        {
            Client = client;
            NumberOfMachine = numberOfMachine;
            ModelOfMachine = modelOfMachine;
        }

        public Client Client { get; private set; }
        public string NumberOfMachine { get; private set; }
        public string ModelOfMachine { get; private set; }
    }
}
