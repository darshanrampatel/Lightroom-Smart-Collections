using System;
using System.IO;
using System.Threading.Tasks;

namespace Lightroom_Smart_Collections
{
    class Program
    {
        static async Task Main()
        {
            var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Lightroom Smart Collections");
            if (!Directory.Exists(desktopPath))
            {
                Directory.CreateDirectory(desktopPath);
            }
            var startDate = new DateTime(2018, 01, 01);
            for (int weekNo = 1; weekNo < (52 * 4) + 2; weekNo++)
            {
                var endDate = startDate.AddDays(6);
                if (weekNo > 52 * 3)
                {
                    var output = @$"
s = {{
	internalName = ""Week {weekNo}"",
    title = ""Week {weekNo}"",
	type = ""LibrarySmartCollection"",
	value = {{
                    {{
                        criteria = ""captureTime"",
			operation = ""in"",
			value = ""{startDate.ToString("yyyy-MM-dd")}"",
			value2 = ""{endDate.ToString("yyyy-MM-dd")}"",
		}},
		combine = ""intersect"",
	}},
	version = 0,
}}
            ";
                    using StreamWriter outputFile = new StreamWriter(Path.Combine(desktopPath, $"Week {weekNo}.lrsmcol"), false);
                    await outputFile.WriteAsync(output);
                }
                startDate = endDate.AddDays(1);
            }
        }
    }
}
