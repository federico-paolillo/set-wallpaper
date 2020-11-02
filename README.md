# Set-Wallpaper

PowerShell cmdlets for wallpaper manipulation on Windows.

## Requirements

Supports PowerShell 5.1+ and PowerShell Core. Works reliably on Windows 10.  
Might support older versions of Windows as long as at PowerShell 5.1+ or PowerShell Core is available.  

**Note:** It is possible that your administrator has blocked changing your desktop background, if that is the case `Set-Wallpaper` might fail.  

## Installation

### PowerShell Gallery

`Install-Module FP.SetWallpaper -AllowPrerelease`

Installing packages from the Gallery requires the latest version of the PowerShellGet module.  
[See Installing PowerShellGet on Microsoft documentation](https://docs.microsoft.com/en-us/powershell/scripting/gallery/installing-psget?view=powershell-7)  

By default, `Install-Module` installs modules to `$env:ProgramFiles\WindowsPowerShell\Modules`, this operation requires an administrator account.  
Specify `-Scope CurrentUser` to install the module in `$env:USERPROFILE\Documents\WindowsPowerShell\Scripts` if you don't have an administrator account.  

### Manually

[Refer to Microsoft documentation](https://docs.microsoft.com/en-us/powershell/scripting/developer/module/installing-a-powershell-module)

## Cmdlets

### Get-Monitor

Returns a list of all the monitors, as `Monitor` objects, available.

#### Parameters

_None_

#### Inputs

_None_

#### Outputs

One or more `Monitor` objects.  
The number of objects returned depends on the number of monitors available to Windows.  

### Get-Wallpaper

Returns the current wallpaper, as a `Wallpaper` object, of the monitors specified.

#### Parameters

`-InputObject`

Specifies one or more `Monitor` objects.  
Enter a variable that contains the objects, or type a command or expression that gets the objects.  
Use either `-InputObject` or `-Id` to specify which monitors to query.  

_Type_: `Monitor[]`  
_Aliases_: None  
_Position_: 0, Named  
_Default value_: None  
_Accept pipeline input_: Yes  
_Accept wildcard parameter_: Nope  

`-Id`

Specifies one or more monitor paths.
Use either `-InputObject` or `-Id` to specify which monitors to query.  

_Type_: `String[]`  
_Aliases_: None  
_Position_: 0, Named  
_Default value_: None  
_Accept pipeline input_: Yes  
_Accept wildcard parameter_: Nope  

#### Inputs

`Monitor[]`

You can pipe a list of `Monitor` objects to this cmdlet.

`String[]`

You can pipe a list of `String` that represent valid monitor ids to this cmdlet. 

#### Outputs

One or more `Wallpaper` objects.  
The number of objects returned depends on the number of monitors queried.  

### Set-Wallpaper

Set a file identified by its path as wallpaper of the monitors specified.  

#### Parameters

`-InputObject`  

Specifies one or more `Monitor` objects.  
Enter a variable that contains the objects, or type a command or expression that gets the objects.  
Use either `-InputObject` or `-Id` to specify which monitors to query.  

_Type_: `Monitor[]`  
_Aliases_: None  
_Position_: 0, Named  
_Default value_: None  
_Accept pipeline input_: Yes  
_Accept wildcard parameter_: Nope  

`-Id`

Specifies one or more monitor paths.
Use either `-InputObject` or `-Id` to specify which monitors to query.  

_Type_: `String[]`  
_Aliases_: None  
_Position_: 0, Named  
_Default value_: None  
_Accept pipeline input_: Yes  
_Accept wildcard parameter_: Nope  

`-Path`

Specifies the path to the desired desktop wallpaper. Wildcard characters are permitted.  
The path must be a path to an items, not a containers. For example: you must specify a file, not folders.  
If more than one item is specified, only the first one found will be used.  

_Type_: `String`  
_Aliases_: None  
_Position_: 1, Named  
_Default value_: None  
_Accept pipeline input_: Nay  
_Accept wildcard parameter_: Yup !  

`-LiteralPath`

Specifies a path to the desired desktop wallpaper.   
The value of LiteralPath is used exactly as it is typed. No characters are interpreted as wildcards.  
If the path includes escape characters, enclose it in single quotation marks. Single quotation marks tell PowerShell not to interpret any characters as escape sequences.  

_Type_: `String`  
_Aliases_: None  
_Position_: 1, Named  
_Default value_: None  
_Accept pipeline input_: Nay  
_Accept wildcard parameter_: Nay  

`-PassThru`

Outputs the `Monitor[]` or `String[]` objects in input.  
By default this cmdlet does not generate any output.  

_Type_: [SwitchParameter](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.switchparameter)  
_Aliases_: None  
_Position_: Named  
_Default value_: None  
_Accept pipeline input_: No  
_Accept wildcard parameter_: Nope  

`-Force`

Sets the wallpaper specified even if it is not an image without asking for confirmation.  
By default, `Set-Wallpaper` stops and asks for confirmation before using a file that does not have an image file extension.  

_Type_: [SwitchParameter](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.switchparameter)  
_Aliases_: None  
_Position_: Named  
_Default value_: None  
_Accept pipeline input_: No  
_Accept wildcard parameter_: No  

`-WhatIf`

Shows what would happen if the cmdlet runs. Does not run the cmdlet.  

_Type_: [SwitchParameter](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.switchparameter)  
_Aliases_: wi  
_Position_: Named  
_Default value_: None  
_Accept pipeline input_: No  
_Accept wildcard parameter_: No  

`-Confirm`

Prompts you for confirmation before running the cmdlet.

_Type_: [SwitchParameter](https://docs.microsoft.com/en-us/dotnet/api/system.management.automation.switchparameter)  
_Aliases_: cf  
_Position_: Named  
_Default value_: False  
_Accept pipeline input_: No  
_Accept wildcard parameter_: No  

`[<CommonParameters>]`

Common parameters like `-Verbose` are supported

#### Inputs

`Monitor[]`  

You can pipe a list of `Monitor` objects to this cmdlet.  

`String[]`  

You can pipe a list of `String` that represent valid monitor ids to this cmdlet.  

#### Outputs

One or more `Wallpaper` objects if you specify `PassThru`, otherwise this cmdlet does not generate any output.  
The number of objects returned depends on the number of monitors queried.  

## Types

### Monitor

Describes a monitor. Contains the monitor `Index` and the `Id`.  

### Wallpaper

Describes a wallpaper set to a particular monitor. Contains the monitor `Id` and the wallpaper `Path`.  

## Examples

Get the wallpaper of every monitor

`Get-Monitor | Get-Wallpaper`

Get the wallpaper of a particular monitor

`Get-Wallpaper '<somemonitorpath>'`

Set the same wallpaper to every monitor

`Get-Monitor | Set-Wallpaper -Path C:\example\wallpaper.png`

## FP.SetWallpaper.COM

Desktop wallpaper manipulation is done through [`IDesktopWallpaper`](https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-idesktopwallpaper) COM interface.  
`FP.SetWallpaper.COM` is the handmade interoperability assembly for `IDesktopWallpaper`.  
This assembly does not expose the full API surface of `IDesktopWallpaper`.  
Only the following methods are available through `FP.SetWallpaper.COM`:  
- `GetMonitorDevicePathCount`
- `GetMonitorDevicePathAt`
- `GetWallpaper`
- `SetWallpaper`
