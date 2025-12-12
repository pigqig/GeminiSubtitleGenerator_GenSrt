using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class GeminiResult
{
    public string SrtText { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
}

public class GeminiService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private const string UploadEndpoint = "https://generativelanguage.googleapis.com/upload/v1beta/files";

    public GeminiService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromMinutes(10);
    }

    public async Task<string> UploadFileAsync(string filePath)
    {
        string mimeType = "audio/mp3";
        using (var fileStream = File.OpenRead(filePath))
        {
            var content = new StreamContent(fileStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{UploadEndpoint}?key={_apiKey}");
            request.Headers.Add("X-Goog-Upload-Protocol", "raw");
            request.Headers.Add("X-Goog-Upload-Command", "start, upload, finalize");
            request.Headers.Add("X-Goog-Upload-File-Name", Path.GetFileName(filePath));
            request.Content = content;
            
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            return json["file"]?["uri"]?.ToString();
        }
    }

    public async Task<GeminiResult> GenerateSrtFromUriAsync(string fileUri, string modelName)
    {
        string generateEndpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent";
        
        string prompt = @"
你是一位頂級的 AI 語言專家，專精於「跨語言語音轉錄」與「SRT 字幕生成」。
你的核心任務是：無論音訊中的說話者使用何種語言（英語、日語、混合語等），你都必須將其內容**直接翻譯並轉錄為 繁體中文 (Traditional Chinese)**。

*** 關鍵指令 (Critical Instructions) ***
1. [翻譯強制]：無論聽到什麼語言，一律翻譯成繁體中文。
2. [時間碼格式]：嚴格使用 HH:mm:ss,xxx (例如: 00:01:05,009)。小時部分 (HH) 不可省略！
3. [防幻覺]：若音訊為長時間靜音或重複噪音，請直接停止輸出，不要重複生成相同的文字 (如 ""是的"", ""好的"")。
4. [結構]：標準 SRT 格式，區塊間保留一行空行。

*** 絕對鐵律：最終輸出內容只能是 繁體中文，嚴禁輸出英文或其他語言！ ***

輸出要求：
1. 最終輸出語言：繁體中文 (Traditional Chinese)。如果聽到英文，請翻譯成中文。
2. 格式：嚴格符合 SRT 標準。
3. 語意：通順、自然、符合台灣繁體中文習慣。

第一部分：SRT 格式規範 (嚴格遵守)
1. 序列號：從 1 開始遞增。
2. 時間碼：hh:mm:ss,xxx --> hh:mm:ss,xxx (注意毫秒需補齊三位，小時需補齊兩位)。
3. 結構：每個字幕塊之間必須有一行空行。
4. 長度：每行字幕建議不超過 16-20 字，過長請斷句。

第二部分：翻譯與轉錄原則
1. **[語言強制]**：若音訊內容為英語或其他語言，請進行意譯，輸出為繁體中文。
   - 錯誤範例 (音訊是英文): Hello everyone. -> Hello everyone.
   - 正確範例 (音訊是英文): Hello everyone. -> 大家好。
2. **[語意優先]**：按語意切分時間軸，不要被語音的停頓切碎句子。
3. **[內容過濾]**：移除結巴、重複詞 (如: 那個、那個)、無意義語助詞 (Uh, Um)。
4. **[非語音處理]**：音樂、噪音、背景音一律忽略，不輸出任何標籤 (如 [Music])，直接留白。

5 ** 移除語助詞（如：嗯、啊、呃）、口吃或不影響語意的重複無意義詞如：那個、那個、我的天啊）。
  - 切分時可用標點作為參考，但輸出字幕中不得出現任何標點符號（如：，。？！。

範例輸入 (假設音訊內容): 
""Welcome to our new tutorial. Today we are going to learn C#.""

範例輸出 (你必須輸出):
1
00:00:01,000 --> 00:00:03,500
歡迎來到我們的新教學

2
00:00:03,600 --> 00:00:06,000
今天我們要來學習 C#

(請注意：即使聽到英文，輸出的也是中文)

開始執行任務，輸出完整的 SRT 內容：
";



        var requestBody = new
        {
            contents = new[] { new { parts = new object[] {
                new { text = prompt },
                new { file_data = new { mime_type = "audio/mp3", file_uri = fileUri } }
            }}},
            generationConfig = new 
            {
                temperature = 0.3,
                maxOutputTokens = 8192,
                topP = 0.95,
                topK = 64
            }
        };

        string jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync($"{generateEndpoint}?key={_apiKey}", content);
        
        if (!response.IsSuccessStatusCode) 
        {
             string errorContent = await response.Content.ReadAsStringAsync();
             throw new HttpRequestException(errorContent, null, response.StatusCode);
        }

        string resultJson = await response.Content.ReadAsStringAsync();
        var jsonObj = JObject.Parse(resultJson);
        
        string text = jsonObj["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString()?.Trim();
        var usage = jsonObj["usageMetadata"];
        int inputTokens = usage?["promptTokenCount"]?.Value<int>() ?? 0;
        int outputTokens = usage?["candidatesTokenCount"]?.Value<int>() ?? 0;

        return new GeminiResult { SrtText = text, InputTokens = inputTokens, OutputTokens = outputTokens };
    }

    public async Task<bool> DeleteFileAsync(string fileUri)
    {
        try { var response = await _httpClient.DeleteAsync($"{fileUri}?key={_apiKey}"); return response.IsSuccessStatusCode; }
        catch { return false; }
    }
}