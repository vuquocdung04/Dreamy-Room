using UnityEngine;
using UnityEditor; 
using System.IO;   

// Đặt file này trong thư mục "Assets/Editor"

public class LocalizationImporter
{
    // 1. Đường dẫn đến file CSV của bạn
    private static string csvPath = "Assets/00_BaseGame/00_Script/00_Controller/LocalizationController/Localcsv.csv"; 
    
    // 2. Đường dẫn lưu ScriptableObject
    private static string soPath = "Assets/00_BaseGame/00_Script/00_Controller/LocalizationController/LocalizationData.asset";

    [MenuItem("Localization/Import CSV (Skip 2 Headers)")]
    public static void ImportCSV()
    {
        // 3. Tìm hoặc tạo mới ScriptableObject
        LocalizationData data = AssetDatabase.LoadAssetAtPath<LocalizationData>(soPath);
        if (data == null)
        {
            data = ScriptableObject.CreateInstance<LocalizationData>();
            AssetDatabase.CreateAsset(data, soPath);
        }

        data.entries.Clear(); // Xóa dữ liệu cũ

        // 4. Đọc file CSV
        string[] lines;
        try
        {
            lines = File.ReadAllLines(csvPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Không tìm thấy file CSV tại: {csvPath}. Lỗi: {e.Message}");
            return;
        }

        // 5. THAY ĐỔI QUAN TRỌNG:
        // Bắt đầu vòng lặp từ i = 2 (bỏ qua dòng 0 và 1)
        for (int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue; 

            string[] values = line.Split(','); 

            // Đảm bảo file của bạn có 3 cột
            if (values.Length < 3)
            {
                Debug.LogWarning($"Bỏ qua dòng {i}: Dòng không có đủ 3 cột.");
                continue;
            }

            // 6. Tạo mục nhập mới
            TranslationEntry entry = new TranslationEntry();
            
            // Đổi tên cột ENG/VIE trong file CSV cho khớp
            entry.key = values[0].Trim(); // Cột 0 là KEY
            entry.EN = values[1].Trim();  // Cột 1 là ENG
            entry.VI = values[2].Trim();  // Cột 2 là VIE
            
            data.entries.Add(entry);
        }

        // 7. Lưu lại thay đổi
        EditorUtility.SetDirty(data); 
        AssetDatabase.SaveAssets();   
        AssetDatabase.Refresh();      
        
        Debug.Log($"Import CSV thành công! Đã thêm {data.entries.Count} mục.");
    }
}