using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Reflection.Emit;

namespace CoolScrumGames.Pages.Account
{
    public class LoginModel : PageModel
    {
        public Credential Login { get; set; }
        public string Code = "12B45";
        public static int Validated = 0;

        public class RandomGenerator
        {
            // Instantiate random number generator.
            // It is better to keep a single Random instance
            // and keep using Next on the same instance.
            private readonly Random _random = new Random();

            // Generates a random number within a range.
            public int RandomNumber(int min, int max)
            {
                return _random.Next(min, max);
            }

            // Generates a random string with a given size.
            public string RandomString(int size, bool lowerCase = false)
            {
                var builder = new StringBuilder(size);

                // Unicode/ASCII Letters are divided into two blocks
                // (Letters 65–90 / 97–122):
                // The first group containing the uppercase letters and
                // the second group containing the lowercase.

                // char is a single Unicode character
                char offset = lowerCase ? 'a' : 'A';
                const int lettersOffset = 26; // A...Z or a..z: length = 26

                for (var i = 0; i < size; i++)
                {
                    var @char = (char)_random.Next(offset, offset + lettersOffset);
                    builder.Append(@char);
                }

                return lowerCase ? builder.ToString().ToLower() : builder.ToString();
            }

            // Generates a random password.
            // 4-LowerCase + 4-Digits + 2-UpperCase
            public string RandomPassword()
            {
                var passwordBuilder = new StringBuilder();

                // 4-Letters lower case
                passwordBuilder.Append(RandomString(4, true));

                // 4-Digits between 1000 and 9999
                passwordBuilder.Append(RandomNumber(1000, 9999));

                // 2-Letters upper case
                passwordBuilder.Append(RandomString(2));
                return passwordBuilder.ToString();
            }
        }

    public virtual void OnGet()
        {
        }

        public virtual IActionResult OnPost()
        {
            string toEmail = Request.Form["toEmail"];
            var from = "coolscrumgames@gmail.com";
            var password = "erga tqwz qzru ucfy";
            var generator = new RandomGenerator();
            var randomPassword = generator.RandomPassword();
            Code = randomPassword.ToString();

            var Client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(from, password),
                EnableSsl = true,
            };

            string codeMessage = "Your code is: " + Code;

            var mailMessage = new MailMessage(from, toEmail)
            {
                Subject = "Here is your temporary access code.",
                Body = codeMessage,
                IsBodyHtml = false,
            };

            Client.Send(mailMessage);



            return RedirectToPage("/account/accesscode");

        }
    }

    public class Credential
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

       

    }
}
