$ErrorActionPreference = 'Stop'

$targetDir = $PSScriptRoot

$arch = '32'
if ([Environment]::Is64BitOperatingSystem) {
    $arch = '64'
}

$release = Invoke-RestMethod -Uri 'https://api.github.com/repos/bogdanfinn/tls-client/releases/latest'

$asset = $release.assets | Where-Object { $_.name -like "*windows-$arch-*.dll" } | Select-Object -First 1

if (-not $asset) {
    throw "Asset not found"
}

$version = ($asset.name -replace '.*-(\d+\.\d+\.\d+)\.dll', '$1')
$targetFile = Join-Path $targetDir 'tls-client.dll'
$versionFile = Join-Path $targetDir 'tls-client-version.txt'

if (Test-Path $versionFile) {
    $currentVersion = Get-Content $versionFile
    if ($currentVersion -eq $version) {
        exit 0
    }
}

if (Test-Path $targetFile) {
    Remove-Item $targetFile -Force
}

Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $targetFile
[System.IO.File]::WriteAllText($versionFile, $version)