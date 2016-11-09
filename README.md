### General Usage

Download from the BISM Normalizer 3 [Visual Studio Gallery page](https://visualstudiogallery.msdn.microsoft.com/f7ebe632-878c-4640-b035-a143d1dd1cf3). New releases will be uploaded to this page.

This video covers BISM Normalizer 3 use cases and demo.

[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/LZdOwfJqFrM/0.jpg)](http://www.youtube.com/watch?v=LZdOwfJqFrM)

### Object Model & Build Process

This video shows the internals of BISM Normalizer 3. Everything still applies except obfuscation, which has been removed.

[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/r3eGK-dSYuw/0.jpg)](http://www.youtube.com/watch?v=r3eGK-dSYuw)

### Command to Perform a Release Build

Output goes to bin\ReleaseObfusc

`msbuild BismNormalizer.csproj /verbosity:m /target:Rebuild /property:Configuration=Release`

### Set Up New Development Machine

Requires VS SDK (comes with installer for VS 2015). See _Installing SDK from Solution_ section [here](https://msdn.microsoft.com/en-us/library/mt683786.aspx) for more info.

May need to temporarily comment out following at bottom of BismNormalizer.csproj to load project into VS for 1st time. After 1st successful load, add it back.

`Import Project="..\packages\MSBuild.Extension.Pack.1.8.0\build\net40\MSBuild.Extension.Pack.targets"`

Ensure NuGet packages are installed (should display warning in Package Manager Console).

Do a Release build from the command-line to set up cross project references for the 1st time (see command above).

Set BismNormalizer as startup project, and in project properities > Debug tab, set
* Start External Program: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
* Command Line Arguments: /rootsuffix Exp
