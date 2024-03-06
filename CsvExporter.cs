using System.Globalization;
using CsvHelper;

namespace TrafficVSL_OMI;

public static class CsvExporter
{
    public static void Export<T>(string baseFileName, IEnumerable<T> records)
    {
        var filePath = GetFilePath(baseFileName);
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
    }

    private static string GetFilePath(string baseFileName) => $"../../../export-{baseFileName}.csv";
}