@echo off
echo ProcessTreeWriter Usage Examples
echo.
echo 1. Show the full process tree from a standard ETL file:
echo    ProcessTreeWriter.exe trace.etl
echo.
echo 2. Show only the subtree for process ID 1234:
echo    ProcessTreeWriter.exe trace.etl 1234
echo.
echo 3. Process a compressed ETL file:
echo    ProcessTreeWriter.exe trace.etl.zip
echo.
echo 4. Process a compressed ETL file and filter by process ID:
echo    ProcessTreeWriter.exe trace.etl.zip 1234
echo.
echo 5. Filter out specific processes and their child processes:
echo    ProcessTreeWriter.exe trace.etl svchost.exe,conhost.exe
echo.
echo 6. Combine PID filter with process name filter:
echo    ProcessTreeWriter.exe trace.etl 1234 explorer.exe,chrome.exe
echo.
echo Note: Replace 'trace.etl' or 'trace.etl.zip' with the actual path to your file,
echo       '1234' with the actual process ID you want to filter by,
echo       and specify process names to filter as a comma-separated list with no spaces.
