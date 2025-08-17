namespace SQLExample
{
    internal class User
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public int grade { get; set; } = 0;
        public bool active { get; set; } = false;

        public override string ToString()
        {
            return $"id: {id}, name:{name},email:{email}, passwrod: {password}, phone: {phone}, grade: {grade}, active: {active}";
        }
    }
}
