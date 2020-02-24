namespace AutoMechanic
{
    public class Client:User
    {
        public Client(string name, string surname, string phoneNumber, string login = null, int passwordHash = 0) : base(login, passwordHash)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
        }

        public Client(string[] datas) : this(datas[2], datas[3], datas[4], datas[0], datas[1].GetHashCode()) { }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string PhoneNumber { get; private set; }
        public override string ToString() => Name + " " + Surname + " " + PhoneNumber;
    }
}
