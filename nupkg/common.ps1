# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
Write-Host "packFolderÂ·¾¶: $packFolder"

$rootFolder = Join-Path $packFolder "../"
Write-Host "rootFolderÂ·¾¶: $rootFolder" 

$frameworkFolder = Join-Path $rootFolder "framework"
Write-Host "frameworkÂ·¾¶: $frameworkFolder" 

function Write-Info   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Black -BackgroundColor Green

	try 
	{
	   $host.UI.RawUI.WindowTitle = $text
	}		
	catch 
	{
		#Changing window title is not suppoerted!
	}
}

function Write-Error   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Red -BackgroundColor Black 
}

function Seperator   
{
	Write-Host ("_" * 100)  -ForegroundColor gray 
}

function Get-Current-Version { 
	$commonPropsFilePath = resolve-path "../common.props"
	$commonPropsXmlCurrent = [xml](Get-Content $commonPropsFilePath ) 
	$currentVersion = $commonPropsXmlCurrent.Project.PropertyGroup.Version.Trim()
	return $currentVersion
}

function Get-Current-Branch {
	return git branch --show-current
}	   

function Read-File {
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $filePath
    )
		
	$pathExists = Test-Path -Path $filePath -PathType Leaf
	if ($pathExists)
	{
		return Get-Content $filePath		
	}
	else{
		Write-Error  "$filePath path does not exist!"
	}
}

# List of solutions
$solutions = (
    "framework"
)

# List of projects
$projects = (
    # framework
    "framework/src/Starshine.Abp.AspNetCore",
    "framework/src/Starshine.Abp.Caching.FreeRedis",
    "framework/src/Starshine.Abp.Core",
    "framework/src/Starshine.Abp.Mapster",
    "framework/src/Starshine.Abp.SqlSugarCore",
	"framework/src/Starshine.Abp.Swashbuckle"
)
