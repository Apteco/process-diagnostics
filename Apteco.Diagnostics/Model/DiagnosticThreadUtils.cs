using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Apteco.Diagnostics.Model
{
  public static class DiagnosticThreadUtils
  {
    public static void SaveThreads(this IEnumerable<DiagnosticThread> threads, string filePath)
    {
      using (var fs = File.OpenWrite(filePath))
      using (var sw = new StreamWriter(fs))
      using (var jsonWriter = new JsonTextWriter(sw))
      {
        jsonWriter.Formatting = Formatting.Indented;
        var serialiser = new JsonSerializer();
        serialiser.Serialize(jsonWriter, threads);
      }
    }

    public static IEnumerable<DiagnosticThread> LoadThreads(string filePath)
    {
      using (var fs = File.OpenRead(filePath))
      using (var sr = new StreamReader(fs))
      using (var jsonReader = new JsonTextReader(sr))
      {
        var deserialiser = new JsonSerializer();
        return deserialiser.Deserialize<IEnumerable<DiagnosticThread>>(jsonReader);
      }
    }

  }
}
