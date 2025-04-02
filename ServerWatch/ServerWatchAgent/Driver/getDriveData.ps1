param(
    [string]$OutputFilePath
)

$reliabilityData = Get-Disk | Get-StorageReliabilityCounter
$physicalDisks = Get-PhysicalDisk

$combined = foreach ($rel in $reliabilityData) {
    # Extract GUID
    if ($rel.UniqueId -match "^({[a-f0-9\-]+})") {
        $guid = $matches[1]

        # Matching physical disk using GUID
        $phys = $physicalDisks | Where-Object {
            $_.ObjectId -like "*$guid*"
        }

        # Iterate trough the disks and extract data
        foreach ($disk in $phys){
            [PSCustomObject]@{
                GUID              = $guid
                DeviceId          = $rel.DeviceId
                UniqueId          = $rel.UniqueId
                FriendlyName      = $disk.FriendlyName
                SerialNumber      = $disk.SerialNumber
                HealthStatus      = $disk.HealthStatus
                BusType           = $disk.BusType
                MediaType         = $disk.MediaType
                Model             = $disk.Model
                SizeGB            = "{0:N1}" -f ($disk.Size / 1GB)
                Temperature       = $rel.Temperature
                TemperatureMax    = $rel.TemperatureMax
                PowerOnHours      = $rel.PowerOnHours
                WearLevel         = $rel.Wear
                ReadLatencyMax    = $rel.ReadLatencyMax
                WriteLatencyMax   = $rel.WriteLatencyMax
            }
        }
    }
}

$combinedOutput = @($combined) | ConvertTo-Json
$combinedOutput | Out-File -FilePath $OutputFilePath -Encoding UTF8