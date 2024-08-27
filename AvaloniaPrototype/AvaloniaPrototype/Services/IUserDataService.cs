using System.Globalization;

namespace AvaloniaPrototype.Services;

internal interface IUserDataService
{
    public (string Name, string Content) GetLocalResource(int contentId);
    public void SaveLocalResource(int contentId, (string Name, string Content) resource);
    public CultureInfo GetUserCultureInfo();
    public void SaveUserCultureInfo(CultureInfo cultureInfo);
}
