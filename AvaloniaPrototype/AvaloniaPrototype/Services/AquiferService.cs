using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AvaloniaPrototype.Services;
internal class AquiferService : IAquiferService
{
    public AquiferService(HttpClient aquiferClient, Settings settings)
    {
        m_aquiferClient = aquiferClient;
        m_settings = settings;
    }

    public async Task<(string Name, string Content)> GetResourceContentAsync(int contentId)
    {
        try
        {
            var response = await m_aquiferClient.GetFromJsonAsync<ResourceContentResponse>($"resources/{contentId}?contentTextType=html&api-key={m_settings.AquiferApiKey}").ConfigureAwait(false);
            return response != null
                ? (response.Name, Content: string.Join("\n", response.Content))
                : (Name: "Error", Content: "Unable to fetch resource content at this time.  Please try again.");
        }
        catch (Exception ex)
        {
            return (Name: "Exception", Content: ex.Message);
        }
    }

    private class ResourceContentResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required IReadOnlyList<string> Content {  get; set; }
    }

    private readonly HttpClient m_aquiferClient;
    private readonly Settings m_settings;
}
