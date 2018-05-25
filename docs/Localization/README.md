# Globalization and Localization

When you develop an application for Windows you want to focus on the widest market of users that could benefit from it. So, it is important that your application functions appropriately in different combinations of languages, regions and cultures.

On top of that, you want to design your application in such a way it can be easily adapted to new languages and countries without requiring to re-write or modify its executable code. That is why you should always develop your applications thinking in Globalization and Localization.

Let´s start introducing the concepts of Globalization, Localizability and Localization.

- **Globalization** is the process of designing and developing your app in such a way that it functions appropriately in different global markets without requiring culture-specific changes or customization.

- **Localizability** is the process of preparing a globalized app for localization, and verifying that the app is ready for localization. Correctly making an app localizable means that the later localization process will not uncover any functional defects in the app. The most essential property of a localizable app is that its executable code has been cleanly separated from the app's localizable resources.

- **Localization** is the process of adapting or translating your app's localizable resources to meet the language, cultural, and political requirements of the specific local markets that the app is intended to support. If your app is accurately localizable then you will not have to modify any logic during this process. The localization of an application usually involves the translation of the string resources, the redesign of any culture-dependent images as necessary, and the modification of any other resource files culture or region dependant.

The first step before you start developing a globalized app is to think ahead and identify which elements of your application will be subject to modifications due to the adaptation to different languages, regions and cultures. Avoid assumptions in your code about language, region, character classification, writing system, date/time formatting, numbers, currencies, weights, and sorting rules. Try always to make use of the Globalization APIs to format these data if possible.
You can find an overview of general rules to make your app localizable in this article: [Make your app localizable](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/prepare-your-app-for-localization).

## Overview of the Globalization APIs

Windows provide a set of Windows Runtime (WinRT) APIs to support you in the process of globalization of your UWP app development. You can find a preview of the classes included in Windows.Globalization namespace in this link [Windows.Globalization Namespace](https://docs.microsoft.com/en-us/uwp/api/windows.globalization). We will discuss some of them in more detail in this chapter.

You may also want to format the information that your app displays to the user based on his particular preferences, region, calendar, currencies or languages. We can check these preferences using the [GlobalizationPreferences](https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile.globalizationpreferences?branch=live) from the [Windows.System.UserProfile](https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile) namespace that holds various user globalization preferences.

## The User-Profile, App-Manifest and Runtime Language Lists

There are three language lists you have to deal with.


**1) User profile language list**

This is an ordered list of the preferred language or languages that the user has configured in his system, using *Settings > Time & Language > Region & language*. It is independent from your app, but you can use the [GlobalizationPreferences.Languages](https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile.globalizationpreferences.Languages) property to access the user profile language list as a read-only list of strings, where each string is a single BCP-47 language tag such as "en-US".

```c#
IReadOnlyList<string> userLanguageList = Windows.System.UserProfile.GlobalizationPreferences.Languages;
```
**2) App manifest language list**

This is the list of languages for which your app declares support. Note that this list can grow as you progress your app through the development lifecycle all the way to localization.

The App manifest list is determined at compile time. By default, your app package manifest source file (Package.appxmanifest) contains this configuration:

