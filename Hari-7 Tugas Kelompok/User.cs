namespace Hari_7_Tugas_Kelompok
{
    public class User
    {
        public string namaDepan;
        public string namaBelakang;
        public bool admin;
        private string userName;
        private string password;
        public string uname
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }
        public string pass
        {
            get
            {
                return password;
            }
            set
            {
                password = BCrypt.Net.BCrypt.HashPassword(value); ;
            }
        }

        public User(string namaDepan, string namaBelakang, string userName, string password, bool admin)
        {
            this.namaDepan = namaDepan;
            this.namaBelakang = namaBelakang;
            this.userName = userName;
            this.password = BCrypt.Net.BCrypt.HashPassword(password);
            this.admin = admin;
        }
    }
}