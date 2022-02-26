using project.Common.Resources;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace project.Tests.Domain
{
    public static class Helper
    {
        public static IStringLocalizer<Lang> StringLocalizer(string key, string value)
        {
            IStringLocalizer<Lang> localizer;
            var mock = new Mock<IStringLocalizer<Lang>>();
            var localizedString = new LocalizedString(key, value);
            mock.Setup(_ => _[key]).Returns(localizedString);
            localizer = mock.Object;
            return localizer;
        }

        public static IStringLocalizer<Lang> StringLocalizer(List<KeyValuePair<string,string>> keyValuePairs)
        {
            IStringLocalizer<Lang> localizer;
            var mock = new Mock<IStringLocalizer<Lang>>();
            foreach (var kvp in keyValuePairs)
            {
                var localizedString = new LocalizedString(kvp.Key, kvp.Value);
                mock.Setup(_ => _[kvp.Key]).Returns(localizedString);
            }

            localizer = mock.Object;
            return localizer;
        }

        public static IFormFile GetImageFormFile()
        {
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var file = fileMock.Object;

            file.CopyTo(ms);

            return file;
        }
    }
}