```xml
  <Resources>
    <Resource Language="x-generate" />
  </Resources>
```
Each time Visual Studio produces your built app package manifest file it expands that single Resource element into a union of all the language qualifiers that it finds in your project (see [Tailor your resources for language, scale, high contrast, and other qualifiers](https://docs.microsoft.com/en-us/windows/uwp/app-resources/tailor-resources-lang-scale-contrast)). For example, if you've begun localizing and you have string, image, and/or file resources whose folder or file names include "en-US", "ja-JP", and "fr-FR", then your built AppxManifest.xml file will contain the following (the first entry in the list is the default language that you set).

```xml
   <Resources>
    <Resource Language="EN-US" />
    <Resource Language="JA-JP" />
    <Resource Language="FR-FR" />
  </Resources>
```
The other option is to replace that single "x-generate" <Resource> element in your app package manifest source file (Package.appxmanifest) with the expanded list of <Resource> elements (being careful to list the default language first). That option involves more maintenance work for you, but it might be an appropriate option for you if you use a custom build system.

When your app is in the **Microsoft Store**, the languages in the app manifest language list are the ones that are displayed to customers. For a list of BCP-47 language tags specifically supported by the Microsoft Store, see [Supported languages](https://docs.microsoft.com/en-us/windows/uwp/publish/supported-languages).

In code you can use the ApplicationLanguages.ManifestLanguages property to access the app manifest language list as a read-only list of strings, where each string is a single BCP-47 language tag (see [ApplicationLanguages Class](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.applicationlanguages) for more information).

```c#
IReadOnlyList<string> manifestLanguageList = Windows.Globalization.ApplicationLanguages.ManifestLanguages;
```
**3) App runtime language list**

The third language list of interest is the intersection between the two lists that we've just described. At runtime, the list of languages for which your app has declared support (the app manifest language list) is compared with the list of languages for which the user has declared a preference (the user profile language list). The app runtime language list is set to this intersection (if the intersection is not empty), or to just the app's default language (if the intersection is empty).

In code you can use the ApplicationLanguages.Languages property to access the ranked app runtime language list as a read-only list of strings, where each string is a single BCP-47 language tag.

```c#
IReadOnlyList<string> runtimeLanguageList = Windows.Globalization.ApplicationLanguages.Languages;
```


## Using string Resources

In your globalized app, you don´t want to have string literals in your code or XAML markup or app package manifest. We are going to move those strings into a Resources File (.resw), and replace the hardcoded string literals with references to resource identifiers. Now you can make a translated copy of that Resources File for each language that your app supports.

You keep your string resources in a Resources File (.resw), and you typically create this kind of resource file in a \Strings folder in your project (see [Tailor your resources for language, scale, and other qualifiers for a background on how to use qualifiers](https://docs.microsoft.com/en-us/windows/uwp/app-resources/tailor-resources-lang-scale-contrast)).

Inside the \Strings folder you will create subfolders for each language that your app supports. Note that you need to provide, at least, string resources localized for the default language of the application defined by the Package.appxmanifest. Those are the resources that will be loaded if no better match can be found between the user profile language list and the app manifest language list.

Here is a view of the Solution Explorer from an app with string resources files for localization in English (US), Spanish (Spain) and French (France): 

![localization](img/localization_01.png)

In each of the Resources.resw files we are going to define the string literals of our app, by assigning a Name, a Value and a Comment.


![localization](img/localization_02.png)

In the example above, "Tittle" and “Description” are string resource identifiers that you can refer to from your markup. For the identifier "Tittle", we are providing two strings: "Tittle.Text" and "Tittle.Width". Those are property identifiers because they correspond to a property of a UI element. The "Description" identifier is a simple string resource identifier; it has no sub-properties and it can be loaded from imperative code, as we'll show.

You cannot have a simple string identifier and property identifiers for the same identifier. If we add a “Description.Text” in the example shown, it will cause a Duplicate Entry error when building Resources.resw.

The Comment column is a good place to provide any special instructions to translators, including any disambiguation that you want to point out. 
Resource identifiers are case insensitive, and must be unique per resource file. Be sure to use meaningful resource identifiers to provide additional context for translators. 

**Refer to a string resource identifier from XAML**

To refer to a string resource in your XAML code, we´ll use an x:Uid directive to associate a control or other element in your markup with a string resource identifier.

```xml
<TextBlock x:Uid="Tittle"/>
```

The x:Uid directive on the TextBlock causes a lookup to take place, to find property identifiers inside Resources.resw that contain the string resource identifier "Tittle". The "Tittle.Text" and "Tittle.Width" property identifiers are found and their values are applied to the TextBlock, overriding any values set locally in the markup. But only property identifiers are used to set properties on XAML markup elements, so setting x:Uid to "Description" on this TextBlock would have no effect (Resources.resw does contain the string resource identifier "Description", but it contains no property identifiers for it).

When assigning a string resource identifier to a XAML element, be certain that all the property identifiers for that identifier are appropriate for the XAML element. For example, if you set x:Uid="Tittle" on a TextBlock then "Tittle.Text" will resolve because the TextBlock type has a Text property. But if you set x:Uid="Tittle" on a Button then "Tittle.Text" will cause a run-time error because the Button type does not have a Text property. One solution for that case is to author a property identifier named "ButtonTittle.Content", and set x:Uid="ButtonTittle" on the Button.

**Refer to a string resource identifier from code**

You can explicitly load a string resource based on a simple string resource identifier (as the “Description” identifier in the above example, without a property attached).

```c#
var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
this.myXAMLTextBlock.Text = resourceLoader.GetString("Description");
```

You can only load the value for a simple string resource identifier this way, not for a property identifier. So we can load the value for "Description" using code like this, but we cannot do so for "Tittle.Text". Trying to do so will return an empty string.

**Refer to a string resource identifier from your app package manifest**

By default, the app's Display name on your app package manifest (the Package.appxmanifest file), is expressed as a string literal. 
 
To make a localizable version of this string, open Resources.resw and add a new string resource with the name "AppDisplayName" and the value "Localizable Application Example".
Then, replace the Display name string literal with a reference to the string resource identifier that you just created ("AppDisplayName"). You use the ms-resource URI (Uniform Resource Identifier) scheme to do this.
 
![localization](img/localization_03.png)

To make a localizable version of this string, open Resources.resw and add a new string resource with the name "AppDisplayName" and the value "Localizable Application Example".

Then, replace the Display name string literal with a reference to the string resource identifier that you just created ("AppDisplayName"). You use the ms-resource URI (Uniform Resource Identifier) scheme to do this.

![localization](img/localization_04.png)

For a list of all items in the app package manifest that you can localize, see [Localizable manifest items](https://docs.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/localizable-manifest-items-win10?branch=live).

**Localize the string resources**

Now that you have stored your string literals into resources, to localize them for another language you only have to create another folder under /Strings, name it with the correspondent qualifiers for the new language, and place there a new Resources.resw file with the translated values of your strings. Here is an example in Spanish (Spain) for the resource file shown before. (Remember that you don’t have to translate the Comment field).

![localization](img/localization_05.png)

## The user´s Geographic Region and Cultural Preferences

The user can specify their location in the world by going to *Settings>Time & Language>Region & language > Country or region*. You should use these settings, instead of language, for choosing what content to display to the user, for example, if you wish to show a map or weather information.

You can use the [Windows.System.UserProfile.GlobalizationPreferences Class](https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile.globalizationpreferences?branch=live) to get the value of the user's current geographic region, preferred currencies, preferred languages, and so on. For example, you can access the region defined by the user by using the [GlobalizationPreferences.HomeGeographicRegion](https://docs.microsoft.com/en-us/uwp/api/windows.system.userprofile.globalizationpreferences.homegeographicregion#Windows_System_UserProfile_GlobalizationPreferences_HomeGeographicRegion) property.

```c#
// Obtain the user's home geographic region.
var region = Windows.System.UserProfile.GlobalizationPreferences.HomeGeographicRegion;
```

You can also use the [GeographicRegion](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.geographicregion) class of the Windows.Globalization namespace, and inspect details about a particular region, such as its display name, native name, and currencies in use in that region. 

```c#
// Get the user's geographic region and its display name, native name and currencies in use.
var geographicRegion = new Windows.Globalization.GeographicRegion();
var displayName = geographicRegion.DisplayName;
var nativeName = geographicRegion.NativeName;
var currenciesInUse = geographicRegion.CurrenciesInUse;
```

## Date and Time Globalization

The representation of dates and times may vary greatly between countries and cultures. We are going to show how to handle these representations and formats in this section.

Different regions and cultures use different date and time formats. These include conventions for the order of day and month in the date, for the separation of hours and minutes in the time, and even for what punctuation is used as a separator. In addition, dates may be displayed in various formats and, of course, the names and abbreviations for the days of the week and months of the year differ between languages.

**Format dates and times for the app runtime language list**

If you need to allow users to choose a date, or to select a time, then use the standard [calendar, date, and time controls](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/date-and-time). These automatically use the best date and time format for the app runtime language list.

If you need to display dates or times yourself then you can use the [DateTimeFormatter](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.datetimeformatting.datetimeformatter) class from the [Windows.Globalization.DateTimeFormatting](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.datetimeformatting?branch=live) namespace. By default, DateTimeFormatter automatically uses the best date and time format for the app runtime language list. So, the code below formats a given DateTime in the best way for that list. As an example, assume that your app manifest language list includes English (United States), which is also your default, and German (Germany). If the current date is Mar 30 2018 and the user profile language list contains German (Germany) first, then the formatter gives "30.03.2018" (German format). If the user profile language list contains English (United States) first (or if it contains neither English nor German), then the formatter gives "03/30/2018" (since "en-US" matches, or is used as the default).

```c#
    // Use the DateTimeFormatter class to display dates and times using basic formatters.

    var shortDateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("shortdate");
    var shortTimeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("shorttime");

    var dateTimeToFormat = DateTime.Now;

    var shortDate = shortDateFormatter.Format(dateTimeToFormat);
    var shortTime = shortTimeFormatter.Format(dateTimeToFormat);

    var results = "Short Date: " + shortDate + "\n" + "Short Time: " + shortTime;
```
**Format dates and times for other languages**

Remember that, by default, DateTimeFormatter matches the app runtime language list. That way, if you display strings such as "The date is \<date>", then the language will match the date format.

If for whatever reason you want to format dates and/or times to another language, for example, for the user profile language list, you can do that using code like the example below.

```c#
    // Use the DateTimeFormatter class to display dates and times using basic formatters.
    var userLanguages = Windows.System.UserProfile.GlobalizationPreferences.Languages;
    var shortDateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("shortdate", userLanguages);
    var results = "Short Date: " + shortDateFormatter.Format(DateTime.Now);
```

But remember that the user can choose a language for which your app doesn't have translated strings. For example, if your app is not localized into German (Germany), but the user chooses that as their preferred language, then that could result in the display of arguably odd-looking strings such as "The date is 03.30.2017".

**Use templates and patterns to format dates and times**

As we have seen, the DateTimeFormatter class provides various ways of formatting time and hours. But if you want even more control over the order and format of the components of the DateTime object you wish to display, you can pass a format pattern to the formatTemplate argument of the constructor. A format pattern uses a special syntax, which allows you to obtain individual components of a DateTime object—just the month name, or just the year value, for example—in order to display them in whatever custom format you choose. Furthermore, the pattern can be localized to adapt to other languages and regions. For a more complete discussion of format templates and format patterns see the Remarks section of the [DateTimeFormatter](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.datetimeformatting.datetimeformatter) class.

For more information about the use of DateTimeFormatter class to display dates and times in exactly the format you wish, see [Use templates and patterns to format dates and times](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/use-patterns-to-format-dates-and-times).

**Use a culturally appropriate calendar**

The calendar differs across regions and languages. The Gregorian calendar is not the default for every region. Users in some regions may choose alternate calendars, such as the Japanese era calendar, or Arabic lunar calendars. Dates and times on the calendar are also sensitive to different time zones and daylight-saving time.

To ensure that the preferred calendar format is used, you can use the standard [calendar, date, and time controls](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/date-and-time). For more complex scenarios, where working directly with operations on calendar dates may be required, Windows.Globalization provides a [Calendar](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.calendar?branch=live) class that gives an appropriate calendar representation for the given culture, region, and calendar type.

## Format numbers and currencies appropriately

Different cultures format numbers differently. Format differences may include how many decimal digits to display, what characters to use as decimal separators, and what currency symbol to use. Use classes in the [NumberFormatting](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.numberformatting?branch=live) namespace to display decimal, percent, or permille numbers, and currencies. Most of the time, you will want these formatter classes to use the best format for the user profile. But you may use the formatters to display a currency for any region or format. This example shows how to display currencies both per the user profile, and for a specific given currency system.

```c#
// This scenario uses the CurrencyFormatter class to format a number as a currency.

    var userCurrency = Windows.System.UserProfile.GlobalizationPreferences.Currencies[0];

    var valueToBeFormatted = 12345.67;

    var userCurrencyFormatter = new Windows.Globalization.NumberFormatting.CurrencyFormatter(userCurrency);
    var userCurrencyValue = userCurrencyFormatter.Format(valueToBeFormatted);

    // Create a formatter initialized to a specific currency,
    // in this case US Dollar (specified as an ISO 4217 code) 
    // but with the default number formatting for the current user.
    var currencyFormatUSD = new Windows.Globalization.NumberFormatting.CurrencyFormatter("USD");
    var currencyValueUSD = currencyFormatUSD.Format(valueToBeFormatted);

    // Create a formatter initialized to a specific currency.
    // In this case it's the Euro with the default number formatting for France.
    var currencyFormatEuroFR = new Windows.Globalization.NumberFormatting.CurrencyFormatter("EUR", new[] { "fr-FR" }, "FR");
    var currencyValueEuroFR = currencyFormatEuroFR.Format(valueToBeFormatted);

    // Results for display.
    var results = "Fixed number (" + valueToBeFormatted + ")\n" +
                    "With user's default currency: " + userCurrencyValue + "\n" +
                    "Formatted US Dollar: " + currencyValueUSD + "\n" +
                    "Formatted Euro (fr-FR defaults): " + currencyValueEuroFR;
```

## Format phone numbers appropriately

Phone numbers are formatted differently across regions. The number of digits, how the digits are grouped, and the significance of certain parts of the phone number vary between countries. Starting in Windows 10, version 1607, you can use classes in the [PhoneNumberFormatting](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.phonenumberformatting?branch=live) namespace to format phone numbers appropriately for the current region.

[PhoneNumberInfo](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.phonenumberformatting.phonenumberinfo) parses a string of digits and allows you to: determine whether the digits are a valid phone number in the current region; compare two numbers for equality; and to extract the different functional parts of the phone number, such as country code or geographical area code.

[PhoneNumberFormatter](https://docs.microsoft.com/en-us/uwp/api/windows.globalization.phonenumberformatting.phonenumberformatter) formats a string of digits or a PhoneNumberInfo for display, even when the string of digits represents a partial phone number. You can use this partial number formatting to format a number as a user is entering the number.

The example below shows how to use PhoneNumberFormatter to format a phone number as it is being entered. Each time text changes in a TextBox named phoneNumberInputTextBox, the contents of the text box are formatted using the current default region and displayed in a TextBlock named phoneNumberOutputTextBlock. For demonstration purposes, the string is also formatted using the region for New Zealand, and displayed in a TextBlock named phoneNumberOutputTextBlockNZ.

```c#
    using Windows.Globalization.PhoneNumberFormatting;

    PhoneNumberFormatter currentFormatter, NZFormatter;

    public MainPage()
    {
        this.InitializeComponent();

        // Use the default formatter for the current region
        this.currentFormatter = new PhoneNumberFormatter();

        // Create an explicit formatter for New Zealand. 
        PhoneNumberFormatter.TryCreate("NZ", out this.NZFormatter);
    }

    private void phoneNumberInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Format for the default region.
        this.phoneNumberOutputTextBlock.Text = currentFormatter.FormatPartialString(this.phoneNumberInputTextBox.Text);

        // If the NZFormatter was created successfully, format the partial string for the NZ TextBlock.
        if(this.NZFormatter != null)
        {
            this.phoneNumberOutputTextBlockNZ.Text = this.NZFormatter.FormatPartialString(this.phoneNumberInputTextBox.Text);
        }
    }
```
## The User Interface (UI) Globalization

When you are developing your globalized application, you have to design your UI to support the layouts and fonts of multiple languages and cultures, including RTL (right-to-left) flow direction. Consider the flow direction not only as the direction in which scripts are written and displayed, but also the direction in which the user scans UI elements on the screen by the eye.

In this section we are going to show some considerations to take in mind when designing an UI to support multiple languages and cultures, and tips to handle the RTL support.

**Layout guidelines.**

Some languages use more characters than English does. Far Eastern fonts typically require more height. And languages such as Arabic and Hebrew require that layout panels and text elements be laid out in right-to-left (RTL) reading order.

-	Use dynamic UI layout to adjust to different language lengths.
-	Use the FrameworkElement.FlowDirection property form the [FrameworkElement](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.frameworkelement) class for RTL languages, and set symmetrical padding and margins. See the article [Best practices for handling right-to-left (RTL) languages](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/adjust-layout-and-fonts--and-support-rtl#best-practices-for-handling-right-to-left-rtl-languages). You can learn more about designing your application to provide bidirectional text support (BiDi) in this article: [Design your app for bidirectional text](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/design-for-bidi-text).
-	In general you should avoid setting absolute layout values on any UI element based on language. But if it's absolutely unavoidable, then you can create a property identifier of the form "TitleText.Width", and modify his value in the different languages-cultures resources. 

## Fonts

Use the [LanguageFont](https://docs.microsoft.com/en-us/uwp/api/Windows.Globalization.Fonts.LanguageFont?branch=live) font-mapping class for programmatic access to the recommended font family, size, weight, and style for a particular language. A LanguageFont object provides data giving a font recommendation for a particular language and for various categories of content including UI headers, notifications, body text, and user-editable document body fonts.
In this example we get the recommended Japanese fonts for traditional and modern documents.

```c#
// Get the recommended Japanese fonts for traditional documents and modern documents.
var fonts = new Windows.Globalization.Fonts.LanguageFontGroup("ja-JP");
var traditionalDocumentFont = fonts.TraditionalDocumentFont;
var modernDocumentFont = fonts.ModernDocumentFont;
```

In [International fonts](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/loc-international-fonts) you can see a list of the fonts available for UWP apps that are localized into languages other than U.S. English.

## Multilingual App Toolkit 4.0.

The Multilingual App Toolkit (MAT) 4.0 integrates with Microsoft Visual Studio 2017 to provide UWP apps with translation support, translation file management, and editor tools. Here are some of the value propositions of the toolkit.
-	Helps you manage resource changes and translation status during development.
-	Provides a UI for choosing languages based on configured translation providers.
-	Supports the localization industry-standard XLIFF file format.
-	Provides a pseudo-language engine to help identify translation issues during development.
-	Connects with the Microsoft Language Portal to easily access translated strings and terminology.
-	Connects with the Microsoft Translator for quick translation suggestions.

To learn more about how to use, download and install the Multilingual App Toolkit 4.0, read the article [Use the Multilingual App Toolkit 4.0](https://docs.microsoft.com/en-us/windows/uwp/design/globalizing/use-mat).


