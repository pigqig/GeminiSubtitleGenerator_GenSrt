using NAudio.Wave;
using NAudio.Lame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AudioSegmentInfo
{
    public string FilePath { get; set; }
    public TimeSpan Duration { get; set; }
}

public class AudioSplitter
{
    private const int SplitLengthMs = 5 * 60 * 1000; 

    public static List<AudioSegmentInfo> SplitAudio(string inputFile, string outputDir)
    {
        var segments = new List<AudioSegmentInfo>();
        if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

        var existingFiles = Directory.GetFiles(outputDir, "part_*.mp3");
        if (existingFiles.Length > 0)
        {
            var sortedFiles = existingFiles
                .OrderBy(f => {
                    string name = Path.GetFileNameWithoutExtension(f);
                    int idx = 0;
                    int.TryParse(name.Replace("part_", ""), out idx);
                    return idx;
                })
                .ToList();

            bool integrityCheck = true;
            foreach (var file in sortedFiles)
            {
                try 
                {
                    using (var reader = new AudioFileReader(file))
                    {
                        segments.Add(new AudioSegmentInfo { FilePath = file, Duration = reader.TotalTime });
                    }
                }
                catch 
                {
                    integrityCheck = false; 
                    break;
                }
            }
            if (integrityCheck && segments.Count > 0) return segments;
            segments.Clear();
        }

        using (var reader = new AudioFileReader(inputFile))
        {
            int fileIndex = 0;
            long bytesPerSplit = reader.WaveFormat.AverageBytesPerSecond * (SplitLengthMs / 1000);
            
            while (reader.Position < reader.Length)
            {
                string outputFile = Path.Combine(outputDir, $"part_{fileIndex}.mp3");
                long remainingBytes = reader.Length - reader.Position;
                long bytesToWrite = Math.Min(remainingBytes, bytesPerSplit);
                
                var buffer = new byte[bytesToWrite];
                int bytesRead = reader.Read(buffer, 0, (int)bytesToWrite);

                if (bytesRead <= 0) break;

                using (var writer = new LameMP3FileWriter(outputFile, reader.WaveFormat, 128))
                {
                    writer.Write(buffer, 0, bytesRead);
                }

                double actualSeconds = (double)bytesRead / reader.WaveFormat.AverageBytesPerSecond;
                if (actualSeconds > 0.5) 
                {
                    segments.Add(new AudioSegmentInfo { FilePath = outputFile, Duration = TimeSpan.FromSeconds(actualSeconds) });
                }
                else
                {
                    try { File.Delete(outputFile); } catch { }
                }
                fileIndex++;
            }
        }
        return segments;
    }
}