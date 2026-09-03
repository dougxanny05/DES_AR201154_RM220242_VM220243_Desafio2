param(
	[int]$requests = 15,
	[string]$url = "http://localhost:5001/eventos"
)

Write-Host "Sending $requests requests to $url"

for ($i=1; $i -le $requests; $i++) {
	try {
		$resp = Invoke-RestMethod -Method Get -Uri $url -ErrorAction Stop
		Write-Host "[$i] Success - HTTP 200"
	} catch {
		if ($_.Exception.Response -and $_.Exception.Response.StatusCode) {
			$code = $_.Exception.Response.StatusCode.value__
			Write-Host "[$i] Failed - HTTP $code"
		} else {
			Write-Host "[$i] Error: $($_.Exception.Message)"
		}
	}
	Start-Sleep -Milliseconds 200
}

Write-Host "Done"
