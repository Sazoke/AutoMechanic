using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic
{
    public abstract class User
    {
        public User(string login, int passwordHash)
        {
            this.login = login;
            this.passwordHash = passwordHash;
        }

        private string login;
        private int passwordHash;
    }
}
