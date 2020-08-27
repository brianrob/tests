###### Configuration ######
$PreExpression = { dotnet new console }
$ExpressionToMeasure = { dotnet build }
$PostExpression = $null
$NumIterations = 10
###########################


function Invoke-Iteration
{
    Param ($Iteration)

    # Create a random working directory.
    Write-Host "[$Iteration] Create Random Working Directory"
    $dirName = [Guid]::NewGuid().ToString();
    New-Item -Path $dirName -ItemType Directory | Out-Null
    
    # Switch to the working directory.
    Set-Location $dirName
    
    # Execute the pre-expression.
    if($PreExpression -ne $null)
    {
        Write-Host "[$Iteration] Pre-Expression: $PreExpression"
        $result = Measure-Command -Expression $PreExpression
        Write-Host "[$Iteration] Completed in $($result.TotalMilliseconds)ms"
    }

    # Execute the expression to measure.
    Write-Host "[$Iteration] Expression-To-Measure: $ExpressionToMeasure"
    $measurementResults = Measure-Command -Expression $ExpressionToMeasure
    Write-Host "[$Iteration] Completed in $($measurementResults.TotalMilliseconds)ms"

    # Execute the post-expression.
    if($PostExpression -ne $null)
    {
        Write-Host "[$Iteration] Post-Expression: $PostExpression"
        $result = Measure-Command -Expression $PostExpression
        Write-Host "[$Iteration] Completed in $($result.TotalMilliseconds)ms"
    }
    
    # Switch back to the original working directory and delete the random directory.
    Write-Host "[$Iteration] Delete Random Working Directory"
    Set-Location ../
    Remove-Item $dirName -Recurse | Out-Null

    return $measurementResults
}


# Setup the results list.
$resultsList = New-Object -TypeName System.Collections.ArrayList

# Print the header.
Out-Host -InputObject '==================='
Out-Host -InputObject 'Measurement Harness'
Out-Host -InputObject '==================='

# Warm-up.
Out-Host -InputObject '*Warm-Up: Starting'
Invoke-Iteration -Iteration 0 | Out-Null
Out-Host -InputObject '*Warm-Up: Stopping'
Out-Host -InputObject ""

# Execute the test.
Out-Host -InputObject "*Execute $($NumIterations) Iterations: Starting"
for($i=1; $i -le $NumIterations; $i++)
{
    $result = Invoke-Iteration -Iteration $i
    $resultsList.Add($result)
}
Out-Host -InputObject "*Execute $($NumIterations) Iterations: Stopping"
Out-Host -InputObject ""
Out-Host -InputObject ""

# Dump the results to the screen.
Out-Host -InputObject "Results (ms)"
for($i=0; $i -le $resultsList.Count; $i++)
{
    $result = $resultsList[$i]
    Out-Host -InputObject "$($result.TotalMilliseconds)"
}