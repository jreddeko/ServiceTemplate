
// run this command to install the new service
$acl = Get-Acl "{EXE PATH}"
$aclRuleArgs = {DOMAIN OR COMPUTER NAME\USER}, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl "{EXE PATH}"

New-Service -Name {SERVICE NAME} -BinaryPathName {EXE FILE PATH} -Credential {DOMAIN OR COMPUTER NAME\USER} -Description "{DESCRIPTION}" -DisplayName "{DISPLAY NAME}" -StartupType Automatic

**** DONT FORGET TO SETUP RECOVERY OPTIONS IN THE SERVICE PROPERTIES ****

// Start a service with the following PowerShell 6 command:
Start-Service -Name {SERVICE NAME}

// Stop a service with the following Powershell 6 command:
Stop-Service -Name {SERVICE NAME}

// After a short delay to stop a service, remove a service with the following Powershell 6 command:
Remove-Service -Name {SERVICE NAME}