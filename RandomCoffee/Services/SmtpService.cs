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
        /*private const string DefaultClosePageResponse =
@"<html>
  <head><title>OAuth 2.0 Authentication Token Received</title></head>
  <body>
    Received verification code. You may now close this window.
    <script type='text/javascript'>
      // This doesn't work on every browser.
      window.setTimeout(function() {
          this.focus();
          window.opener = this;
          window.open('', '_self', ''); 
          window.close(); 
        }, 1000);
      //if (window.opener) { window.opener.checkToken(); }
    </script>
  </body>
</html>";*/

        public async Task SendEmail(AppUser user1, AppUser user2)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("no reply", "mykhailo.popivniak.pz.2020@lpnu.ua"));
            email.To.Add(MailboxAddress.Parse(user1.Email));

            if (user2 != null)
            {
                email.Subject = "You have new match!";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h1>Hello {user1.UserName}!</h1><br/>"
                    + $"<h2>You matched {user2.UserName}.</h2><br/>" + $"<p>Write this person at {user2.Email}.</p>"
                };
            }
            else
            {
                email.Subject = "No matches this time :(";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h1>Hello {user1.UserName}!</h1><br/>"
                    + $"<h2>You didn't match anyone this time.</h2><br/>" + $"<p>Good luck next time.</p>"
                };
            }

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                //await OAuthAsync(smtp);
                smtp.Authenticate("mykhailo.popivniak.pz.2020@lpnu.ua", "tkiyskgvmvnplbat");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        /*static async Task OAuthAsync(SmtpClient client)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = "948212602902-nt7omubfq6k123pkd5hqr0tjrrie7j59.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets
            });

            // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            var codeReceiver = new LocalServerCodeReceiver(DefaultClosePageResponse, CallbackUriChooserStrategy.ForceLocalhost);
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync("mykhailo.popivniak.pz.2020@lpnu.ua", CancellationToken.None);

            if (credential.Token.IsExpired(SystemClock.Default))
                await credential.RefreshTokenAsync(CancellationToken.None);

            // Note: We use credential.UserId here instead of GMailAccount because the user *may* have chosen a
            // different GMail account when presented with the browser window during the authentication process.
            SaslMechanism oauth2;

            if (client.AuthenticationMechanisms.Contains("OAUTHBEARER"))
                oauth2 = new SaslMechanismOAuthBearer(credential.UserId, credential.Token.AccessToken);
            else
                oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            await client.AuthenticateAsync(oauth2);
        }*/
    }
}
