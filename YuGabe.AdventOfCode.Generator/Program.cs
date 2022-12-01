using System.Reflection;

var year = DateTime.Now.Year;
var targetFolder = new DirectoryInfo($"Year{year}");
if (targetFolder.Exists)
{
    Console.WriteLine($"Folder '{targetFolder}' already exists.");
    return;
}

targetFolder.Create();

string template;
using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("YuGabe.AdventOfCode.Generator.Template.cs")!))
    template = reader.ReadToEnd();

foreach(var day in Enumerable.Range(1, 25))
{
    File.WriteAllText(Path.Combine(targetFolder.FullName, $"Day{day}.cs"), template.Replace($$$"""{{year}}""", year.ToString()).Replace($$$"""{{day}}""", day.ToString()));
}
