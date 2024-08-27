using System.Threading.Tasks;

namespace AvaloniaPrototype.Services;
internal class DummyAquiferService : IAquiferService
{
    public Task<(string Name, string Content)> GetResourceContentAsync(int contentId)
    {
        return Task.FromResult((Name: "Abraham", Content: "Father Abraham had many sons.  Many sons had father Abraham."));
    }
}
