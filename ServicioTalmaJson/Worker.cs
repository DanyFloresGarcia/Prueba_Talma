namespace ServicioTalmaJson
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using ServicioTalmaJson.Entity;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private static readonly string InputFilePath = @"D:\Dany\GIT\ASP .NET CORE\Prueba tecnica\JSON\input.json";
        private static readonly string OutputFilePath = @"D:\Dany\GIT\ASP .NET CORE\Prueba tecnica\JSON\output.json";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Iniciando proceso...");
                    
                    //Leer archivo json
                    var originalData = ReadDataFromFile(InputFilePath);

                    //Transformar data
                    var processedDataList = TransformData(originalData);

                    //Archivo de salida
                    WriteDataToFile(OutputFilePath, processedDataList);

                    _logger.LogInformation("Data procesada correctamente.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al procesar los datos: {ex.Message}");
                }

                //Esperar un intervalo de 5 minutos
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private OriginalData ReadDataFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"El archivo no se encontró: {filePath}");

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<OriginalData>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            }) ?? new OriginalData();
        }

        private void WriteDataToFile<T>(string filePath, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private dynamic TransformData(OriginalData originalData)
        {
            var listResponse = new List<ProcessedData>();

            var groupedCities = originalData.cities.Select(c => new {
                City = c,
                ClimateType = c.weather.FirstOrDefault()?.main ?? "Unknown",
                TempRange = GetTemperatureRange(c.main.temp)
            })
            .GroupBy(x => new
            {
                x.ClimateType
            }).Select(g => new 
            {
                climateType = g.Key.ClimateType,
                temperatureGroups = g
                    .GroupBy(x => x.TempRange)
                    .Select(tg => new
                    {
                        TempRange = tg.Key,
                        Cities = tg.Select(x => new
                        {
                            Id = x.City.id,
                            Name = x.City.name,
                            Temp = x.City.main.temp
                        }).ToList()
                    }).ToList()
            })
            .ToList();

            return groupedCities;
        }

        private static string GetTemperatureRange(double temp)
        {
            if (temp < 85) return "Cold";
            if (temp >= 85) return "Mild";
            return "Warm";
        }
    }


}
