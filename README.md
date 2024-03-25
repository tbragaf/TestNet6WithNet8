# Pre requisites
1. Have both .NET 6 and .NET 8 installed.

# Steps to reproduce
1. dotnet restore
2. dotnet build
3. dotnet publish
4. dotnet publish --self-contained
5. cd AppInNet8
6. dotnet run

# Overview
The console app in is .NET 8, whereas the library is in .NET 6.

On the one hand, the console app simply consumes the library as a project reference, and has some code I created (using reflection) to figure out which .NET version the DLLs are using.

On the other hand, the library app consumes a package (System.Reflection for the sake of the example, but doesn't do anything with it).

Once I publish the console app (with dotnet publish), I was expecting that at least the package reference inside the csproj library would be in .NET 6, but that's not the case.
