using Newtonsoft.Json;
using System;
using System.IO;

public class AppConfig
{
    public string ApiKey { get; set; } = "";
    public string SelectedModel { get; set; } = "gemini-2.5-pro"; 
    public string OutputPath { get; set; } = "";
    public string TempPath { get; set; } = "";
}

public static class ConfigManager
{
    private static string ConfigPath = "config.json";

    public static AppConfig Load()
    {
        if (File.Exists(ConfigPath))
        {
            try
            {
                string json = File.ReadAllText(ConfigPath);
                return JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
            }
            catch { }
        }
        return new AppConfig();
    }

    public static void Save(AppConfig config)
    {
        try
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }
        catch { }
    }
}