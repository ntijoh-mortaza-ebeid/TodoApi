Write-Host ">> RUNNING pre-commit GITHOOK in .githooksWindows/"

# Select all git added files
$FILES = (git diff --cached --name-only --diff-filter=ACMR | ForEach-Object { $_ -replace " ", "\ " } | Select-String -Pattern 'frontend.*').Matches.Value
if ([string]::IsNullOrEmpty($FILES)) { exit 0 }
 
# Run Prettier on all selected files
Write-Host ">> RUNNING PRETTIER"
$FILES | ForEach-Object { & ".\Frontend\node_modules\.bin\prettier" --ignore-unknown --write $_ }
Write-Host ">> FINISHED PRETTIER"

# Run ESLint on all selected files
Write-Host ">> RUNNING ESLINT"
$FILES | ForEach-Object { & ".\Frontend\node_modules\.bin\eslint" --fix $_ }
Write-Host ">> FINISHED ESLINT"

# Add back the modified files to staging
$FILES | ForEach-Object { git add $_ }
 
exit 0
