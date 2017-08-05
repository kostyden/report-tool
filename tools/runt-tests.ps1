function TryRunTests
{
    param( [string]$Path)
    $isTestsExists = Test-Path $Path

    if ($isTestsExists)
    {
        & "..\packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe" $Path
    }
}

TryRunTests -Path "..\tests\ReportTool.Tests\bin\Debug\ReportTool.Tests.dll"
TryRunTests -Path "..\tests\ReportTool.Tests\bin\Release\ReportTool.Tests.dll"



