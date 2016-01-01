Push-Location "..\src\SimpleConfigReader\"

nuget pack SimpleConfigReader.csproj -Prop Configuration=Release -Symbols

Move-Item *.nupkg ..\..\nuget\ -Force

Pop-Location