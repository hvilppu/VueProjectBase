using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Project.Domain.Services
{
    public class BaseService
    {
        public string StringAsCulture(IStringLocalizer localizer, string culture, string key)
        {
            try
            {
                return localizer.WithCulture(new CultureInfo(culture))[key];
            }

            catch
            {
                return key;
            }
        }
    }
}
