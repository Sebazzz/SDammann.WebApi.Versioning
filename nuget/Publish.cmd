@echo off
NuGet Pack SDammann.WebAPI.Versioning.nuspec
Copy /B *.nupkg SDammann.WebAPI.Versioning.nupkg
NuGet Push SDammann.WebAPI.Versioning.nupkg
Erase *.nupkg
Pause