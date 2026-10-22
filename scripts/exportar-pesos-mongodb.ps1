param(
    [Parameter(Mandatory=$true)][string]$Token,
    [Parameter(Mandatory=$true)][int[]]$PetIds,
    [string]$ApiUrl = 'http://localhost:5088',
    [string]$Arquivo = './exports/pesos-mongodb.json'
)
# Consulta o Oracle atraves do endpoint existente e gera JSON compatível com mongoimport.
# Execute com PowerShell do Windows. Nao registre o token em arquivo.
$headers = @{ Authorization = "Bearer $Token" }
$documentos = @()
foreach ($petId in $PetIds) {
    $url = "$ApiUrl/api/pets/$petId/pesos/exportar-json"
    $resposta = Invoke-RestMethod -Uri $url -Headers $headers -Method Get
    $medicoes = @()
    foreach ($p in $resposta.historico) {
        $medicoes += @{
            pesoId = @{ '$numberDecimal' = ([decimal]$p.idHistoricoPeso).ToString([System.Globalization.CultureInfo]::InvariantCulture) }
            peso = @{ '$numberDecimal' = ([decimal]$p.peso).ToString([System.Globalization.CultureInfo]::InvariantCulture) }
            dataMedicao = @{ '$date' = ([datetime]$p.dataMedicao).ToUniversalTime().ToString('yyyy-MM-ddTHH:mm:ss.fffZ') }
        }
    }
    $documentos += @{
        '_id' = "pet-$petId"
        petId = @{ '$numberDecimal' = ([decimal]$petId).ToString([System.Globalization.CultureInfo]::InvariantCulture) }
        atualizadoEm = @{ '$date' = [DateTime]::UtcNow.ToString('yyyy-MM-ddTHH:mm:ss.fffZ') }
        historico = $medicoes
    }
}
$pasta = Split-Path -Parent $Arquivo
if ($pasta -and -not (Test-Path $pasta)) { New-Item -ItemType Directory -Path $pasta -Force | Out-Null }
ConvertTo-Json -InputObject $documentos -Depth 10 | Set-Content -Path $Arquivo -Encoding UTF8
Write-Host "Dataset gerado: $Arquivo - total de pets: $($documentos.Count)"
Write-Host "Importar: mongoimport --uri mongodb://localhost:27017 --db amandaba --collection historico_pesos --file $Arquivo --jsonArray --mode upsert --upsertFields _id"
