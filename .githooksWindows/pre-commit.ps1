Write-Host ">> RUNNING pre-commit GITHOOK in .githooks/"
 
$FILES = (git diff --cached --name-only --diff-filter=ACMR | ForEach-Object { $_ -replace " ", "\ " } | Select-String -Pattern 'Frontend.*').Matches.Value
if ([string]::IsNullOrEmpty($FILES)) { exit 0 }
 
# Prettify all selected files
Write-Host ">> RUNNING PRETTIER"
$FILES | ForEach-Object { & ".\Frontend\node_modules\.bin\prettier" --ignore-unknown --write $_ }
Write-Host ">> FINISHED PRETTIER"
 
# Add back the modified/prettified files to staging
$FILES | ForEach-Object { git add $_ }
 
exit 0
