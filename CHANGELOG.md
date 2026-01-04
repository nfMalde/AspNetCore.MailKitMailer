# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.2.1]
### Added
- CID (Content-ID) support for inline/embedded images and resources in HTML emails

#### How to Use Inline Images with CID

**1. Add a linked resource in your mailer method:**
```csharp
public IMailerContextResult WelcomeMailWithLogo(string email, string logoPath)
{
    return this.HtmlMail(
        new EmailAddressModel("User", email),
        "Welcome!",
        new WelcomeModel(),
        withAttachments: a => a.AddLinkedResource(logoPath, "company-logo"));
}
```

**2. Reference the image in your Razor view using `cid:`:**
```html
<img src="cid:company-logo" alt="Logo" />
```

The `AddLinkedResource` method supports file paths, byte arrays, and URLs. See the README for full documentation.

## [2.2.0] 
* Attachments now support byte array and fixed filenames.
* Updated dependencies to latest versions.
* General code improvements and optimizations.
* Added Async Method support for mailer contexts.
* Updated to .NET 10
## [2.1.1]
- Fixed Bug - SMTP connection was not properly disposed after sending an email.

## [2.1.0]
- Updated to .NET 9
- Updated to MailKit 4.11.0
- Updated to Razor 9.0.4
- Updated to HtmlAgilityPack 1.12.1
- Removed Newtonsoft.Json dependency
- Removed obsolete and unused package RestSharp
- Improved nullability annotations

## [2.0.2]
### Fixed
- Resolved a bug where the memory cache for the CSS Inline Style Helper returned empty strings or null instead of reading the actual file.

## [2.0.1]
### Added
- Introduced the `GetContentAsync` method to render the HTML without sending the mail.

## [2.0.0]
### Changed
- Upgraded to .NET 8.

## [1.2.2]
### Updated
- Updated dependencies for .NET 6.

## [1.1.1]
### Updated
- Updated changelog for 1.1.x.

## [1.1.0]
### Added
- Initial .NET 5 release.

## [1.0.2]
### Updated
- Updated examples.

## [1.0.1]
### Updated
- Updated documentation.

## [1.0.0]
### Added
- Initial release.