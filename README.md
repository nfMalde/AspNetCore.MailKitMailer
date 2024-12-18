[![Nuget](https://img.shields.io/nuget/v/AspNetCore.MailKitMailer?style=flat-square)](https://www.nuget.org/packages/AspNetCore.MailKitMailer/) 
 [![Downloads](https://img.shields.io/nuget/dt/AspNetCore.MailKitMailer?style=flat-square)](https://www.nuget.org/packages/AspNetCore.MailKitMailer/)
 [![Paypal Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/donate/?hosted_button_id=SVZHLRTQ6H4VL)
[![.NET](https://github.com/nfMalde/AspNetCore.MailKitMailer/actions/workflows/build-master.yml/badge.svg?branch=main)](https://github.com/nfMalde/AspNetCore.MailKitMailer/actions/workflows/build-master.yml)
# AspNetCore.MailKitMailer (by Malte)
This Mail Client is baded on MailKit to provide HTML-Emails rendered by razor view engine for .NET 6.x


## Other Versions
For best support look at the table below please:

Each minor version has its own support for each .net version.
Future major releases are only released for the next, current and lts support versions.

.NET Version | Package Version | Branch 
------------ | ------------ | ------------
.NET Core 3.1 | 1.0.x | [1.0.x](https://github.com/nfMalde/AspNetCore.MailKitMailer/tree/1.0.x)
.NET 5 | 1.1.x | [1.1.x](https://github.com/nfMalde/AspNetCore.MailKitMailer/tree/1.1.x)
.NET 6 | 1.2.x | [1.2.x](https://github.com/nfMalde/AspNetCore.MailKitMailer/tree/1.2.x)
.NET 8 | 2.0.x | [2.0.x](https://github.com/nfMalde/AspNetCore.MailKitMailer/tree/2.0.x)



# Third Party Dependencies
* [MailKit](https://github.com/jstedfast/MailKit)
* [Json.NET](https://www.newtonsoft.com/json)
* [XUnit](https://xunit.net/)
* [Nunit](https://docs.nunit.org/index.html)
* [Html Agility Pack (HAP)](https://github.com/zzzprojects/html-agility-pack/)
## Install
Using the nuget package manager:

```
Install-Package AspNetCore.MailKitMailer
```

Using the dotnet cli:
```
dotnet add package AspNetCore.MailKitMailer
```

Enable it:
```C#
// Startup.cs

 public IServiceProvider ConfigureServices(IServiceCollection services) {
     
    services.AddAspNetCoreMailKitMailer(Configuration)
                .RegisterAllMailContexesOfCallingAssembly(); // This will add all MailerContexes defined in the calling assembly (see below for more options)
 }
```


## Configuration
You can configure the mailer via IConfiguration using your appsettings.json (I highly recommend to use the AppSecrets in production mode for storing your password/login)
```json
{
"MailKitMailer": {
    "Host": "localhost",
    "Port": 0,
    "UseSSL": true,
    "CheckCertificateRevocation": false,
    "Username": "user",
    "Password": "pass",
    "FromAddress": {
      "Name": "Community",
      "Email": "noreply@localhost"
    }
  }
}
```
### Configuration Entries and their meanings

Configuration Entry Name | Description | Default Value | Type
------------ | ------------- | ------------- | -------------
Host | The host to the smtp server | null | String
Port | The Port to connect to the smtp server | 0 | Integer
UseSSL | Enforces SSL usage for smtp connection | false | Boolean
CheckCertificateRevocation | Force MailKot to dont check for certificate revocation | false | Boolean
Username | The Username to authenticate to the smtp server | null | String
Password | The Password to authenticate to the smtp server | null | String
FromAddress | The From Address for all mails (default from address) | null | FromAddress
FromAddress.Name | The name for the from address (e.g. John Doe) | undefined | String
FromAddress.Email | The email for the from address (e.g. john.doe@localhost) | undefined | String

## Usage


### Preparing your view folder
Create an folder in your root  web project folder called `Mailer-Views`
Add the following files with its contents for the start:

**_ViewStart.cshtml** 
```C#
@{
    Layout = "_Layout";
}
```

**_ViewImports.cshtml**
```C#
@using MailKitMailerExample
@using MailKitMailerExample.Models
@using MailKitMailerExample.Models.MailModels
@using AspNetCore.MailKitMailer.Helper

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, AspNetCore.MailKitMailer // this will add the mailkit css helper to inline css files
```

**Shared/_Layout.cshtml**
```C#
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Integration Tests</title>
</head>
<body>

    <div class="inttest-body">
        @RenderBody()
    </div>

</body>
</html>
```

#### Using the "partial" Tag Helper
In order to use the partial tag helper  `<partial name="viewname" for="Model"/>` you  need to adjust your settings.

##### 1. Create a CustomViewLocationExpander class
To add the "Mailer-Views" path to the actual razor view lookup paths you need an LocatioExpander Class of type `IViewLocationExpander`.
```C#
 public class CustomViewLocationExpander : IViewLocationExpander
 {
     public void PopulateValues(ViewLocationExpanderContext context)
     {
         // Can use context to add custom values based on route or other data
     }

     public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
     {
         // Add custom locations
         return new[]
         {
         "/Mailer-Views/{1}/{0}.cshtml", // Add Mailer Views to the context
     }.Concat(viewLocations); // Ensure default locations are preserved
     }
 }
```
##### 2. Configure the razor view engine using your ViewLocationExpander Class
Now head to your `Program.cs` or for old approach `Startup.cs`.

Add to your services:

```C#
// New approach
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
});

// Old approach
public void ConfigureServices(IServiceCollection services)
{
   services.Configure<RazorViewEngineOptions>(options =>
   {
       options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
   });
}
```

Now the partial view tag helper which is built-in will look into our Mailer-Views folder. 


Now your view folder is set up for using the mailer.
### Creating your contex


Fist we need to create or mailing contex.
Create an class called "TesTmailer".

Our Testmailer class will extend `AspNetCore.MailKitMailer.Data.MailerContextAbstract`.
```C#
using System;
using System.Collections.Generic;
using System.Linq; 
using AspNetCore.MailKitMailer.Data;
using AspNetCore.MailKitMailer.Domain;
using AspNetCore.MailKitMailer.Models;
using MailKitMailerExample.Models.MailModels;

namespace MailKitMailerExample.Mailer
{
    public class TestMailer : MailerContextAbstract
    {
        public IMailerContextResult WelcomeMail(string username, string email)
        {
            return this.HtmlMail(new EmailAddressModel(username, email),
                $"Welcome {username}!",

                new WelcomeModel() { Username = username, Date = DateTime.Now });
        } 
    }
}
```

For explaining: The Method "WelcomeMaiL" will prepare an HtmlMessage (Possible is also plain text Message. Use the helper function "TextMessage" in this case)

Since our method is called "WelcomeMail" and we didnt provide an addtitional view name the mailer will try to render the view "WelcomeMail" in the `~/Mailer-Views/TestMailer/` or `~/Views/Mailer/TestMailer` directory.
Fallback paths for this  whould bne `~/Views/Mailer/WelcomeMail.cshtml` or `~/Mailer-Views/WelcomeMail.cshtml`

All we need to do now is extracting our class to an Interface that extends `AspNetCore.MailKitMailer.Domain.IMailerContext`
```C#
public interface ITestMailer:AspNetCore.MailKitMailer.Domain.IMailerContext
{
    IMailerContextResult WelcomeMail(string username, string email);
}

public class TestMailer : MailerContextAbstract, ITestMailer
{
        ...
    
}

```

Now we create a view located in `~/Mailer-Views/TestMailer` called `WelcomeMail.cshtml` 

#### Sending the Mail
Assuming we are using it inside an MvcController all we have to do is to inject the `IMailClient`.

```C#
using AspNetCore.MailKitMailer.Domain;
using MailKitMailerExample.Mailer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailKitMailerExample.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        private readonly IMailClient client;

        public TestController(IMailClient client)
        {
            this.client = client;
        }

        [HttpGet("welcome")]
        public IActionResult Welcome()
        {
            string username = "John.Doe";
            string useremail = "john@example.com";

            this.client.Send<ITestMailer>(x => x.WelcomeMail(username, useremail));
            
            return View();
        } 
    }
}


```

As we see we are injecting the mail client and calling our contex to prepare the message.
Then we are sending it, in one line.

#### Retrieving the rendered mail content
With Version 2.0.1 added: The `GetContentAsync` Method
It renders the html body of your context call and returns the HTML as string.

Helpfull to display your email in a browser or for debugging your styles.

```C#
namespace MailKitMailerExample.Mailer.Controllers
{
    public class TestController(IMailClient client) : Controller
    { 

        [HttpGet("[action]")]
        public async Task<IActionResult> DebugMail()
        {
             
             string content = await client.GetContentAsync<ITestMailer>(x => x.Test_Single_Values(payload));

            return Content(content, "text/html");
        } 
    }
}

```

#### Default Values
Our contex can provide an load of default values. Lets assume our welcome mail should go also  to "admin@example.com":

```C#

namespace MailKitMailerExample.Mailer
{
    public class TestMailer : MailerContextAbstract, ITestMailer
    {
        public TestMailer() {
            this.DefaultReceipients.Add(new EmailAddressModel("admin", "admin@localhost"));
        }

        public IMailerContextResult WelcomeMail(string username, string email)
        {
            return this.HtmlMail(new EmailAddressModel(username, email),
                $"Welcome {username}!",

                new WelcomeModel() { Username = username, Date = DateTime.Now });
        } 
    }
}
```
In the contructor you see we are adding the admin@locahost address to `DefaultReceipients`.
This will cause if we send an mail we will  also send it to all addresses located in `DefaultReceipients`.
Same for `DefaultCCReceipients` for cc and  `DefaultBCCReceipients` for bcc. 

 

#### Sending Attachments
Sending attachment is as easy as sending an default mail.

Lets create a new contex method for this in our `TestMailer`:

```C#
public IMailerContextResult Test_Attachment(string attachmentPath)
{
return HtmlMail(
    new EmailAddressModel("test", "test@localhost"),
    "Test-Attachment",
    null,
    null,
    x => x.Add(attachmentPath) 
    );
}
```
You see something different here. We have this anonymous function that adds an file path for some kind of collection. Thats our attachment collection. It will hold information of planned attachments and its content type. On send the mail client will resolve this and add it to the mail.
You can also download files to add  as attachments. Just provide an type of "`Uri`" then.
Second paramter in our x.Add method is the content type. So you  can override the content type if youn want to.

Extract it to our interface and test it.

#### Auto-Registering Contexes
AspNetCore.MailKitMailer is able to register all mail contexes to services that match an certain criteria:
The class has to be public
The class exntends `AspNetCore.MailKitMailer.Data.MailerContextAbstract`.
If you want to register it as interface your interface needs to also extend `AspNetCore.MailKitMailer.Domain.IMailerContext`.
This all done your mail contex is automaically available via dependency injection.

#### Tag Helper
Ive created an tag helper based on the old approach of https://www.meziantou.net/inlining-a-stylesheet-using-a-taghelper-in-asp-net-core.htm
```HTML
    <mailer-inline-style href="css/site.css"></mailer-inline-style>
```
This call will load the file css/site.css file as inline style.
By default the base path is the Web-Root e.g. "wwwroot".
You can change this by setting `use-content-root` to  `true`. If this cases it uses the main content root.

**Caching**
The tag supports caching. It will  cache if an memory cache is registered the result in the memory cache.
If an distributed cache is registerted it will prefer this.

To force the use of memory cache you can set `force-memory-cache` to `true`.


#### Url Helper
AspNetCore.MailKitMailer comes with an UrlHelper to provide AbsoluteUrls for mail bodies.

Usage: 

```C#
href="@Url.MailerAbsoluteAction("Index","Home")"

```

This will generate an absolute url to the action `Index` of Controller `Home`.
See  https://github.com/nfMalde/AspNetCore.MailKitMailer/blob/main/src/AspNetCore.MailKitMailer/Helper/UrlHelper.cs for more info.



#### Examples 
There is an fully working example .net core project located at https://github.com/nfMalde/AspNetCore.MailKitMailer/tree/main/examples/MailKitMailerExample.
Feel free to download it and play arround.
Also this project got a few integration tests where you can see all different type of usages.
 

## Contribute / Donations
If you got any Ideas to improve my projects feel free to send an pull request. 

If you like my work and want to support me (or want to buy me a coffee/beer) paypal donation are more than appreciated.

 [![Paypal Donate](https://www.paypalobjects.com/en_US/DK/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=SVZHLRTQ6H4VL)


## Changelog

Version | Changes 
------------ | ------------
2.0.2 | Fixed a bug where the memory cache for the CSS Inline Style Helper returned empty string or null instead of reading the actual file.
2.0.1 | Added "GetContentAsync" Method to render the HTML without sending the mail
2.0.0 | Upgraded to .net8 
1.2.2 | Updated Dependecies for .NET 6
1.1.1 | Updated Changed Log for 1.1.x
1.1.0 | Initial .NET 5 Release
1.0.2 | Updated Examples
1.0.1 | Updated Docs
1.0.0 | Initial Release
