using Microsoft.AspNetCore.Mvc;
using LoginApp.Models;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace LoginApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly string connectionString = "Host=localhost;Username=1;Password=2;Database=3"; // write your username, password, and the name of your database here (replace 1,2,3)

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (AuthenticateUser(model.Username, model.Password))
                {
                    TempData["Message"] = "Login successful!";
                    return View(model);
                }
                else
                {
                    TempData["Message"] = "Wrong username or password.";
                }
            }
            return View(model);
        }

    private bool AuthenticateUser(string username, string password)
    {
        string providedUsernameHash = CustomHash.HashUsername(username);

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT username_hash, password_hash, salt FROM users WHERE username_hash=@username_hash", conn))
            {
                cmd.Parameters.AddWithValue("username_hash", providedUsernameHash);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedUsernameHash = reader.GetString(reader.GetOrdinal("username_hash"));
                        string storedPasswordHash = reader.GetString(reader.GetOrdinal("password_hash"));
                        string storedSalt = reader.GetString(reader.GetOrdinal("salt"));

                        string providedPasswordHash = CustomHash.HashPasswordWithSalt(password, storedSalt);

                        Console.WriteLine("Stored Hashed Username: " + storedUsernameHash);
                        Console.WriteLine("Generated Hashed Username: " + providedUsernameHash);
                        Console.WriteLine("Stored Hashed Password: " + storedPasswordHash);
                        Console.WriteLine("Generated Hashed Password: " + providedPasswordHash);
                        Console.WriteLine("Stored Salt: " + storedSalt);

                        return storedPasswordHash == providedPasswordHash;
                    }
                }
            }
        }
        return false;
    }





    public static class CustomHash
    {
        private const int HashLength = 50; // Desired length of the hash
        private const char UsernameStartingCharacter = '#';
        private const char PasswordStartingCharacter = '$';
        private static readonly Random GlobalRandom = new Random();

        public static string HashUsername(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));


            string rot13 = ApplyRot13(input);

            string customHash = Transform(rot13, input, true);

            customHash = EnsureLengthAndStart(customHash, UsernameStartingCharacter);

            return customHash;
        }

        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            string salt = GenerateSalt();
            string saltedPassword = password + salt;
            string rot13 = ApplyRot13(saltedPassword);

            string customHash = Transform(rot13, saltedPassword, false);

            customHash = EnsureLengthAndStart(customHash, PasswordStartingCharacter);

            return (customHash, salt);
        }

        public static string HashPasswordWithSalt(string password, string salt)
        {
            string saltedPassword = password + salt;
            string rot13 = ApplyRot13(saltedPassword);
            string customHash = Transform(rot13, saltedPassword, false);
            customHash = EnsureLengthAndStart(customHash, PasswordStartingCharacter);
            return customHash;
        }

        private static string ApplyRot13(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }

        private static string Transform(string input, string seed, bool isUsername)
        {
            StringBuilder transformed = new StringBuilder(input.Length);
            Random rand = new Random(GetHashSeed(seed)); // Use the hash of the input string as the seed

            for (int i = 0; i < input.Length; i++)
            {
                if (i % 2 == 0)
                {
                    transformed.Append(input[i]);
                }
                else
                {
                    // Replace every second character with a deterministic "random" number or symbol
                    transformed.Append(GetDeterministicSymbolOrNumber(rand, isUsername));
                }
            }

            return transformed.ToString();
        }

        private static char GetDeterministicSymbolOrNumber(Random rand, bool isUsername)
        {
            string symbolsAndNumbers = isUsername ? "0123456789!@#$%^&*" : "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int index = rand.Next(symbolsAndNumbers.Length);
            return symbolsAndNumbers[index];
        }

    private static string EnsureLengthAndStart(string hash, char startingCharacter)
        {
            if (hash.Length >= HashLength)
            {
                hash = hash.Substring(0, HashLength);
            }
            else
            {
                int fillLength = HashLength - hash.Length;
                int fillStartIndex = hash.Length / 2;

                StringBuilder filler = new StringBuilder();
                for (int i = 0; i < fillLength; i++)
                {
                    filler.Append(GetDeterministicSymbolOrNumber(new Random(i), false));
                }

                hash = hash.Insert(fillStartIndex, filler.ToString());
            }

            return startingCharacter + hash.Substring(1);
        }

        private static string GenerateSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] salt = new char[8];
            lock (GlobalRandom)
            {
                for (int i = 0; i < salt.Length; i++)
                {
                    salt[i] = chars[GlobalRandom.Next(chars.Length)];
                }
            }
            return new string(salt);
        }

        private static int GetHashSeed(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToInt32(bytes, 0);
            }
        }
    }
}
}