try {
   . ".\common.ps1"
    # Delete existing nupkg files
    del *.nupkg

    # Rebuild all solutions
    foreach($solution in $solutions) {
        $solutionFolder = Join-Path $rootFolder $solution
        Set-Location $solutionFolder
        dotnet restore
    }

    # Create all packages
    $i = 0
    $projectsCount = $projects.length
    Write-Info "�� $projectsCount ����Ŀ������ dotnet pack"

    foreach($project in $projects) {
        $i += 1
        $projectFolder = Join-Path $rootFolder $project
	    $projectName = ($project -split '/')[-1]
		
	    # Create nuget pack
        Write-Info "[$i / $projectsCount] - ���ڴ����Ŀ: $projectName"
	    Set-Location $projectFolder

        #dotnet clean
       dotnet pack -c Release --no-build -- /maxcpucount

        if (-Not $?) {
            Write-Error "��Ŀ: $projectName ���ʧ��" 
            exit $LASTEXITCODE
        }
    
        # Move nuget package
        $projectName = $project.Substring($project.LastIndexOf("/") + 1)
        $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
        Move-Item -Force $projectPackPath $packFolder
	
	    Seperator
    }

    # Go back to the pack folder
    Set-Location $packFolder

} catch {
    Write-Host "��������: $_" -ForegroundColor Red
    Pause
}
