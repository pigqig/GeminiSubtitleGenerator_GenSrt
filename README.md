# 酷意 AI 字幕產生器 (CoolIdea AI Subtitle Generator)

**Studio v6.0 ** | Author: Joseph
**官網**：[酷意 AI 平台](http://ourcoolidea.com)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📖 專案簡介 (Introduction)

**酷意 AI 字幕產生器** 是一款專為影片創作者開發的高效能自動化工具，深度整合 Google Gemini AI 技術。

本軟體旨在解決傳統字幕製作中繁瑣的「聽打」與「時間軸校對」痛點。透過自動化的工作流程，系統能將影片中的語音精準轉換為字幕。針對長時間的影片，系統內建智慧分割與合併演算法，能有效突破 AI 模型的 Token 上限與連線逾時問題，實現一鍵產出高品質 SRT 字幕檔。

**詳細介面可以查看：
**產品說明書.pdf**

Gemini API 申請位置，需先申請 API Key 才能使用本軟體：
https://aistudio.google.com/api-keys

Gemini API Key 申請說明：
https://ai.google.dev/gemini-api/docs/api-key?hl=zh-tw

使用 NetFramework 6，下載位址：
https://dotnet.microsoft.com/zh-tw/download/dotnet/thank-you/sdk-6.0.428-windows-x64-installer


---

## ✨ 核心特色 (Features)

* **強大 AI 核心驅動**：支援 Google Gemini 系列模型（如 `gemini-2.5-pro`），具備優異的語意理解與多語言聽寫能力。
* **長影片自動分割機制**：系統會自動偵測影片長度，將音軌切割為多個 MP3 片段進行分批處理，確保長時間影片也能穩定生成字幕。
* **全自動批次流程**：支援拖曳影片至待處理清單，軟體將自動執行「影音分離」、「音訊切割」、「雲端辨識」至「最終合併」的所有步驟。
* **智慧時間軸合併 (Smart Merge)**：分段生成的字幕會由系統自動計算時間偏移量，精準合併為單一完整的 `.srt` 檔案，無需人工手動拼接。
* **透明化資源監控**：內建詳細的執行日誌 (Log)，即時顯示 API 連線狀態、處理進度以及 Token 消耗統計，方便使用者控管成本。

---

## 💎 進階版功能 (Advanced Version)

本專案另提供付費進階版本，針對有專業製作需求與系統效能極致要求的用戶提供以下升級：

### 1. ✂️ AI 自動短影音剪輯 (AI Smart Shorts)
**專為抖音 (TikTok)、Reels 與 YouTube Shorts 打造的殺手級功能。**
AI 會深度分析長影片內容，自動識別並挑選出數個最具吸引力的「精彩片段 (Highlights)」。系統將直接把這些精華片段剪輯成適合社群傳播的 30~60 秒直式/橫式短影音，並同步完成字幕內嵌。讓您一鍵實現「長片轉短片」的二次創作，輕鬆搶佔社群流量。

### 2. 🌐 輸出可自訂語言 (Multi-Language Output)
打破語言隔閡，進階版支援在生成字幕的同時進行翻譯。您可以指定輸出的目標語言（例如：將中文語音直接生成為英文或日文字幕），大幅降低內容國際化的門檻。

### 3. 🎬 字幕直接內嵌 (Direct Subtitle Embedding)
支援「硬字幕 (Hardsub)」壓制功能。系統能將 AI 生成的字幕直接燒錄至影片畫面中，產出已帶有字幕的影片檔，觀眾無需額外掛載字幕檔即可觀看。

### 4. 🚀 更強大的轉碼核心 (Enhanced Transcoding Core)
採用企業級影音編碼引擎，不僅支援更多特殊的影片編碼格式，更能大幅提升處理 4K 高畫質或高流量影片時的轉檔效率與穩定性。

### 5. 🖥️ 縮小至系統列 (Minimize to Tray)
支援將程式最小化至 Windows 系統列 (System Tray)。當您進行大量影片的批次處理或長時間掛機運算時，可將軟體隱藏於背景執行，保持桌面清爽，不干擾其他日常工作進行。

### 6. 🧹 關閉前強制清理 (Force Cleanup on Exit)
優化資源管理機制。當使用者關閉軟體時，系統會強制掃描並終止所有相關的外部轉碼子程序。徹底杜絕背景殘留程序持續佔用 CPU 與記憶體效能的問題，確保電腦在軟體關閉後立即恢復最佳狀態。

> **💡 如有進階版相關需求，請直接聯繫 [酷意 AI 平台](http://ourcoolidea.com)。**


## 🛠️ 環境設定 (Configuration)

在開始使用前，請於軟體主介面完成以下基礎參數設定：

1.  **API Key**：輸入您的 Google Gemini API 金鑰以啟用雲端服務。
2.  **Gemini 模型**：從下拉選單選擇欲使用的模型版本（系統預設支援 `gemini-2.5-pro` 等高效能模型）。
3.  **路徑設定**：
    * **字幕儲存**：指定生成後的 `.srt` 檔案存放位置。
    * **音效暫存**：指定中間過程產生的暫存音訊檔存放位置（處理完成後可手動清除）。

---

## 🚀 運作原理與流程 (Workflow)

本軟體將複雜的字幕製作簡化為以下四個自動化階段：

### 1. 提取與分割 (Extraction & Splitting)
當您將影片加入清單並開始處理時，系統首先會提取影片音軌，並將其物理切割為數個較小的 MP3 檔案（例如命名為 `part_000.mp3`, `part_001.mp3`...）。此舉是為了確保單次請求不會超出 API 的處理極限。

### 2. AI 雲端運算 (Cloud Processing)
系統將依序上傳分割後的音訊片段至 Gemini 進行分析。執行日誌會即時顯示當前的處理狀態（例如：`上傳片段 2/5...`、`生成字幕...`）。系統內建冷卻機制 (Cool-down)，在片段之間自動調節請求頻率，以維持連線穩定。

### 3. 分段字幕生成 (Segment Generation)
AI 分析完成後，會回傳包含精確時間碼的 `.srt` 文字檔。此時，您會在暫存資料夾中看到與音訊片段對應的字幕檔（如 `part_000.srt`）。這些片段各自擁有獨立的時間軸。

### 4. 智慧合併與輸出 (Smart Merge & Output)
當所有片段皆處理完畢，系統會啟動「智慧合併」程序。此演算法會將所有分段字幕串接，並自動依據片段順序修正時間軸偏移，最終產出一個與原始影片同名的完整 SRT 檔案，實現影音同步。

---

## 📊 效能與統計 (Statistics)

為了協助使用者掌握 API 使用量，軟體會在每次任務結束後於 Log 視窗顯示詳細的資源統計報告：

* **Total Token**：本次任務消耗的總 Token 數量。
* **Input Token**：包含音訊數據與提示詞的輸入量。
* **Output Token**：AI 生成字幕文字的輸出量。

---

## 📝 License

Distributed under the MIT License. See below for more information.

**MIT License**

Copyright (c) 2025 Joseph (CoolIdea)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
