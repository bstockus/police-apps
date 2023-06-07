Remove-Item .\out -Recurse -Force

dotnet clean .\Police.sln


Set-Location Police.Web
npm install
npm run gulp min
dotnet publish .\Police.Web.csproj -c Release -p:UseAppHost=false -o .\..\out