using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class SrtItem
{
    public int Sequence { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Text { get; set; } = "";
}

public class SrtMerger
{
    public static string MergeSrts(List<string> srtContents, List<AudioSegmentInfo> segments)
    {
        List<SrtItem> allSubtitles = new List<SrtItem>();
        TimeSpan currentBaseOffset = TimeSpan.Zero;

        for (int i = 0; i < srtContents.Count; i++)
        {
            string srt = srtContents[i];
            TimeSpan segmentDuration = (i < segments.Count) ? segments[i].Duration : TimeSpan.FromMinutes(10); // 預設防護

            if (string.IsNullOrWhiteSpace(srt) || !srt.Contains("-->"))
            {
                if (i < segments.Count) currentBaseOffset = currentBaseOffset.Add(segments[i].Duration);
                continue;
            }

            try 
            {
                // 傳入目前的片段長度，用於過濾幻覺 (Hallucination)
                var parsedItems = ParseSrt(srt, currentBaseOffset, segmentDuration);
                
                // [防幻覺機制] 檢查連續重複內容
                // 如果連續 3 句內容一模一樣，視為模型跳針，過濾掉
                var filteredItems = RemoveRepetitiveLoops(parsedItems);
                
                allSubtitles.AddRange(filteredItems);
            }
            catch { }

            if (i < segments.Count) currentBaseOffset = currentBaseOffset.Add(segments[i].Duration);
        }

        // 排序
        allSubtitles = allSubtitles.OrderBy(s => s.StartTime).ToList();

        // 去重疊
        for (int i = 0; i < allSubtitles.Count - 1; i++)
        {
            var current = allSubtitles[i];
            var next = allSubtitles[i + 1];
            if (current.EndTime > next.StartTime)
            {
                if (next.StartTime > current.StartTime) current.EndTime = next.StartTime.Subtract(TimeSpan.FromMilliseconds(10));
                else next.StartTime = current.EndTime.Add(TimeSpan.FromMilliseconds(10));
            }
        }

        StringBuilder finalSrt = new StringBuilder();
        int globalCounter = 1;

        foreach (var item in allSubtitles)
        {
            if (item.EndTime <= item.StartTime) continue;
            finalSrt.AppendLine(globalCounter.ToString());
            finalSrt.AppendLine($"{FormatTime(item.StartTime)} --> {FormatTime(item.EndTime)}");
            finalSrt.AppendLine(item.Text);
            finalSrt.AppendLine();
            globalCounter++;
        }
        return finalSrt.ToString();
    }

    private static List<SrtItem> RemoveRepetitiveLoops(List<SrtItem> items)
    {
        if (items.Count < 3) return items;
        
        List<SrtItem> result = new List<SrtItem>();
        for (int i = 0; i < items.Count; i++)
        {
            // 檢查是否跟前兩句完全一樣
            if (result.Count >= 2)
            {
                string t1 = result[result.Count - 1].Text;
                string t2 = result[result.Count - 2].Text;
                string current = items[i].Text;

                // 簡單去重複：如果這句跟上一句一樣，且長度很短(<5字)，可能是 "是的" 迴圈，跳過
                if (current == t1 && current.Length < 5) continue;
                
                // 嚴格去重複：如果連續三句一樣
                if (current == t1 && current == t2) continue;
            }
            result.Add(items[i]);
        }
        return result;
    }

    private static List<SrtItem> ParseSrt(string srtContent, TimeSpan offset, TimeSpan maxDuration)
    {
        var items = new List<SrtItem>();
        srtContent = srtContent.Replace("```srt", "").Replace("```", "").Trim();
        var lines = srtContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        SrtItem currentItem = null;
        
        // [Fix] 更新 Regex 以支援缺小時的格式 (MM:ss,fff)
        // Group 1: HH (Optional), Group 2: mm, Group 3: ss, Group 4: fff
        Regex timeRegex = new Regex(@"(?:(\d{1,2}):)?(\d{1,2}):(\d{1,2})[,.](\d{1,3})");

        foreach (var line in lines)
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) 
            {
                if (currentItem != null) 
                { 
                    // [防幻覺機制] 檢查：如果這句字幕的開始時間，超過了這個音檔的實際長度
                    // 代表 AI 產生了超出範圍的幻覺 (例如 5 分鐘音檔產生了 4 小時的字幕)
                    // 給予 30 秒的緩衝 (Buffer)
                    if (currentItem.StartTime.Subtract(offset) < maxDuration.Add(TimeSpan.FromSeconds(30)))
                    {
                        items.Add(currentItem); 
                    }
                    currentItem = null; 
                }
                continue;
            }
            
            if (trimmed.Contains("-->"))
            {
                var matches = timeRegex.Matches(trimmed);
                if (matches.Count >= 2)
                {
                    if (currentItem == null) currentItem = new SrtItem();
                    
                    TimeSpan start = ParseFlexibleTime(matches[0]);
                    TimeSpan end = ParseFlexibleTime(matches[1]);

                    currentItem.StartTime = start.Add(offset);
                    currentItem.EndTime = end.Add(offset);
                }
            }
            else if (int.TryParse(trimmed, out int seq) && currentItem == null) { }
            else
            {
                if (currentItem != null)
                {
                    if (!string.IsNullOrEmpty(currentItem.Text)) currentItem.Text += "\n";
                    currentItem.Text += trimmed;
                }
            }
        }
        if (currentItem != null)
        {
             if (currentItem.StartTime.Subtract(offset) < maxDuration.Add(TimeSpan.FromSeconds(30)))
             {
                 items.Add(currentItem);
             }
        }
        return items;
    }

    private static TimeSpan ParseFlexibleTime(Match match)
    {
        // match groups: 0=full, 1=HH(maybe empty), 2=mm, 3=ss, 4=fff
        int hours = 0;
        int minutes = 0;
        int seconds = 0;
        int milliseconds = 0;

        if (match.Groups[1].Success) int.TryParse(match.Groups[1].Value, out hours);
        int.TryParse(match.Groups[2].Value, out minutes);
        int.TryParse(match.Groups[3].Value, out seconds);
        int.TryParse(match.Groups[4].Value, out milliseconds);

        return new TimeSpan(0, hours, minutes, seconds, milliseconds);
    }

    private static string FormatTime(TimeSpan ts)
    {
        return ts.ToString(@"hh\:mm\:ss\,fff");
    }
}