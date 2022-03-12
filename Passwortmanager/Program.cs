using System.Text.RegularExpressions;
class Program
{
    public static bool run = true;
    public static string passwordgen = "";
    
    public static void Main()
    {
        
        
        string[] commands = new string[8];
        commands[0] = "generate password";
        commands[1] = "new password";
        commands[2] = "clear";
        commands[3] = "exit";
        commands[4] = "set password";
        commands[5] = "get password";
        commands[6] = "delete";
        commands[7] = "password strength";
        Console.WriteLine("Hello. What do you want to do? Say 'commands' to see all of them.");
        while (run)
        {           
            Console.Write("Command<< ");
            string cmd = Console.ReadLine();
            if (cmd == commands[0])
            {
                Generate();
            }
            else if(cmd == commands[2])
            {
                Console.Clear();
            }
            else if(cmd == commands[1])
            {
                NewPassword();
            }
            else if(cmd == commands[4])
            {
                Setpassword();
            }
            else if (cmd == "commands")
            {
                for(int i = 0; i < commands.Length; i++)
                {
                    Console.WriteLine(commands[i]);
                }
            }
            else if(cmd == commands[5])
            {
                GetPassword();
            }
            else if(cmd == commands[6])
            {
                Delete();
            }
            else if (cmd == "exit")
            {
                run = false;
            }
            else if(cmd == commands[7])
            {
                PasswordStrength();
            }
            else
            {
                Console.WriteLine("hmmm this command doesn't exists yet...");
            }
        }       
    }

    static void Generate()
    {
        int x;
        Console.Write("How many Charakters do you want?: ");
        string input = Console.ReadLine();
        x = int.Parse(input);
        Console.WriteLine("This is your random password: ");
        List<char> charakters = new() { '!', '§', '$', '%', '&', '/', '(', ')', '[', ']', '{', '}', '=', '\\', '?', '~', '#', '*', '-', '_', '+', '<', '>', '|', ';', ',', '"', '\'', '.', ':', '°', '^' };   
        for (char i = 'A'; i <= 'Z'; i++)
        {
            charakters.Add(i);
            
        }
        for (char i = 'a'; i <= 'z'; i++)
        {
            charakters.Add(i);
        }
        for (char i = '0'; i <= '9'; i++)
        {
            charakters.Add(i);
        }       
        Random random = new();
        
        for(int j = 0; j < x; j++)
        {
            int num = random.Next(0, charakters.Count);
            passwordgen += charakters[num].ToString();            
        }
        Console.WriteLine(passwordgen);
    }

    static void NewPassword()
    {      
        string use;
        string password;
        string path = @"x:\Passwords\password";
        Console.Write("Which usage?: ");
        use = Console.ReadLine();
        Console.Write("Type here your password: ");
        password = Console.ReadLine();
        path += use;
        if (password == "generated password" && passwordgen != "")
        {
            password = passwordgen;
        }
        File.WriteAllText(Path.Combine(path), password);
    }

    static void Setpassword()
    {
        string adminPassword;
        string path = @"x:\Passwords\MasterPassword.txt";
        Console.Write("Set here your password, which gives you access to all the password you created and saved: ");
        adminPassword = Console.ReadLine();
        File.WriteAllText(Path.Combine(path), adminPassword);
    }

    static void GetPassword()
    {
        string input;
        string MasterPassword = File.ReadAllText(@"X:\Passwords\MasterPassword.txt");
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine();
            if (input == MasterPassword)
            {
                break;
            }
            else
            {
                x++;
            }
            if(x == 3)
            {
                run = false;
                return;
            }
        }
        while (true)
        {
            string fileName = @"x:\Passwords\password";
            Console.Write("Which password do you want?: ");
            input = Console.ReadLine();
            fileName += input;
            if (File.Exists(fileName))
            {
                string fileContent;
                fileContent = File.ReadAllText(fileName);
                Console.WriteLine(fileContent);            
                break;
            }
            else if(input == "exit")
            {
                return;
            }
            else
            {
                Console.WriteLine("Named password doesn't exist yet. Make sure you typed it correctly...");
            }
        }
    }

    static void Delete()
    {
        string input;
        string MasterPassword = File.ReadAllText(@"X:\Passwords\MasterPassword.txt");
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine();
            if (input == MasterPassword)
            {
                break;
            }
            else
            {
                x++;
            }
            if (x == 3)
            {
                run = false;
                return;
            }
        }
        while (true)
        {
            string fileName = @"x:\Passwords\password";
            Console.Write("Which password do you wan't to delete?: ");
            input = Console.ReadLine();
            fileName += input;
            if (File.Exists(fileName))
            {
                Console.Write("Are you sure? [J/N]: ");
                input = Console.ReadLine();
                if (input == "J")
                {
                    File.Delete(fileName);
                    Console.WriteLine("Password was sucessfully deleted!");
                    break;
                }                
                else
                {
                    return;
                }
            }
            else if (input == "exit")
            {
                return;
            }
            else
            {
                Console.WriteLine("Named password doesn't exist yet. Make sure you typed it correctly...");
            }
        }
    }

    static void PasswordStrength()
    {
        int passwordLength;
        int passwordUC;
        int passwordLC;
        int passwordSC;
        int passwordNR;
        string input;
        string MasterPassword = File.ReadAllText(@"X:\Passwords\MasterPassword.txt");
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine();
            if (input == MasterPassword)
            {
                break;
            }
            else
            {
                x++;
            }
            if (x == 3)
            {
                run = false;
                return;
            }
        }
        while (true)
        {
            string fileName = @"x:\Passwords\password";
            Console.Write("Which password do you want?: ");
            input = Console.ReadLine();
            fileName += input;
            if (File.Exists(fileName))
            {
                string fileContent;
                fileContent = File.ReadAllText(fileName);
                Console.WriteLine(fileContent);
                passwordLength = fileContent.Length;
                passwordSC = Regex.Matches(fileContent, "[°!\"§${[|>:;,._<+}%&/()=?]").Count;
                passwordNR = Regex.Matches(fileContent, "[0123456789]").Count;
                passwordUC = Regex.Matches(fileContent, "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]").Count;
                passwordLC = Regex.Matches(fileContent, "[abcdefghijklmnopqrstuvwxyz]").Count;              
                if(passwordLength >= 16 && passwordNR >= 4 && passwordUC >= 4 && passwordSC >= 4 && passwordLC >= 5)
                {
                    Console.WriteLine("The given password is very strong");
                }
                else if(passwordLength >= 8 && passwordNR >= 2 && passwordUC >= 2 && passwordSC >= 2 && passwordLC >= 3)
                {
                    Console.WriteLine("The given password is moderate");
                }
                else
                {
                    Console.WriteLine("The given password is weak");
                }
                break;
            }
            else
            {
                Console.WriteLine("Named password doesn't exist yet. Make sure you typed it correctly...");
            }
        }
    }
}