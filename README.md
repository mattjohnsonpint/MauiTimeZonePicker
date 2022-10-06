# MauiTimeZonePicker
Time Zone Picker control for .NET MAUI

[![NuGet](https://img.shields.io/nuget/v/mjp.MauiTimeZonePicker)](https://www.nuget.org/packages/mjp.MauiTimeZonePicker)
[![MIT Licensed](https://img.shields.io/github/license/mattjohnsonpint/MauiTimeZonePicker)](https://github.com/mattjohnsonpint/MauiTimeZonePicker/blob/main/LICENSE)

Read all about it:
https://blog.sentry.io/2022/07/19/making-a-time-zone-picker-control-for-net-maui

## Installation

```shell
dotnet add package mjp.MauiTimeZonePicker --prerelease
```

## Example Usage

- In your XAML where you want to use the control, add the namespace:

    ```
    xmlns:mtzp="clr-namespace:MauiTimeZonePicker;assembly=MauiTimeZonePicker"
    ```

- Then add the control.  Give the instance a name if desired.  Bind to the `SelectedItemChanged` event to respond to selection changes.
    ```xml
    <mtzp:TimeZonePicker
        x:Name="TimeZonePicker"
        SelectedItemChanged="TimeZoneChanged" />
    ```

Review the application in the [/sample](sample) directory for futher details. 

## Screenshots

### Android
![Android Screenshot](images/screenshot-android.png)

### iOS
![iOS Screenshot](images/screenshot-ios.png)

### macOS (Mac Catalyst)
![macOS Screenshot](images/screenshot-mac.png)

### Windows
![Windows Screenshot](images/screenshot-windows.png)
