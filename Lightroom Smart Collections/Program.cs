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
            var currentYear = 3;
            await CreateSmartCollections(string.Empty);
            await CreateSmartCollections("D_Book");
            await CreateSmartCollections("R_Book");

            async Task CreateSmartCollections(string keywordName)
            {
                var startDate = new DateTime(2018, 01, 01);
                for (int weekNo = 1; weekNo < (52 * (currentYear + 1)) + 1; weekNo++)
                {
                    var endDate = startDate.AddDays(6);
                    if (weekNo > 52 * currentYear)
                    {
                        var collectionName = $"Week {weekNo}";
                        if (!string.IsNullOrWhiteSpace(keywordName))
                        {
                            collectionName = $"{keywordName} - {collectionName}";
                        }
                        var output = @$"
s = {{
	internalName = ""{collectionName}"",
    title = ""{collectionName}"",
	type = ""LibrarySmartCollection"",
	value = {{
        {{
            criteria = ""captureTime"",
			operation = ""in"",
			value = ""{startDate:yyyy-MM-dd}"",
			value2 = ""{endDate:yyyy-MM-dd}"",
		}},";
            if (!string.IsNullOrWhiteSpace(keywordName))
            {
                output += @$"
	    {{
            criteria = ""keywords"",
            operation = ""any"",
			value = ""{keywordName}"",
			value2 = """",
		}},";
            }
            output += @$"
		combine = ""intersect"",
	}},
	version = 0,
}}";
                                                
                        using StreamWriter outputFile = new StreamWriter(Path.Combine(desktopPath, $"{collectionName}.lrsmcol"), false);
                        await outputFile.WriteAsync(output);
                    }
                    startDate = endDate.AddDays(1);
                }
            }
        }
    }
}
