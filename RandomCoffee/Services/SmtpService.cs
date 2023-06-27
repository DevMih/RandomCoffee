using MimeKit;
using MimeKit.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using MailKit.Security;
using static Google.Apis.Auth.OAuth2.LocalServerCodeReceiver;
using RandomCoffee.Entities;

namespace RandomCoffee.Services
{
    public class SmtpService
    {
        public async Task SendMatches(AppUser user1, AppUser user2)
        {
            var sendTo = user1.Email;
            var subject = "";
            var body = "";

            if (user2 != null)
            {
                subject = "You have new match!";
                body =  $"<h1>Hello {user1.UserName}!</h1><br/>"
                    + $"You've been randomly matched with {user2.UserName} on RandomCoffee, and we think it's a perfect match! 🎉<br><br>" +
                    $"Finding someone who shares your interests and sparks great conversations through a random selection is truly remarkable. We're delighted that our platform has brought you together." +
                    $"Now it's time to embrace this opportunity and make the most of it. Reach out to your match, strike up a conversation, and see where it leads. Who knows what amazing connections and possibilities await!" +
                    $"Wishing you a fantastic journey filled with intriguing discussions and memorable moments!<br><br>" +
                    $"Write this person at {user2.Email} or check their profile:<br>" +
                    $"<a href='http://localhost:4200/members/{user2.UserName}'><button>Check Profile</button></a><br><br>" +
                    $"Best regards,<br>" +
                    $"RandomCoffee Team";
            }
            else
            {
                subject = "No matches this time :(";
                body = $"<h1>Hello {user1.UserName}!</h1><br/>"
                    + $"<h2>You didn't match anyone this time.</h2><br/>" + $"<p>Good luck next time.</p>";
            }

            await SendEmail(sendTo, subject, body);
        }

        public async Task SendEmail(string sendToEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("RandomCoffee", "mykhailo.popivniak.pz.2020@lpnu.ua"));
            email.To.Add(MailboxAddress.Parse(sendToEmail));

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                smtp.Authenticate("mykhailo.popivniak.pz.2020@lpnu.ua", "tkiyskgvmvnplbat");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
