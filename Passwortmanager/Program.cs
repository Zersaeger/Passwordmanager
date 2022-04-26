using System.Text.RegularExpressions;
using System.IO;
class Program
{
    public static bool run = true;
    public static string passwordGen = "";
    public static string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "passwords");
    public static void Main()
    {
        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);
        if (!File.Exists(Path.Combine(basePath, "Passwords"))){
            File.Create(Path.Combine(basePath, "Passwords")).Close();
        }
        if(!File.Exists(Path.Combine(basePath, "MasterPassword.txt"))){
            string input;
            File.Create(Path.Combine(basePath, "MasterPassword.txt")).Close();
            Console.Write("Set here your password, which gives you access to all the password you created and saved: ");
                input = Console.ReadLine()!;
                if (input == "exit")
                {
                    return;
                }
                File.WriteAllText(Path.Combine(basePath, "MasterPassword.txt"), input);
        }

        string[] commands = new string[10];
        commands[0] = "generate";
        commands[1] = "new password";
        commands[2] = "clear";
        commands[3] = "exit";
        commands[4] = "set password";
        commands[5] = "get password";
        commands[6] = "delete";
        commands[7] = "password strength";
        commands[8] = "all password names";
        commands[9] = "delete all";
        Console.WriteLine("Hello. What do you want to do? Say 'commands' to see all of them.");
        while (run)
        {
            Console.Write("Command<< ");
            string cmd = Console.ReadLine()!;
            if (cmd == commands[0])
            {
                Generate();
            }
            else if (cmd == commands[2])
            {
                Console.Clear();
            }
            else if (cmd == commands[1])
            {
                NewPassword();
            }
            else if (cmd == commands[4])
            {
                Setpassword();
            }
            else if (cmd == commands[5])
            {
                GetPassword();
            }
            else if (cmd == commands[6])
            {
                Delete();
            }
            else if (cmd == commands[7])
            {
                PasswordStrength();
            }
            else if (cmd == "commands")
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    Console.WriteLine(commands[i]);
                }
            }
            else if(cmd == commands[8]){
                allPasswords();
            }
            else if(cmd == commands[9]){
                DeleteAll();
            }
            else if (cmd == "exit")
            {
                run = false;
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
        string input = Console.ReadLine()!;
        if (input == "exit")
        {
            return;
        }
        x = int.Parse(input);
        Console.WriteLine("This is your random password: ");
        List<char> charakters = new(94) { '!', '§', '$', '%', '&', '/', '(', ')', '[', ']', '{', '}', '=', '\\', '?', '~', '#', '*', '-', '_', '+', '<', '>', '|', ';', ',', '"', '\'', '.', ':', '°', '^' };
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
        char[] genPassword = new char[x];
        for (int j = 0; j < x; j++)
        {
            int num = random.Next(0, charakters.Count);
            genPassword[j] = charakters[num];
        }
        passwordGen = new string(genPassword);
        Console.WriteLine(passwordGen);

    }

    static void NewPassword()
    {
        string password;
        string filename = Path.Combine(basePath, "Passwords");
        bool password_found = false;
        int index_of_password_name = 0;
        Console.Write("Which usage?: ");
        string use = Console.ReadLine()!;
        if (use == "exit")
        {
            return;
        }
        
        Console.Write("Type here your password: ");
        password = Console.ReadLine()!;
        
        if (password == "generated password" && passwordGen != "")
        {
            password = passwordGen;
        }
        else if(password == "exit"){
            return;
        }
        string[] allLines = File.ReadAllLines(filename);
        for(int i = 0; i < allLines.Count(); i++){
            string[] tokens = allLines[i].Split(":");
            if(tokens[0] == use){
                password_found = true;
                index_of_password_name = i;
                break;
            }
        }
        if(!password_found){
            StreamWriter x = File.AppendText(filename);
            x.WriteLine(use + ":" + password);
            x.Close();
        }
        else{
            int index = index_of_password_name;
            allLines[index] = use + ":" + password;
            File.WriteAllLines(filename, allLines);
        }
        
    }

    static void Setpassword()
    {
        string adminPassword = File.ReadAllText(Path.Combine(basePath, "MasterPassword.txt"));
        if(adminPassword != ""){
            Console.WriteLine("You already have an administration-password");         
            for(int i = 0; i<3; i++){
                Console.Write("Enter your old password: ");
                string input = Console.ReadLine()!;
                if (input == adminPassword){
                    Console.Write("Set here your password, which gives you access to all the password you created and saved: ");
                    adminPassword = Console.ReadLine()!;
                    if (adminPassword == "exit")
                    {
                    return;
                    }
                    else{
                    File.WriteAllText(Path.Combine(basePath, "MasterPassword.txt"), adminPassword);
                    }
                    i = 3;
                }
                else if (input != adminPassword){
                        Console.WriteLine("wrong password, try it again...");
                    }
                }
            }             
        else if(adminPassword == ""){
            Console.Write("Set here your password, which gives you access to all the password you created and saved: ");
                adminPassword = Console.ReadLine()!;
                if (adminPassword == "exit")
                {
                    return;
                }
                File.WriteAllText(Path.Combine(basePath, "MasterPassword.txt"), adminPassword);
        }
    }

    static void GetPassword()
    {
        string input;
        string MasterPassword = File.ReadAllText(Path.Combine(basePath, "MasterPassword.txt"));
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine()!;
            if (input == MasterPassword)
            {
                break;
            }
            else if (input == "exit")
            {
                return;
            }
            else
            {
                x++;
                if (x == 3)
                {
                run = false;
                return;
                }
            }        
        }
        while (true)
        {
            bool password_found = false;
            int index = 0;
            string filename = Path.Combine(basePath, "Passwords");
            Console.Write("Which password do you want?: ");
            input = Console.ReadLine()!;
            if(input == "exit"){
                break;
            }
            string[] allLines = File.ReadAllLines(filename);
            for(int i = 0; i < allLines.Count(); i++){
                string[] tokens = allLines[i].Split(":");
                if(tokens[0] == input){
                    password_found = true;
                    index = i;
                    break;
                }
            }
            if(password_found){
                string[] outputArr = allLines[index].Split(":");
                string output = outputArr[1];
                Console.WriteLine($"Password for {input} is:\n{output}");
                break;
            }
            else{
                Console.WriteLine("Password does not exist yet...");

            }
        }
    }

    static void Delete()
    {
        string input;
        string MasterPassword = File.ReadAllText(Path.Combine(basePath, "MasterPassword.txt"));
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine()!;
            if (input == MasterPassword)
            {
                break;
            }
            else if (input == "exit")
            {
                return;
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
            bool password_found = false;
            string allText = "";
            string fileName = Path.Combine(basePath, "Password");
            string filename = Path.Combine(basePath, "Passwords");
            Console.Write("Which password do you want to delete?: ");
            input = Console.ReadLine()!;
            if(input == "exit"){
                break;
            }
            Console.Write("Are you sure? [Y/N]: ");
            string YN = Console.ReadLine()!;
            if (YN == "Y" || YN == "y")
            {
                string[] allLines = File.ReadAllLines(filename);
                for(int i = 0; i < allLines.Count(); i++){
                    string[] tokens = allLines[i].Split(":");
                    if(tokens[0] != input){
                        allText += allLines[i] + "\n";
                    }
                    else{
                        password_found = true;
                    }
                }
                if(password_found){
                    File.WriteAllText(filename, allText);
                    Console.WriteLine("Password is deleted");
                    return;
                }
                else{
                    Console.WriteLine("Password does not exist yet...");
                    return;
                }
            }
            else
            {
                return;
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
        string MasterPassword = File.ReadAllText(Path.Combine(basePath, "MasterPassword.txt"));
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine()!;
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
            bool password_found = false;
            int index = 0;
            Console.Write("Which password do you want?: ");
            input = Console.ReadLine()!;
            string fileName = Path.Combine(basePath, "Passwords");
            string[] allLines = File.ReadAllLines(fileName);
        for(int i = 0; i < allLines.Count(); i++){
            string[] tokens = allLines[i].Split(":");
            if(tokens[0] == input){
                password_found = true;
                index = i;
                break;
            }
            
        }
        string[] outputArr = allLines[index].Split(":");
        if(password_found){
            string Password = outputArr[1];
                Console.WriteLine(Password);
                passwordLength = Password.Length;
                passwordSC = Regex.Matches(Password, "[°!\"§${[|>:;,._<+}%&/()=?]").Count;
                passwordNR = Regex.Matches(Password, "[0123456789]").Count;
                passwordUC = Regex.Matches(Password, "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]").Count;
                passwordLC = Regex.Matches(Password, "[abcdefghijklmnopqrstuvwxyz]").Count;
                if (passwordLength >= 16 && passwordNR >= 4 && passwordUC >= 4 && passwordSC >= 4 && passwordLC >= 5)
                {
                    Console.WriteLine("The given password is strong");
                }
                else if (passwordLength >= 8 && passwordNR >= 2 && passwordUC >= 2 && passwordSC >= 2 && passwordLC >= 3)
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
    static void allPasswords(){
        string fileName = Path.Combine(basePath, "Passwords");
        if(File.ReadAllText(Path.Combine(basePath, "Passwords")) == ""){
            Console.WriteLine("You have no passwords yet");
            return;
        }
        string[] allLines = File.ReadAllLines(fileName);
        int num = 1;
        for(int i = 0; i < allLines.Count(); i++){
            string[] tokens = allLines[i].Split(":");
            string output = tokens[0];
            if(allLines[i] == null || allLines[i] == ""){
                continue;
            }
            else{
                Console.WriteLine($"{num}) {output}");
                num++;
            }
        }
    }

    static void DeleteAll(){
        string input;
        string MasterPassword = File.ReadAllText(Path.Combine(basePath, "MasterPassword.txt"));
        int x = 0;
        while (true)
        {
            Console.Write("Type your ADMINPASSWORD: ");
            input = Console.ReadLine()!;
            if (input == MasterPassword)
            {
                break;
            }
            else if (input == "exit")
            {
                return;
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
            string fileName = Path.Combine(basePath, "Passwords");
            string content = "";
            Console.Write("Are you sure you want to delete everything? [Y/N]: ");
            string YN = Console.ReadLine()!;
            if (YN == "Y" || YN == "y"){
                File.WriteAllText(Path.Combine(fileName), content);
                return;
            }
            else{
                return;
            }
        }
    }
}