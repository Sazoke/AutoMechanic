namespace AutoMechanic
{
    public class Client:User
    {
        public Client(string login, int passwordHash, string name, string surname, string phoneNumber) : base(login, passwordHash)
        {
            Name = name;
            Surname = surname;
            PhoneNumber = phoneNumber;
        }

        public Client(string[] datas) : this(datas[0], datas[1].GetHashCode(), datas[2], datas[3], datas[4]) { }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string PhoneNumber { get; private set; }
    }
}
