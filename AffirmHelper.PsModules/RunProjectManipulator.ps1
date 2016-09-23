#
# Script.ps1
#
cls
Write-Host "Loading Module"
Write-Host $($PSScriptRoot)
 [System.Reflection.Assembly]::LoadFrom("$($PSScriptRoot)\CodeManagement.dll")
#Add-Type -Path "C:\projects\AffirmHelper\CodeManagement\bin\Debug\CodeManagement.dll"
$nodeReplacer = New-Object CodeManagement.TargetNodeReplacer

$file = "C:\ADMServer2008\Core\0000_5000_ADMServerCore_7_1_1_1\ADM.Web\ADM.Web.csproj"
try
{
    Write-Host "Attempting to checkout $file"
    tf checkout $file
    $nodeReplacer.ReplaceTargetsWithImports($file)
    Write-Host "Replaced Target Node for : $file"
    #tf status $file | out-file C:\TestFiles\CodeManagement\outputPath\TFS_STATUS_RESULTS.txt $file.FullName -Append
}
catch [System.Exception]
{
    Write-Host "Error found with file : $file"
    #$_.Exception.Message
    #Send-MailMessage -From amckenzie@ipipeline.com -To wennis#ipipeline.com -Subject "TESTING -- SOME EXCEPTION WAS THROWN!!" -SmtpServer mta.ipipeline.us -Body "Hello, I need to make changes on the following project file: $file.  Could you please release the lock on it? \n Thanks, Adam McKenzie"
}