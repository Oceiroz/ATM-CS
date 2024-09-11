using System;
using System.IO;

namespace Bank_Teller_Machine
{
    class Program
    {
        public static string username, fName, lName;
        public static bool overdraft;
        public static int password;
        public static double money, overdraftAmount = 0.0;
        public static string filePath = @"Accounts";
        public static string[] accountList = GetAccounts();
        static void Main(string[] args)
        {
            bool signOut = false;
            while (signOut == false)
            {
                int area = 0;
                bool verified = false;
                int start;
                start = Menu();
                if (start == 1)
                {
                    CreateAccount();
                }
                else
                {
                    verified = SignIn();
                }
                while (verified == true)
                {
                    area = AccountInfo();
                    if (area == 1)
                    {
                        AccountBalance();
                    }
                    else if (area == 2)
                    {
                        DepositMoney();
                    }
                    else if (area == 3)
                    {
                        WithdrawMoney();
                    }
                    else if (area == 4)
                    {
                        OverdraftApply();
                    }
                    else if (area == 5)
                    {
                        HelpMenu();
                    }
                    else if (area == 6)
                    {
                        signOut = SignOut();
                        verified = false;
                    }
                }
            }
        }
        static string GetStringInput(string inputMessage)
        {
            string userInput = "";
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = rawInput;
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static int GetIntInput(string inputMessage)
        {
            int userInput = 0;
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = int.Parse(rawInput);
                    if (userInput < 0 && rawInput.Length != 4)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static double GetDoubleInput(string inputMessage)
        {
            double userInput = 0.0;
            while (true)
            {
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    userInput = double.Parse(rawInput);
                    Math.Round(userInput, 2);
                    if (userInput < 0)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("invalid input");
                }
            }
            return userInput;
        }
        static int GetChoice(string inputMessage, string[] options)
        {
            int choice = 0;
            while (true)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"\n{i + 1} ---> {options[i]}\n");
                }
                Console.WriteLine($"{inputMessage}\n");
                string rawInput = Console.ReadLine();
                try
                {
                    choice = int.Parse(rawInput);
                    if (choice > options.Length || choice <= 0)
                    {
                        throw new FormatException();
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("this is not a valid input");
                }
            }
            return choice;
        }
        static string[] GetAccounts()
        {
            string[] accounts = Directory.GetFiles(filePath);
            string[] accountList = new string[accounts.Length];
            int x = 0;
            foreach (string username in accounts)
            {
                string newUsername = username.Replace(".txt", "");
                string newUsername1 = newUsername.Replace($"{filePath}\\","");
                accountList[x] = newUsername1;
                x++;
            }
            return accountList;
        }
        static void OpenAccount()
        {
            string[] lines = File.ReadAllLines($"{filePath}\\{username}.txt");
            try
            {
                fName = lines[0];
                lName = lines[1];
                money = double.Parse(lines[2]);
                password = int.Parse(lines[3]);
                overdraft = bool.Parse(lines[4]);
                overdraftAmount = double.Parse(lines[5]);
            }
            catch (FormatException)
            {
                Console.WriteLine("unintentional error occured in openAccount();");
            }
        }
        static int Menu()
        {
            string[] options = {"Create Account", "Sign-In"};
            int choice = GetChoice("What would you like to do?", options);
            return choice;
        }
        static void CreateAccount()
        {
            bool x = true;
            while (x == true)
            {
                int rawUsername = new Random().Next(0, 99999999);
                string usernameTemp = rawUsername.ToString("D8");
                if (accountList.Length == 0)
                {
                    username = usernameTemp;
                    x = false;
                }
                else
                {
                    foreach (string name in accountList)
                    {
                        if (name != usernameTemp)
                        {
                            username = usernameTemp;
                            x = false;
                        }
                    }
                }
            }
            fName = GetStringInput("What is your first name?");
            lName = GetStringInput("what is your last name?");
            money = GetDoubleInput("How much money do you currently have?");
            while (true)
            {
                password = GetIntInput("Please enter a 4 digit password");
                int passwordReenter = GetIntInput("Please re-enter your password");
                if (password != passwordReenter)
                {
                    Console.WriteLine("Your password does match the first password\n");
                }
                else
                {
                    Console.WriteLine("Password has been set\n");
                    Console.WriteLine($"your account id is: {username}, do not forget this\n");
                    break;
                }
            }
            overdraft = false;
            overdraftAmount = 0.00;
            SaveFile();
        }
        static bool SignIn()
        {
            bool verified;
            bool x = true;
            while (x == true)
            {
                username = GetStringInput("What is your Username?");
                foreach (string account in accountList)
                {
                    if (account == username)
                    {
                        x = false;
                        OpenAccount();
                        break;
                    }
                }
                if (x == true)
                {
                    Console.WriteLine("This ID could not be found, please try again:\n");
                }
            }
            while (true)
            {
                int passTemp = GetIntInput("Please enter your 4 digit pin");
                if (passTemp == password)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("password is incorrect, please try again");
                }
            }
            verified = true;
            return verified;
        }
        static bool Finish()
        {
            bool x = true;
            string[] options = { "Yes", "No" };
            int choice = GetChoice("are you finished in this section?", options);
            if (choice == 1)
            {
                x = false;
            }
            return x;
        }
        static int AccountInfo()
        {
            Console.WriteLine($"Welcome {fName}:\n");
            string[] options = { "Account Balance", "Deposit Money", "Withdraw Money", "Apply Overdraft", "Help", "Sign-Out" };
            int area = GetChoice("Where woudld you like to go?", options);
            return area;
        }
        static void AccountBalance()
        {
            bool x = true;
            while (x == true)
            {
                Console.WriteLine($"Your account balance is:{money}\n");
                x = Finish();
            }
        }
        static void DepositMoney()
        {
            bool x = true;
            while (x == true)
            {
                double amountEnter = GetDoubleInput("How much money would you like to deposit?");
                money += amountEnter;
                Console.WriteLine($"Your new total is {money}");
                x = Finish();
            }
        }
        static void WithdrawMoney()
        {
            bool x = true;
            while (x == true)
            {
                double amountRemove = GetDoubleInput("How much money would you like to withdraw?");
                if (amountRemove > money && overdraft == false)
                {
                    Console.WriteLine("You cannot withdraw that amount\n");
                }
                else if (amountRemove > money && overdraft == true)
                {
                    double totalTemp = money + overdraftAmount;
                    if (amountRemove > totalTemp)
                    {
                        Console.WriteLine("You cannot withdraw that amount as it goes beyond your overdraft\n");
                    }
                }
                else
                {
                    money -= amountRemove;
                    Console.WriteLine($"Your new total is: {money}\n");
                }
                x = Finish();
            }
        }
        static void OverdraftApply()
        {
            bool x = true;
            while (x == true)
            {
                if (overdraft == true)
                {
                    string[] options = { "Yes", "No" };
                    int choice = GetChoice("It looks like you already have an overdraft...\nwould you like to change it?", options);
                    if (choice == 1)
                    {
                        Console.WriteLine("If you set it to 0, we will remove the overdraft");
                        overdraftAmount = GetDoubleInput("What would you like your overdraft to be?");
                    }
                }
                else
                {
                    string[] options = { "Yes", "No" };
                    int choice = GetChoice("It looks like you do not have an overdraft...\nwould you like one?", options);
                    if (choice == 1)
                    {
                        overdraftAmount = GetDoubleInput("What would you like your overdraft to be?");
                    }
                }
                if (overdraftAmount == 0)
                {
                    overdraft = false;
                }
                else
                {
                    overdraft = true;
                }
                x = Finish();
            }
        }
        static void HelpMenu()
        {
            while (true)
            {

            }
        }
        static bool SignOut()
        {
            bool signOut = true;
            while (true)
            {
                string[] options = { "Yes", "No" };
                int confirmation = GetChoice("Are you sure you wish to sign out?", options);
                if (confirmation == 1)
                {
                    SaveFile();
                    break;
                }
                else
                {
                    AccountInfo();
                }
            }
            return signOut;
        }
        static void SaveFile()
        {
            FileStream newFile = File.Open($"{filePath}\\{username}.txt", FileMode.OpenOrCreate);
            StreamWriter writeFile = new(newFile);
            dynamic[] credentials = {fName, lName, money, password, overdraft, overdraftAmount};
            foreach (dynamic cred in credentials)
            {
                writeFile.WriteLine(cred);
            }
            writeFile.Close();
        }
    }
}
