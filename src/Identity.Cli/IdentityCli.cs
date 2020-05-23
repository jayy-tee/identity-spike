using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Identity.Application.Users;
using Identity.Application.Users.Model;
using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.Cli
{
    public class IdentityCli
    {
        private readonly IUserFacade _users;
        public IdentityCli(IUserFacade userFacade)
        {
            _users = userFacade;
        }

        public bool Run()
        {
            Console.WriteLine("Enter a command to continue: ");

            var showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            return true;
        }

        private bool MainMenu()
        {
            Console.Write("Command> ");
            var input = Console.ReadLine();

            return input switch
            {
                "new user" => NewUser(),
                "get user" => GetUser(),
                "quit" => false,
                _ => true,
            };
        }

        private bool NewUser()
        {
            Console.WriteLine("... NEW USER ...\n\nEnter details:");

            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine($"Looking up {username} to see if it exists...\n\n");

            if (_users.GetUser(username).Result != null)
            {
                Console.WriteLine("$Username {username} already exists in the database.");
                return true;
            }

            Console.Write("First Name: ");
            var firstname = Console.ReadLine();
            Console.Write("Last Name: ");
            var lastname = Console.ReadLine();
            Console.Write("Email address: ");
            var emailAddress = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            try
            {
                var user = _users.NewUser(new NewUserDto()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    EmailAddress = emailAddress,
                    Password = password,
                    Username = username

                }).Result;

                Console.WriteLine($"Successfully created {firstname} {lastname} ({username}) in the database.");
            }
            catch
            {
                Console.WriteLine("Failed to created user in database.");
            }

            return true;
        }

        private bool GetUser()
        {
            Console.WriteLine("Enter a username to look up.\n");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine($"Looking up {username}...");

            var user = _users.GetUser(username).Result;
            if (user == null)
            {
                Console.WriteLine($"Could not find user {username} in the database.");
                return true;
            }

            var output = new StringBuilder();
            output.Append($"Username: {user.Username} | Name: {user.FirstName} {user.LastName} | Email: {user.EmailAddress}");
            Console.WriteLine(output.ToString());

            return true;
        }
    }
}