using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//Anggota Kelompok :
//Afi
//Lisza
//Syauqi


namespace Hari_7_Tugas_Kelompok
{ 

    public class Program
    {
       
        static void Main()
        {
            List<User> listUser = new List<User>();
            listUser.Add(item: new Admin("Bambang", "Noer","Bano", "1234", true));
            listUser.Add(item: new User("Afif", "fakri", "afifjunihar", "1234", false));
            MenuUtama(listUser);
        }

        static void MenuUtama(List<User> listUser)
        {
            int exit = 0;
            try
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Selamat Datang di Appku");
                    Console.WriteLine("=============================");
                    string[] menuList = { "1. Create User", "2. List User", "3. Search", "4. Login", "5. Exit" };
                    foreach (string menu in menuList)
                    {
                        Console.WriteLine(menu);
                    }
                    Console.Write("Masukan Pilihan Anda (hanya berupa angka) : ");
                    //Console.WriteLine($"{listUser[listUser.FindIndex(user => (user.uname == "Bano"))].pass}");

                    int pilih;
                    pilih = int.Parse(Console.ReadLine());
                    switch (pilih)
                    {
                        case 1:
                            CreateUser(listUser);
                            break;
                        case 2:
                            ShowUser(listUser);
                            break;
                        case 3:
                            Search(listUser);
                            break;
                        case 4:
                            Login(listUser);
                            break;
                        case 5:
                            Environment.Exit(1);
                            //exit = 1;
                            Console.Clear();
                            break;
                    }
                }
                while (exit == 0);
            }
            catch (FormatException)
            {

                Console.WriteLine("\n=============================");
                Console.Write("Masukan Input Not Valid");
                Console.ReadKey();
                MenuUtama(listUser);
            }

