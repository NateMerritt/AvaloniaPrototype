using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPrototype.Services;

internal class UserDataService : IUserDataService
{
    public (string Name, string Content) GetLocalResource(int contentId)
    {
        throw new NotImplementedException();
    }

    public CultureInfo GetUserCultureInfo()
    {
        throw new NotImplementedException();
    }

    public void SaveLocalResource(int contentId, (string Name, string Content) resource)
    {
        throw new NotImplementedException();
    }

    public void SaveUserCultureInfo(CultureInfo cultureInfo)
    {
        throw new NotImplementedException();
    }
}
