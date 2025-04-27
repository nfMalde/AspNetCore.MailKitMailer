# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
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