            catch (IndexOutOfRangeException)
            {
                Console.Write(" Mohon Masukan ID yang Benar");
                Console.ReadKey();
                MenuUtama(listUser);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("\n=============================");
                Console.Write("\nMohon Masukan Data dengan Benar");
                Console.ReadKey();
                MenuUtama(listUser);
            }
        }

        static void CreateUser(List<User> listUser)
        {
            Console.Clear();
            Console.WriteLine("Silahkan masukkan data yang dibutuhkan seperti pada form berikut :");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.Write("Nama Depan\t: ");
            string namaDepan = Console.ReadLine();
            Console.Write("Nama Belakang\t: ");
            string namaBelakang = Console.ReadLine();
            string uname = namaDepan.Substring(0, 2) + namaBelakang.Substring(0, 2);


            // Check Username Already Exist
            bool checkUser = listUser.Exists(item => item.uname == uname);
            if (checkUser)
            {
                uname += RandomUname();
            }
 
            Console.Write($"Username\t: {uname}");
            Console.Write("\nPassword\t: ");
            string pass = Console.ReadLine();

            // Add Regex
            Regex rgx = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$",
            RegexOptions.Compiled);

            // Find matches.
            var matches = rgx.IsMatch(pass);

            if (matches)
            {
                listUser.Add(new User(namaDepan, namaBelakang, uname, pass, false));
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("Data berhasil diregistrasi, silahkan login untuk mengakses profile");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\nPassword harus memenuhi syarat:\n" +
                "1. Min 1 Number \n" +
                "2. Min 1 Huruf Kecil\n" +
                "3. Min 1 Huruf Besar\n" +
                "4. Min 1 Huruf Symbol\n" +
                "5. Min 8 Karakter\n"
                );

                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowUser(List<User> listUser)
        {
            Console.Clear();

            if (listUser.Count == 0)
            {
                Console.Write("Data User Kosong");
                Console.ReadKey();
            }
            int i = 0;
            foreach (var user in listUser)
            {
                i += 1;
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"ID           : {i}" );
                Console.WriteLine($"Nama Depan   : {user.namaDepan}");
                Console.WriteLine($"namaBelakang : {user.namaBelakang}");
                Console.WriteLine($"Username     : {user.uname}" );
                Console.WriteLine("-------------------------------------------");
                
            }
            Console.ReadKey();
        }

        static void Search(List<User> listUser)
        {
            Console.Clear();
            Console.WriteLine("        Search Appku         ");
            Console.WriteLine("=============================");
            Console.Write("Username : ");
            string userName = Console.ReadLine();
            bool exist = listUser.Exists(item => item.uname == userName);

            if (exist)
            {
                string namDep = listUser.Find(user => user.uname.Contains(userName)).namaDepan;
                string namBel = listUser.Find(user => user.uname.Contains(userName)).namaBelakang;
                Console.WriteLine("=============================");
                Console.WriteLine("Username Anda Sudah Terdaftar");
                Console.WriteLine($"Dengan Nama : {namDep} {namBel}");
                Console.WriteLine("=============================");
                Console.ReadKey();
            }

            else
            {
                Console.WriteLine("=============================");
                Console.WriteLine("Username Anda Belum Terdaftar");
                Console.ReadKey();
            }
                   
            Console.ReadKey();

        }

        static void Login(List<User> listUser)
        {
            Console.Clear();
            Console.WriteLine("          Login Appku        ");
            Console.WriteLine("=============================");
            Console.Write("Username\t: ");
            string userName = Console.ReadLine();
            Console.Write("Password\t: ");

            string pass = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } 
            while (key != ConsoleKey.Enter);

            bool exist = listUser.Exists(item => item.uname == userName);

            if (exist)
            {
                bool passBenar = BCrypt.Net.BCrypt.Verify(pass, listUser[listUser.FindIndex(user => user.uname == userName)].pass);

                if (passBenar) 
                {
                    int index = listUser.FindIndex(user => (user.uname == userName));
                    if (listUser[index].admin)
                    {
                        Console.WriteLine("\n=============================");
                        Console.WriteLine($"Selamat datang Admin : {userName}");
                        Console.ReadKey();
                        AdminLoggedIn(listUser, userName);                      
                    }

                    Console.WriteLine("\n=============================");
                    Console.WriteLine($"Selamat Datang {userName}");
                    Console.ReadKey();
                    LoggedIn(listUser, userName);                    
                }
                else 
                {
                    Console.WriteLine("\n=============================");
                    Console.WriteLine("Gagal Login: Password yang Anda masukkan salah");
                    Console.ReadKey();
                }               
            }

            else            
            {
                Console.WriteLine("\n=============================");
                Console.WriteLine("Gagal Login: Username tidak terdaftar");
                Console.ReadKey();
            }

         }

        static void LoggedIn(List<User> listUser, string userName)
        {
            Console.Clear();
            Console.WriteLine($"Selamat Datang, {userName}");
            Console.WriteLine("=============================");
            string[] menuList = { "1. Edit Username", "2. Edit Password", "3. Delete Account", "4. Keluar"};
            foreach (string menu in menuList)
            {
                Console.WriteLine(menu);
            }
            Console.Write("Pilihan Anda : ");
            int pilihlogin = int.Parse(Console.ReadLine());
            // Get User Index
            int index = listUser.FindIndex(user => (user.uname == userName));

            // Menu Options
            switch (pilihlogin)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("==============================");
                    Console.WriteLine("        Edit User Name        ");
                    Console.WriteLine("==============================");
                    Console.Write("Username Baru\t\t: ");
                    string unm1 = Console.ReadLine();
                    Console.Write("Konfirmasi userName\t: ");
                    string unm2 = Console.ReadLine();

                    if (unm1 == unm2)
                    {
                        if (unm1.Length != 0)
                        {
                            listUser[index].uname = unm1;
                            Console.WriteLine("==============================");
                            Console.Write("Username Berhasil Diganti");
                            Console.ReadKey();
                            LoggedIn(listUser, userName);
                        }
                        else
                        {
                            Console.WriteLine("==============================");
                            Console.Write("Username Gagal Diganti: Username tidak boleh kosong");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("==============================");
                        Console.Write("Username Gagal Diganti: Username yang dimasukkan tidak cocok");
                        Console.ReadKey();
                        LoggedIn(listUser, userName);
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("==============================");
                    Console.WriteLine("         Edit Password        ");
                    Console.WriteLine("==============================");
                    Console.Write("Password Baru\t\t: ");
                    string pass1 = Console.ReadLine();
                    Console.Write("Konfirmasi Password\t: ");
                    string pass2 = Console.ReadLine();

                    if (pass1 == pass2)
                    {
                        listUser[index].pass = pass1;
                        // Add Regex
                        Regex rgx = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$",
                        RegexOptions.Compiled);

                        // Find matches.
                        var matches = rgx.IsMatch(pass2);
                        if (matches)
                        {
                            listUser[index].pass = pass2;
                            Console.WriteLine("\n==============================");
                            Console.Write("Password Berhasil Diganti");
                            Console.ReadKey();
                            LoggedIn(listUser, userName);
                        }
                        else 
                        {
                            GagalGantiPass();
                            Console.ReadKey();
                            Console.Clear();
                            LoggedIn(listUser, userName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n==============================");
                        Console.Write("Password Gagal Diganti: Password yang dimasukkan tidak cocok");
                        GagalGantiPass();
                        Console.ReadKey();
                        Console.Clear();
                        LoggedIn(listUser, userName);
                    }                   
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("==================================");
                    Console.WriteLine("            Remove Akun           ");
                    Console.WriteLine("==================================");

                    bool exist = listUser.Exists(item => item.uname == userName);

                    if (exist)
                    {
                        Console.Write("Apakah Anda yakin ingin menghapus akun ini? [Y] : ");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            listUser.RemoveAll(item => item.uname == userName);
                            Console.Write("\nUser berhasil dihapus");
                            Console.ReadKey();
                            MenuUtama(listUser);
                        }
                        else
                        {
                            Console.Write("\nUser gagal terhapus");
                            Console.ReadKey();
                            LoggedIn(listUser, userName);
                        }
                    }
                    break;
                case 4:
                    MenuUtama(listUser);
                    break;
            }
        }
        static void GagalGantiPass()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("Password Gagal Diganti");
            Console.WriteLine("Password harus memenuhi syarat:");
            Console.WriteLine("1. Min 1 Number");
            Console.WriteLine("2. Min 1 Huruf Kecil");
            Console.WriteLine("3. Min 1 Huruf Besar");
            Console.WriteLine("4. Min 1 Huruf Symbol");
            Console.WriteLine("5. Min 8 Karakter");
        }
        static public string RandomUname()
        {
            Random rd = new Random();
            int randNum = rd.Next(1, 100);
            return $"{randNum}";
        }

        static void AdminLoggedIn(List<User> listUser, string userName)
        {
            Console.Clear();
            Console.WriteLine($"Selamat Datang, Admin {userName}");
            Console.WriteLine("=============================");
            string[] menuList = { "1. Edit Username", "2. Edit Password", "3. Delete Account", "4. Delete User", "5. Keluar" };
            foreach (string menu in menuList)
            {
                Console.WriteLine(menu);
            }
            Console.Write("Pilihan Anda : ");
            int pilihlogin = int.Parse(Console.ReadLine());
            // Get User Index
            int index = listUser.FindIndex(user => (user.uname == userName));

            // Menu Options
            switch (pilihlogin)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("==============================");
                    Console.WriteLine("        Edit User Name        ");
                    Console.WriteLine("==============================");
                    Console.Write("Username Baru\t\t: ");
                    string unm1 = Console.ReadLine();
                    Console.Write("Konfirmasi userName\t: ");
                    string unm2 = Console.ReadLine();

                    if (unm1 == unm2)
                    {
                        if (unm1.Length != 0)
                        {
                            listUser[index].uname = unm1;
                            Console.WriteLine("==============================");
                            Console.Write("Username Berhasil Diganti");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                        else
                        {
                            Console.WriteLine("==============================");
                            Console.Write("Username Gagal Diganti: Username tidak boleh kosong");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("==============================");
                        Console.Write("Username Gagal Diganti: Username yang dimasukkan tidak cocok");
                        Console.ReadKey();
                        AdminLoggedIn(listUser, userName);
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("==============================");
                    Console.WriteLine("         Edit Password        ");
                    Console.WriteLine("==============================");
                    Console.Write("Password Baru\t\t: ");
                    string pass1 = Console.ReadLine();
                    Console.Write("Konfirmasi Password\t: ");
                    string pass2 = Console.ReadLine();

                    if (pass1 == pass2)
                    {
                        listUser[index].pass = pass1;
                        // Add Regex
                        Regex rgx = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$",
                        RegexOptions.Compiled);

                        // Find matches.
                        var matches = rgx.IsMatch(pass2);
                        if (matches)
                        {
                            listUser[index].pass = pass2;
                            Console.WriteLine("\n==============================");
                            Console.Write("Password Berhasil Diganti");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                        else
                        {
                            GagalGantiPass();
                            Console.ReadKey();
                            Console.Clear();
                            AdminLoggedIn(listUser, userName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n==============================");
                        Console.Write("Password Gagal Diganti: Password yang dimasukkan tidak cocok");
                        GagalGantiPass();
                        Console.ReadKey();
                        Console.Clear();
                        AdminLoggedIn(listUser, userName);
                    }
                    break;

                case 3:
                    Console.Clear();
                    Console.WriteLine("==================================");
                    Console.WriteLine("            Remove Akun           ");
                    Console.WriteLine("==================================");

                    bool exist = listUser.Exists(item => item.uname == userName);

                    if (exist)
                    {
                        Console.Write("Apakah Anda yakin ingin menghapus akun Admin ini? [Y] : ");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            listUser.RemoveAll(item => item.uname == userName);
                            Console.Write("\nUser berhasil dihapus");
                            Console.ReadKey();
                            MenuUtama(listUser);
                        }
                        else
                        {
                            Console.Write("\nUser gagal terhapus");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                    }
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine("==================================");
                    Console.WriteLine("          Remove Akun User        ");
                    Console.WriteLine("==================================");

                    Console.WriteLine("Tulis Username user yang mau dihapus");
                    string removeUserName = Console.ReadLine();
                    bool ada = listUser.Exists(item => (item.uname == removeUserName) && (item.admin == false));

                    if (ada)
                    {
                        Console.Write("Apakah Anda yakin ingin menghapus akun ini? [Y] : ");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            listUser.RemoveAll(item => item.uname == removeUserName);
                            Console.Write("\nUser berhasil dihapus");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }

                        else
                        {
                            Console.Write("\nUser gagal terhapus");
                            Console.ReadKey();
                            AdminLoggedIn(listUser, userName);
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Username tidak terdaftar");
                        Console.ReadKey();
                        AdminLoggedIn(listUser, userName);
                    }
                    break;

                case 5:
                    MenuUtama(listUser);
                    break;
            }
        }
    }
}