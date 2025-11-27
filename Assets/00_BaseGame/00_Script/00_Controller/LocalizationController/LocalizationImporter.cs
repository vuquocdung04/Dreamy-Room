using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

public static class LocalizationImporter
{
    private static readonly Regex CsvSplitRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

    /// <summary>
    /// Import từ Google Sheets URL
    /// </summary>
    public static async UniTask ImportFromGoogleSheets(string googleSheetUrl, string localPath, string soPath)
    {
        if (string.IsNullOrEmpty(googleSheetUrl))
        {
            Debug.LogError("Chưa có URL Google Sheets!");
            return;
        }

        Debug.Log("Đang tải từ Google Sheets...");
        UnityWebRequest www = UnityWebRequest.Get(googleSheetUrl);
        await www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Lỗi: {www.error}");
            return;
        }

        string csvContent = www.downloadHandler.text;

        // Lưu file về local
        SaveToLocalFile(csvContent, localPath);

        // Import vào ScriptableObject
        ImportCsv(csvContent, soPath);
    }

    /// <summary>
    /// Import từ file CSV local
    /// </summary>
    public static void ImportFromLocalFile(string localPath, string soPath)
    {
        if (!File.Exists(localPath))
        {
            Debug.LogError($"Không tìm thấy file tại: {localPath}");
            return;
        }

        string csvContent = File.ReadAllText(localPath);
        ImportCsv(csvContent, soPath);
    }

    /// <summary>
    /// Lưu CSV content vào file local
    /// </summary>
    private static void SaveToLocalFile(string csvContent, string localPath)
    {
        try
        {
            string directory = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }

            File.WriteAllText(localPath, csvContent);
            Debug.Log($"Đã lưu file local tại: {localPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Lỗi khi lưu file local: {e.Message}");
        }
    }

    /// <summary>
    /// Parse CSV content và import vào LocalizationData ScriptableObject
    /// </summary>
    private static void ImportCsv(string csvContent, string soPath)
    {
        LocalizationData data = AssetDatabase.LoadAssetAtPath<LocalizationData>(soPath);
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<LocalizationData>();
            AssetDatabase.CreateAsset(data, soPath);
        }

        List<TranslationEntry> newEntries = new List<TranslationEntry>();
        string[] lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        int importedCount = 0;
        for (int i = 2; i < lines.Length; i++) // Bỏ qua 2 dòng header
        {
            string line = lines[i];
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = CsvSplitRegex.Split(line);

            if (values.Length < 3)
            {
                Debug.LogWarning($"Dòng {i + 1} có định dạng CSV không hợp lệ, bỏ qua: {line}");
                continue;
            }

            TranslationEntry entry = new TranslationEntry
            {
                key = CleanValue(values[0]),
                EN = CleanValue(values[1]),
                VI = CleanValue(values[2])
            };

            newEntries.Add(entry);
            importedCount++;
        }

        data.entries.Clear();
        data.entries.AddRange(newEntries);

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();

        Debug.Log($"✅ Import thành công {importedCount} mục!");
    }

    /// <summary>
    /// Làm sạch giá trị CSV (xóa dấu ngoặc kép và escape characters)
    /// </summary>
    private static string CleanValue(string value)
    {
        if (string.IsNullOrEmpty(value)) return "";

        string result = value.Trim();

        if (result.Length > 1 && result.StartsWith("\"") && result.EndsWith("\""))
        {
            result = result.Substring(1, result.Length - 2);
        }

        result = result.Replace("\"\"", "\"");

        return result;
    }
}