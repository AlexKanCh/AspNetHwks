$jsonDirectory = ".\"
$apiEndpoint = "http://localhost:8094/preference"

# Получаем все JSON-файлы в указанной директории
$jsonFiles = Get-ChildItem -Path $jsonDirectory -Filter *.json

# Проходим по каждому JSON-файлу и загружаем его
foreach ($jsonFile in $jsonFiles) {
    # Строим полный путь к JSON-файлу
    $jsonFilePath = $jsonFile.FullName
    
    # Выводим имя файла, который загружается
    Write-Host "Uploading $jsonFilePath..."
    
    # Читаем содержимое JSON-файла
    $jsonContent = Get-Content -Raw -Path $jsonFilePath

    # Выполняем POST-запрос с использованием Invoke-WebRequest
    try {
        $response = Invoke-WebRequest -Uri $apiEndpoint `
                                      -Method POST `
                                      -Headers @{
                                          "accept" = "text/plain"
                                          "Content-Type" = "application/json"
                                      } `
                                      -Body $jsonContent

        # Проверяем результат и выводим сообщение
        if ($response.StatusCode -eq 200) {
            Write-Host "Successfully uploaded $jsonFilePath."
        } else {
            Write-Host "Failed to upload $jsonFilePath. Status code: $($response.StatusCode)"
        }
    } catch {
        Write-Host "Error uploading ${jsonFilePath}: $($_.Exception.Message)"
    }
}

Write-Host "All uploads complete."