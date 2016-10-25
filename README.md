Everything in this video applies except obfuscation, which has been removed.

<a href="http://www.youtube.com/watch?feature=player_embedded&v=r3eGK-dSYuw" target="_blank"><img src="http://img.youtube.com/vi/r3eGK-dSYuw/0.jpg" alt="BISM Normalizer Internals" border="10" /></a>

### COMMAND TO PERFORM A RELEASE BUILD

Output goes to bin\ReleaseObfusc

`msbuild BismNormalizer.csproj /verbosity:m /target:Rebuild /property:Configuration=Release`

### SET UP NEW DEVELOPMENT MACHINE

Requires VS SDK (comes with installer for VS 2015). See _Installing SDK from Solution_ section [here](https://msdn.microsoft.com/en-us/library/mt683786.aspx) for more info.

May need to temporarily comment out following at bottom of BismNormalizer.csproj to load project into VS for 1st time. After 1st successful load, add it back.

`Import Project="..\packages\MSBuild.Extension.Pack.1.8.0\build\net40\MSBuild.Extension.Pack.targets"`

Do a Release build from the command-line to set up cross project references for the 1st time (see command above).
Set BismNormalizer as startup project, and in project properities > Debug tab, set
* Start External Program: C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
* Command Line Arguments: /rootsuffix Exp
