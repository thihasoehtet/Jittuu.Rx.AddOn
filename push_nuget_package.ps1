Param(
  [parameter(Mandatory = $true)]
  [string]$version,
  
  [parameter(Mandatory = $true)]
  [string]$apiKey
    )

function Push() {
    &".nuget\NuGet.exe" push "Jittuu.Rx.AddOn.$version.nupkg" $apiKey
    &".nuget\NuGet.exe" push "Jittuu.Rx.AddOn.$version.symbols.nupkg" $apiKey
}

Push