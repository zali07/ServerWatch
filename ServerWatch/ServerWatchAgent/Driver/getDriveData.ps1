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
                FriendlyName      = $phys.FriendlyName
                SerialNumber      = $phys.SerialNumber
                HealthStatus      = $phys.HealthStatus
                BusType           = $phys.BusType
                MediaType         = $phys.MediaType
                Model             = $phys.Model
                SizeGB            = "{0:N1}" -f ($phys.Size / 1GB)
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

@($combined) | ConvertTo-Json