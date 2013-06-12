# The creation of this build script (and associated files) was only possible using the 
# work that was done on the BoxStarter Project on GitHub:
# http://boxstarter.codeplex.com/
# Big thanks to Matt Wrock (@mwrockx} for creating this project, thanks!

$psake.use_exit_on_error = $true
properties {
	$config = 'Debug';
	$nugetExe = "./../lib/NuGet.exe";
	$projectName = "wifi-manager";
	$preversion = '1.0.0-pre13'
}

$private = "This is a private task not meant for external use!";

function get-buildArtifactsDirectory {
	return "." | Resolve-Path | Join-Path -ChildPath "../BuildArtifacts";
}

function get-sourceDirectory {
	return "." | Resolve-Path | Join-Path -ChildPath "../";
}

function create-PackageDirectory( [Parameter(ValueFromPipeline=$true)]$packageDirectory ) {
    process {
        Write-Verbose "checking for package path $packageDirectory...";
        if( !(Test-Path $packageDirectory ) ) {
    		Write-Verbose "creating package directory at $packageDirectory...";
    		mkdir $packageDirectory | Out-Null;
    	}
    }    
}

function remove-PackageDirectory( [Parameter(ValueFromPipeline=$true)]$packageDirectory ) {
	process {
		Write-Verbose "Checking directory at $packageDirectory...";
        if(Test-Path $packageDirectory) {
    		Write-Verbose "Removing directory at $packageDirectory...";
    		Remove-Item $packageDirectory -recurse -force;
    	}
	}
}

Task -Name Default -Depends BuildSolution

# private tasks

Task -Name __VerifyConfiguration -Description $private -Action {
	Assert ( @('Debug', 'Release') -contains $config ) "Unknown configuration, $config; expecting 'Debug' or 'Release'";
}

Task -Name __CreateBuildArtifactsDirectory -Description $private -Action {
	get-buildArtifactsDirectory | create-packageDirectory;
}

Task -Name __RemoveBuildArtifactsDirectory -Description $private -Action {
	get-buildArtifactsDirectory | remove-packageDirectory;
}

# primary targets

Task -Name PackageSolution -Depends RebuildSolution, PackageChocolatey -Description "Complete build, including creation of Chocolatey Package."

# build tasks

Task -Name BuildSolution -Depends __VerifyConfiguration -Description "Builds the main solution for the package" -Action {
	$sourceDirectory = get-sourceDirectory;
	exec { 
		msbuild "$sourceDirectory\WindowsWifiManager.sln" /t:Build /p:Configuration=$config
	}
}

Task -Name RebuildSolution -Depends CleanSolution, __CreateBuildArtifactsDirectory, BuildSolution -Description "Rebuilds the main solution for the package"

# clean tasks

Task -Name CleanSolution -Depends __RemoveBuildArtifactsDirectory, __VerifyConfiguration -Description "Deletes all build artifacts" -Action {
	$sourceDirectory = get-sourceDirectory;
	exec {
		msbuild "$sourceDirectory\WindowsWifiManager.sln" /t:Clean /p:Configuration=$config
	}
}

# package tasks

Task -Name PackageChocolatey -Description "Packs the module and example package" -Action { 
	$sourceDirectory = get-sourceDirectory;
	$buildArtifactsDirectory = get-buildArtifactsDirectory;
    
    exec { 
		.$nugetExe pack "$sourceDirectory\ChocolateyPackage\wifi-manager.nuspec" -OutputDirectory "$buildArtifactsDirectory" -NoPackageAnalysis -version $preversion 
	}
}