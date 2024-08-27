using System.Threading.Tasks;

namespace AvaloniaPrototype.Services;
public interface IAquiferService
{
    public Task<(string Name, string Content)> GetResourceContentAsync(int contentId);
}
