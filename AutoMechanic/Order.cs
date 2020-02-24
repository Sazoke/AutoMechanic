using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic
{
    public class Order
    {
        public Order(Client client, string modelOfMachine, string numberOfMachine)
        {
            Client = client;
            NumberOfMachine = numberOfMachine;
            ModelOfMachine = modelOfMachine;
        }

        public Client Client { get; private set; }
        public string ModelOfMachine { get; private set; }
        public string NumberOfMachine { get; private set; }
        public override string ToString() => Client.ToString() + " " + ModelOfMachine + " " + NumberOfMachine;
    }
}
