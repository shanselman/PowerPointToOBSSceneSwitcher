# PowerPointToOBSSceneSwitcher
A .NET core based scene switcher that connects to OBS and changes scenes based note meta data. Put "OBS:Your Scene Name" on its own line in your notes and ensure the OBS Web Sockets Server is running and this app will change your scene as you change your PowerPoint slides.

Note this won't build with "dotnet build," instead open a Visual Studio 2019 Developer Command Prompt and build with "msbuild"

This video explains how it works!

[![Watch the video](https://i.imgur.com/v369AtP.png)](https://www.youtube.com/watch?v=ciNcxi2bPwM)

## Usage
* Set a scene for a slide with 
```<language>
OBS:{Scene name as it appears in OBS}
```

Example:
```<language>
OBS:Scenename
```

* Set a default scene (used when a scene is not defined) with
```<language>
OBSDEF:{Scene name as it appears in OBS}
```

Example:
```<language>
OBSDEF:DefaultScene
```

UPDATE: For many this just clones and builds, but for some folks (unknown why) it doesn't.

Here are some instructions that worked for community member Harold Dickerman. I haven't tested these instructions:

1. Here are the build instructions for PowerPointToOBSSceneSwitcher:
https://visualstudio.microsoft.com/thank-you-downloading-visual-studio-exp/?sku=Community&rel=16

    - .NET Development (Core only, No Optional requirements)
    - Individual Components: C# and Visual Basic Roslyn compiler, MSBUILD

2. Ensure NuGet.org is listed in Visual Studio as a package source, per fix: https://stackoverflow.com/questions/52376567/how-to-resolve-unable-to-find-package-nuget-error 

3. Download and install: https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-3.1.14-windows-x64-installer

4. Download and install: https://github.com/Palakis/obs-websocket/releases/download/4.9.0/obs-websocket-4.9.0-Windows-Installer.exe

5. Download and unzip https://github.com/shanselman/PowerPointToOBSSceneSwitcher/archive/refs/heads/main.zip

6. Double click on PowerPointToOBSSceneSwitcher.sln should open Visual Studio

7. Select Build > Build PowerPointToOBSSceneSwitcher.sln
