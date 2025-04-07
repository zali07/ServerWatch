param(
    [string]$OutputFilePath
)

$reliabilityData = Get-Disk | Get-StorageReliabilityCounter
$physicalDisks = Get-PhysicalDisk

$combined = foreach ($rel in $reliabilityData) {

    $disk = $physicalDisks | Where-Object { $_.DeviceID -eq $rel.DeviceId }    

    if ($disk) {
        [PSCustomObject]@{
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

$combinedOutput = @($combined) | ConvertTo-Json
$combinedOutput | Out-File -FilePath $OutputFilePath -Encoding UTF8