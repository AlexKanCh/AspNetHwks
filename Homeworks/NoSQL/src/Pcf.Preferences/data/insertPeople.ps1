#curl -Method POST -Uri http://localhost:5000/preference -InFile .\preferenceTheater.json -Headers @{'Content-Type'='application/json'}

$jsonDirectory=".\"
$apiEndpoint="http://localhost:5000/preference"

# Get all JSON files in the specified directory
$jsonFiles = Get-ChildItem -Path $jsonDirectory -Filter *.json

# Loop through each JSON file and upload it using curl
foreach ($jsonFile in $jsonFiles) {
    # Construct the full path to the JSON file
    $jsonFilePath = $jsonFile.FullName
    
    # Display the name of the file being uploaded
    Write-Host "Uploading $jsonFilePath..."
    
    # Execute the curl command to upload the JSON file
    curl -X POST -H "Content-Type: application/json" -d @$jsonFilePath $apiEndpoint
    
    # Check the result of the curlsl command and display a message
    if ($?) {
        Write-Host "Successfully uploaded $jsonFilePath."
    } else {
        Write-Host "Failed to upload $jsonFilePath."
    }
}

Write-Host "All uploads complete."