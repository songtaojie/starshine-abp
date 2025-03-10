. ".\common.ps1"

$apiKey = $args[0]

# Get the version
$commonPropsPath  
[xml]$commonPropsXml = Get-Content (Join-Path $frameworkFolder "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

# Publish all packages
$i = 0
$errorCount = 0
$totalProjectsCount = $projects.length
$nugetUrl = "https://api.nuget.org/v3/index.json"
Set-Location $packFolder

foreach($project in $projects) {
	$i += 1
	$projectFolder = Join-Path $rootFolder $project
	$projectName = ($project -split '/')[-1]
	$nugetPackageName = $projectName + "." + $version + ".nupkg"	
	$nugetPackageExists = Test-Path $nugetPackageName -PathType leaf
 
	Write-Info "[$i / $totalProjectsCount] - 正在推送: $nugetPackageName"
	
	if ($nugetPackageExists)
	{
		dotnet nuget push $nugetPackageName --skip-duplicate -s $nugetUrl --api-key "$apiKey"		
		#Write-Host ("Deleting package from local: " + $nugetPackageName)
		#Remove-Item $nugetPackageName -Force
	}
	else
	{
		Write-Host ("********** 错误包未找到: " + $nugetPackageName) -ForegroundColor red
		$errorCount += 1
		#Exit
	}
	
	Write-Host "--------------------------------------------------------------`r`n"
}

if ($errorCount > 0)
{
	Write-Host ("******* $errorCount error(s) occured *******") -ForegroundColor red
}
