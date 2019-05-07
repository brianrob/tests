param($script)

$iterations=10

Write-Host "--------------------------------------------------------------"
Write-Host "Running $iterations iterations (Plus warm-up iteration.)."
Write-Host "Command: $script"
Write-Host "--------------------------------------------------------------"

foreach($i in 0..$iterations)
{
    Write-Host "Iteration $i"
    Invoke-Command $script
}

Write-Host "--------------------------------------------------------------"
Write-Host "Finished."
Write-Host "--------------------------------------------------------------"
