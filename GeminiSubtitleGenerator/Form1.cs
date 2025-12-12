using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Net.Http;
using NAudio.Wave;

namespace GeminiSubtitleGenerator
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cts;
        private bool _isPaused = false;
        private int _requestInterval = 4000; 

        public Form1()
        {
            InitializeComponent();
            ApplyDarkTheme();
            InitializeModels();
            LoadSettings();
        }

        private void InitializeModels()
        {
            cmbModel.Items.AddRange(new object[] {
                "gemini-2.5-pro",
                "gemini-1.5-pro",
                "gemini-1.5-flash",
                "gemini-2.0-flash-exp"
            });
        }

        private void LoadSettings()
        {
            var config = ConfigManager.Load();
            if (!string.IsNullOrEmpty(config.ApiKey)) txtApiKey.Text = config.ApiKey;
            if (!string.IsNullOrEmpty(config.SelectedModel) && cmbModel.Items.Contains(config.SelectedModel))
                cmbModel.SelectedItem = config.SelectedModel;
            else
                cmbModel.SelectedItem = "gemini-2.5-pro"; 
            if (!string.IsNullOrEmpty(config.OutputPath)) txtSrtPath.Text = config.OutputPath;
            if (!string.IsNullOrEmpty(config.TempPath)) txtTempPath.Text = config.TempPath;
        }

        private void menuSaveKey_Click(object sender, EventArgs e) { SaveConfig(); MessageBox.Show("設定已儲存！", "設定儲存"); }
        private void SaveConfig()
        {
            var config = new AppConfig 
            { 
                ApiKey = txtApiKey.Text.Trim(),
                SelectedModel = cmbModel.SelectedItem?.ToString() ?? "gemini-2.5-pro",
                OutputPath = txtSrtPath.Text.Trim(),
                TempPath = txtTempPath.Text.Trim()
            };
            ConfigManager.Save(config);
        }
        private void btnBrowseSrt_Click(object sender, EventArgs e) { using (var fbd = new FolderBrowserDialog()) if (fbd.ShowDialog() == DialogResult.OK) txtSrtPath.Text = fbd.SelectedPath; }
        private void btnBrowseTemp_Click(object sender, EventArgs e) { using (var fbd = new FolderBrowserDialog()) if (fbd.ShowDialog() == DialogResult.OK) txtTempPath.Text = fbd.SelectedPath; }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string apiKey = txtApiKey.Text.Trim();
            string selectedModel = cmbModel.SelectedItem?.ToString() ?? "gemini-2.5-pro";
            string customSrtDir = txtSrtPath.Text.Trim();
            string customTempDir = txtTempPath.Text.Trim();

            if (lstFiles.Items.Count == 0) { MessageBox.Show("請先加入檔案！"); return; }
            if (string.IsNullOrEmpty(apiKey)) { MessageBox.Show("請輸入 Gemini API Key"); return; }

            if (selectedModel.Contains("pro")) _requestInterval = 10000;
            else _requestInterval = 4000;

            SaveConfig();
            _cts = new CancellationTokenSource();
            _isPaused = false;
            btnPause.Text = "⏸ 暫停";
            UpdateUIState(true);

            try
            {
                GeminiService gemini = new GeminiService(apiKey);
                int totalFiles = lstFiles.Items.Count;
                for (int i = 0; i < totalFiles; i++)
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    string filePath = lstFiles.Items[i].ToString();
                    lblStatus.Text = $"處理中 ({i + 1}/{totalFiles}): {Path.GetFileName(filePath)}";
                    lstFiles.SelectedIndex = i;
                    await ProcessSingleFileAsync(filePath, gemini, selectedModel, customSrtDir, customTempDir, _cts.Token);
                }
                lblStatus.Text = "所有檔案處理完成！";
                MessageBox.Show("批次處理完成！");
            }
            catch (OperationCanceledException) { Log("=== 操作已取消 ===", Color.Orange); lblStatus.Text = "已取消"; }
            catch (Exception ex) { Log($"錯誤: {ex.Message}", Color.Red); }
            finally { UpdateUIState(false); progressBar1.Value = 0; if (_cts != null) _cts.Dispose(); }
        }

        private async Task ProcessSingleFileAsync(string filePath, GeminiService gemini, string modelName, string srtDir, string tempRoot, CancellationToken token)
        {
            string baseTemp = string.IsNullOrEmpty(tempRoot) ? Path.GetDirectoryName(filePath) : tempRoot;
            string tempDir = Path.Combine(baseTemp, "temp_" + Path.GetFileNameWithoutExtension(filePath));
            
            string baseOut = string.IsNullOrEmpty(srtDir) ? Path.GetDirectoryName(filePath) : srtDir;
            string logFilePath = Path.Combine(baseOut, Path.GetFileName(filePath) + ".log.txt");

            long totalInputTokens = 0;
            long totalOutputTokens = 0;

            void LogEvent(string msg, Color? color = null) 
            {
                Log(msg, color ?? Color.LightGray);
                WriteFileLog(logFilePath, msg);
            }

            try
            {
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                
                File.WriteAllText(logFilePath, $"=== 開始處理: {Path.GetFileName(filePath)} ===\n時間: {DateTime.Now}\n模型: {modelName}\n\n");

                LogEvent($"[1/4] 提取與分割音訊 (Model: {modelName})", Color.Cyan);
                await CheckPauseAndCancel(token);
                
                List<AudioSegmentInfo> segments = await Task.Run(() => AudioSplitter.SplitAudio(filePath, tempDir), token);
                
                if (segments.Count == 0) throw new Exception("音訊提取失敗或為空");
                LogEvent($"    -> 準備好 {segments.Count} 個音訊片段");

                progressBar1.Value = 0; progressBar1.Maximum = segments.Count;
                List<string> srtSegments = new List<string>();

                for (int j = 0; j < segments.Count; j++)
                {
                    await CheckPauseAndCancel(token);
                    var segment = segments[j];
                    string partSrtPath = Path.ChangeExtension(segment.FilePath, ".srt"); 
                    
                    if (File.Exists(partSrtPath))
                    {
                        LogEvent($"  -> 片段 {j + 1}/{segments.Count}: 讀取暫存檔 (跳過生成)", Color.LightGreen);
                        string cachedSrt = File.ReadAllText(partSrtPath);
                        srtSegments.Add(cachedSrt);
                        progressBar1.PerformStep();
                        continue; 
                    }
                    
                    string fileUri = null;
                    try
                    {
                        if (j > 0) { LogEvent($"  ... 冷卻中 (等待 {_requestInterval/1000} 秒) ..."); await Task.Delay(_requestInterval, token); }
                        LogEvent($"  -> 上傳片段 {j + 1}/{segments.Count}...");
                        fileUri = await ExecuteWithRetryAsync(async () => await gemini.UploadFileAsync(segment.FilePath), token);
                        await Task.Delay(2000, token);
                        LogEvent($"  -> 生成字幕...", Color.LightYellow);
                        
                        var result = await ExecuteWithRetryAsync(async () => await gemini.GenerateSrtFromUriAsync(fileUri, modelName), token);
                        
                        totalInputTokens += result.InputTokens;
                        totalOutputTokens += result.OutputTokens;

                        string srt = result.SrtText;
                        if (string.IsNullOrWhiteSpace(srt)) LogEvent("    (回傳為空，視為靜音)", Color.Gray);
                        
                        File.WriteAllText(partSrtPath, srt ?? ""); 
                        srtSegments.Add(srt ?? "");
                        LogEvent($"  -> 完成 (已暫存). Token: In={result.InputTokens}, Out={result.OutputTokens}", Color.LightGreen);
                    }
                    catch (Exception ex) 
                    { 
                        LogEvent($"  X 片段 {j + 1} 失敗: {ex.Message}", Color.Red); 
                        srtSegments.Add(""); 
                    }
                    finally { if (!string.IsNullOrEmpty(fileUri)) await gemini.DeleteFileAsync(fileUri); }
                    progressBar1.PerformStep();
                }

                await CheckPauseAndCancel(token);
                LogEvent($"[4/4] 智慧合併 (修正時間軸/重疊)...", Color.Cyan);
                string finalSrt = SrtMerger.MergeSrts(srtSegments, segments);
                string outputSrtPath = Path.Combine(baseOut, Path.ChangeExtension(Path.GetFileName(filePath), ".srt"));
                File.WriteAllText(outputSrtPath, finalSrt);
                LogEvent($"成功儲存: {outputSrtPath}", Color.Lime);

                string resourceStats = $"\n 資源使用\n* **Token 總用量**: {totalInputTokens + totalOutputTokens}\n    * **輸入 Token**: {totalInputTokens}\n    * **輸出 Token**: {totalOutputTokens}\n";
                Log(resourceStats, Color.White); 
                WriteFileLog(logFilePath, resourceStats); 
            }
            catch (OperationCanceledException) { LogEvent("=== 操作已取消 ===", Color.Orange); }
            catch (Exception ex) { LogEvent($"檔案錯誤: {ex.Message}", Color.Red); }
            finally { LogEvent("提示: 暫存檔保留以供中斷續傳，如無需使用請手動刪除 temp 資料夾"); }
        }

        private void WriteFileLog(string path, string message)
        {
            try { File.AppendAllText(path, $"[{DateTime.Now:HH:mm:ss}] {message}\r\n"); } catch { }
        }

        private void btnManualMerge_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog 
            { 
                Filter = "Media Files|*.mp3;*.wav;*.mp4;*.avi;*.mov;*.mpeg;*.mkv;*.m4a|Audio|*.mp3;*.wav|Video|*.mp4;*.avi;*.mov;*.mpeg;*.mkv", 
                Title = "選擇原始檔案以進行合併" 
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;
            string customTempDir = txtTempPath.Text.Trim();
            string customSrtDir = txtSrtPath.Text.Trim();
            string baseTemp = string.IsNullOrEmpty(customTempDir) ? Path.GetDirectoryName(filePath) : customTempDir;
            string tempDir = Path.Combine(baseTemp, "temp_" + Path.GetFileNameWithoutExtension(filePath));

            if (!Directory.Exists(tempDir)) { MessageBox.Show($"找不到暫存資料夾：\n{tempDir}", "錯誤"); return; }

            try
            {
                Log("開始手動合併...", Color.Cyan);
                var segments = new List<AudioSegmentInfo>();
                var mp3Files = Directory.GetFiles(tempDir, "part_*.mp3");
                Array.Sort(mp3Files, (a, b) => {
                    int idxA = int.Parse(Regex.Match(a, @"part_(\d+)").Groups[1].Value);
                    int idxB = int.Parse(Regex.Match(b, @"part_(\d+)").Groups[1].Value);
                    return idxA.CompareTo(idxB);
                });

                foreach (var f in mp3Files) using (var reader = new AudioFileReader(f)) segments.Add(new AudioSegmentInfo { FilePath = f, Duration = reader.TotalTime });

                var srtSegments = new List<string>();
                foreach (var seg in segments)
                {
                    string srtPath = Path.ChangeExtension(seg.FilePath, ".srt");
                    srtSegments.Add(File.Exists(srtPath) ? File.ReadAllText(srtPath) : "");
                }

                string finalSrt = SrtMerger.MergeSrts(srtSegments, segments);
                string baseOut = string.IsNullOrEmpty(customSrtDir) ? Path.GetDirectoryName(filePath) : customSrtDir;
                string outputSrtPath = Path.Combine(baseOut, Path.ChangeExtension(Path.GetFileName(filePath), ".srt"));
                File.WriteAllText(outputSrtPath, finalSrt);
                Log($"手動合併成功: {outputSrtPath}", Color.Lime);
                MessageBox.Show("合併完成！");
            }
            catch (Exception ex) { Log($"手動合併失敗: {ex.Message}", Color.Red); MessageBox.Show($"合併失敗: {ex.Message}"); }
        }

        private void btnAddFiles_Click(object sender, EventArgs e) 
        { 
            OpenFileDialog ofd = new OpenFileDialog 
            { 
                Filter = "Media Files|*.mp3;*.wav;*.mp4;*.avi;*.mov;*.mpeg;*.mkv;*.m4a|Audio|*.mp3;*.wav|Video|*.mp4;*.avi;*.mov;*.mpeg;*.mkv", 
                Multiselect = true 
            }; 
            if (ofd.ShowDialog() == DialogResult.OK) foreach (string file in ofd.FileNames) if (!lstFiles.Items.Contains(file)) lstFiles.Items.Add(file); 
        }

        private async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action, CancellationToken token, int maxRetries = 5)
        {
            for (int i = 1; i <= maxRetries; i++)
            {
                try { return await action(); }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("400") || ex.Message.Contains("INVALID_ARGUMENT")) throw;
                    if (i == maxRetries) throw;
                    bool isRateLimit = ex.Message.Contains("429") || ex.Message.Contains("RESOURCE_EXHAUSTED") || ex.Message.Contains("quota");
                    int waitSeconds = (int)Math.Pow(2, i);
                    if (isRateLimit)
                    {
                        var match = Regex.Match(ex.Message, @"retry in (\d+(\.\d+)?)s");
                        if (match.Success && double.TryParse(match.Groups[1].Value, out double serverWait))
                        {
                            waitSeconds = (int)Math.Ceiling(serverWait) + 1;
                            Log($"    (觸發限制) Google 要求等待 {waitSeconds} 秒...", Color.Orange);
                        }
                        else { Log($"    (觸發限制) 自動重試中 ({i}/{maxRetries})...", Color.Orange); }
                    }
                    else { Log($"    (連線錯誤) 重試中 ({i}/{maxRetries})...", Color.Orange); }
                    await Task.Delay(waitSeconds * 1000, token);
                }
            }
            return default;
        }
        
        private void ApplyDarkTheme() { menuStrip1.Renderer = new DarkMenuRenderer(); menuStrip1.BackColor = Color.FromArgb(30, 30, 30); menuStrip1.ForeColor = Color.White; }
        private void menuAbout_Click(object sender, EventArgs e) { MessageBox.Show("作者： Joseph\n信箱： pigqig@gmail.com\n網站名稱： 酷意 AI 平台", "關於"); }
        private void btnClear_Click(object sender, EventArgs e) { lstFiles.Items.Clear(); lblStatus.Text = "清單已清除"; }
        private void btnPause_Click(object sender, EventArgs e) { _isPaused = !_isPaused; btnPause.Text = _isPaused ? "▶ 繼續" : "⏸ 暫停"; btnPause.BackColor = _isPaused ? Color.FromArgb(40, 167, 69) : Color.FromArgb(255, 193, 7); if (_isPaused) Log(">>> 流程已暫停", Color.Yellow); else Log(">>> 流程繼續執行", Color.LightGreen); }
        private void btnCancel_Click(object sender, EventArgs e) { if (_cts != null) { _cts.Cancel(); Log(">>> 正在取消...", Color.Red); btnCancel.Enabled = false; } }
        private async Task CheckPauseAndCancel(CancellationToken token) { token.ThrowIfCancellationRequested(); while (_isPaused) { token.ThrowIfCancellationRequested(); await Task.Delay(500, token); } }
        private void UpdateUIState(bool isProcessing) { grpInput.Enabled = !isProcessing; btnAddFiles.Enabled = !isProcessing; btnClear.Enabled = !isProcessing; btnStart.Enabled = !isProcessing; lstFiles.Enabled = !isProcessing; btnPause.Enabled = isProcessing; btnCancel.Enabled = isProcessing; btnManualMerge.Enabled = !isProcessing; btnStart.BackColor = isProcessing ? Color.Gray : Color.FromArgb(40, 167, 69); }
        private void Log(string msg, Color color) { if (InvokeRequired) { Invoke(new Action(() => Log(msg, color))); return; } rtbLog.SelectionStart = rtbLog.TextLength; rtbLog.SelectionLength = 0; rtbLog.SelectionColor = color; rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n"); rtbLog.ScrollToCaret(); }
        private class DarkMenuRenderer : ToolStripProfessionalRenderer { public DarkMenuRenderer() : base(new DarkColors()) { } }
        private class DarkColors : ProfessionalColorTable { public override Color MenuItemSelected => Color.FromArgb(60, 60, 60); public override Color MenuItemBorder => Color.FromArgb(80, 80, 80); public override Color MenuItemPressedGradientBegin => Color.FromArgb(40, 40, 40); public override Color MenuItemPressedGradientEnd => Color.FromArgb(40, 40, 40); public override Color ToolStripDropDownBackground => Color.FromArgb(30, 30, 30); public override Color ImageMarginGradientBegin => Color.FromArgb(30, 30, 30); public override Color ImageMarginGradientMiddle => Color.FromArgb(30, 30, 30); public override Color ImageMarginGradientEnd => Color.FromArgb(30, 30, 30); }
    }
}