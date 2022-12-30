using System.Reflection;

foreach(var year in Enumerable.Range(2015, DateTime.Now.Year - 2015))
{
    var targetFolder = new DirectoryInfo($"Year{year}");
    targetFolder.Create();

    string template;
    using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("YuGabe.AdventOfCode.Generator.Template.cs")!))
        template = reader.ReadToEnd();

    foreach (var day in Enumerable.Range(1, 25))
    {
        var path = Path.Combine(targetFolder.FullName, $"Day{day}.cs");
        if (!File.Exists(path))
            File.WriteAllText(path, template.Replace($$$"""{{year}}""", year.ToString()).Replace($$$"""{{day}}""", day.ToString()));

    }
}
