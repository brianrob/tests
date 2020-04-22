$lengthInMS = 1000 * 60 * 60

Write-Host $([System.DateTime]::Now)
$startTime = [System.Environment]::TickCount
$endTime = $startTime + $lengthInMS
$count = 0
while([System.Environment]::TickCount -lt $endTime)
{
    bin\Release\netcoreapp3.1\hello_world_loop.exe > $NULL
    $count++
    if ($count % 1000 -eq 0)
    {
        Write-Host "Current Count = $count"
    }
}
Write-Host "Final Count = $count"
Write-Host $([System.DateTime]::Now)
