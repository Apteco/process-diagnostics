using Newtonsoft.Json;

namespace Apteco.Diagnostics.Model
{
  [JsonObject("exception")]
  public class DiagnosticException
  {

    [JsonProperty("address")]
    public ulong Address { get; set; }
    [JsonProperty("hresult")]
    public int HResult { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }

  }
}
