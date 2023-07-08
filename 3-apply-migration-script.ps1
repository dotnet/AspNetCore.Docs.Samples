# Get the output variables from terraform
$serverName = & terraform -chdir=terraform output -raw sqlserver_name
$resourceGroup = & terraform -chdir=terraform output -raw sqlserver_rg
$adminUsername = & terraform -chdir=terraform output -raw sqlserver_username
$databaseName = & terraform -chdir=terraform output -raw sqlserver_db
$adminPassword = & terraform -chdir=terraform output -raw sqlserver_password

# Construct the fully qualified server name
$serverUrl = $serverName + ".database.windows.net"

# Define the path of your SQL script file
$sqlFilePath = Join-Path (Get-Location).Path "migrate.sql"

# Check if the SQL file exists
if(-not (Test-Path $sqlFilePath)) {
    Write-Error "SQL file does not exist: $sqlFilePath"
    exit 1
}

# Get the public IP address of the current machine
$publicIp = Invoke-RestMethod http://ifconfig.me/ip
Write-Output "Public IP: $publicIp"

# Create a unique firewall rule name
$firewallRuleName = "AllowMyIP_" + (Get-Date -Format FileDateTimeUniversal)

# Add firewall rule to allow current public IP
az sql server firewall-rule create --name $firewallRuleName --resource-group $resourceGroup --server $serverName --start-ip-address $publicIp --end-ip-address $publicIp

# Run the SQL script using sqlcmd utility
sqlcmd -S $serverUrl -d $databaseName -U $adminUsername -P $adminPassword -i $sqlFilePath

# Remove the firewall rule
az sql server firewall-rule delete --name $firewallRuleName --resource-group $resourceGroup --server $serverName
