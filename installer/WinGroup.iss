#ifndef MyAppName
  #define MyAppName "WinGroup"
#endif

#ifndef MyAppVersion
  #define MyAppVersion "0.1.0"
#endif

#ifndef MyAppPublisher
  #define MyAppPublisher "WinGroup"
#endif

#ifndef MyOutputBaseFilename
  #define MyOutputBaseFilename "WinGroup-setup"
#endif

#ifndef MyOutputDir
  #define MyOutputDir "..\\dist"
#endif

[Setup]
AppId={{A0E5202D-2450-46AB-A75E-FF6EB3FC9B9A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin
OutputDir={#MyOutputDir}
OutputBaseFilename={#MyOutputBaseFilename}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
SetupIconFile=..\src\app.ico
UninstallDisplayIcon={app}\WinGroup.exe

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"; Flags: unchecked

[Files]
Source: "..\publish\win-x64\WinGroup.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\WinGroup.exe"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\WinGroup.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\WinGroup.exe"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent
