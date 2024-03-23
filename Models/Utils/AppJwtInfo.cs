using System.Text.Json;

namespace Todo_API.Models.Utils;

public class AppJwtInfo
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
