using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Apteco.Diagnostics.Model
{
  [JsonObject("thread")]
  public class DiagnosticThread
  {

    [JsonIgnore]
    public string Name => "Thread: " + Id;
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("stack")]
    public string[] Stack { get; set; }
    [JsonIgnore]
    public string Stacktrace
    {
      get
      {
        var builder = new StringBuilder();
        foreach (var s in Stack)
        {
          builder.AppendLine(s);
        }
        return builder.ToString();
      }
    }
    [JsonProperty("exception")]
    public DiagnosticException Exception { get; set; }

  }
}
