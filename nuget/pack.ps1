Push-Location "..\src\SimpleConfigReader\"

nuget pack SimpleConfigReader.csproj -Prop Configuration=Release -Symbols

Move-Item *.nupkg ..\..\nuget\ -Force

Pop-Location

Write-Host "Press any key to continue ..."

$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
