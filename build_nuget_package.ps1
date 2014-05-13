function BuildSolution() {
    $v4_net_version = (ls "$env:windir\Microsoft.NET\Framework\v4.0*").Name    
    
    &"C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" ".\Jittuu.Rx.AddOn.sln" /p:Configuration=Release
}

function CreatePackage() {
    &".nuget\NuGet.exe" pack ".\Jittuu.Rx.AddOn\Jittuu.Rx.AddOn.csproj" -Prop Configuration=Release -Symbols
}

BuildSolution
CreatePackage