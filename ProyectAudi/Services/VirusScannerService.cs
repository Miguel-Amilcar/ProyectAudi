using VirusTotalNet;
using VirusTotalNet.Objects;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;
using Microsoft.AspNetCore.Http;

public interface IVirusScannerService
{
    Task<bool> ArchivoEsSeguroAsync(IFormFile archivo);
}

public class VirusScannerService : IVirusScannerService
{
    private readonly VirusTotal _virusTotal;

    public VirusScannerService(string apiKey)
    {
        _virusTotal = new VirusTotal(apiKey)
        {
            UseTLS = true
        };
    }

    public async Task<bool> ArchivoEsSeguroAsync(IFormFile archivo)
    {
        using var stream = archivo.OpenReadStream();

        // Escanear el archivo
        var scanResult = await _virusTotal.ScanFileAsync(stream, archivo.FileName);

        // Esperar unos segundos para que el análisis esté disponible
        int intentos = 0;
        FileReport report = null!;
        do
        {
            await Task.Delay(3000);
            report = await _virusTotal.GetFileReportAsync(scanResult.Resource);
            intentos++;
        }
        while (report.ResponseCode != FileReportResponseCode.Present && intentos < 5);

        // Validar si el archivo es seguro
        return report.ResponseCode == FileReportResponseCode.Present && report.Positives == 0;
    }
}
