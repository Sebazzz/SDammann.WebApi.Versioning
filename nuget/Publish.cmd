@echo off
NuGet Pack SDammann.WebAPI.Versioning.nuspec
Copy /B *.nupkg SDammann.WebAPI.Versioning.nupkg
Pause
NuGet Push SDammann.WebAPI.Versioning.nupkg
Erase *.nupkg
Pause