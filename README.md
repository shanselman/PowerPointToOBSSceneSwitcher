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


