using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public static class JsonSaveSystem<T> where T : class
{
    private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "SaveData");

    private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
#if UNITY_EDITOR
        Formatting = Formatting.Indented, // Dễ đọc khi ở Editor
#else
        Formatting = Formatting.None, // Tiết kiệm dung lượng khi build
#endif
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include,
        TypeNameHandling = TypeNameHandling.Auto
    };

    // --- Quản lý Lock (Chống Race Condition) ---

    // Dictionary để quản lý các "khóa" (Semaphore) cho từng file
    private static readonly Dictionary<string, SemaphoreSlim> FileLocks = new Dictionary<string, SemaphoreSlim>();
    // Object dùng để khóa việc truy cập vào Dictionary FileLocks
    private static readonly object LockObject = new object();

    /// <summary>
    /// Lấy hoặc tạo một Semaphore (khóa) cho một đường dẫn file cụ thể.
    /// </summary>
    private static SemaphoreSlim GetFileLock(string filePath)
    {
        lock (LockObject)
        {
            if (!FileLocks.TryGetValue(filePath, out var semaphore))
            {
                // Chỉ cho phép 1 luồng truy cập file tại một thời điểm
                semaphore = new SemaphoreSlim(1, 1); 
                FileLocks[filePath] = semaphore;
            }
            return semaphore;
        }
    }
    
    /// <summary>
    /// Lưu dữ liệu async (không block main thread)
    /// </summary>
    public static async UniTask<bool> SaveAsync(T data, string fileName, CancellationToken cancellationToken = default)
    {
        string filePath = GetFilePath(fileName);
        var fileLock = GetFileLock(filePath);

        // Chờ để "chiếm" được khóa file
        await fileLock.WaitAsync(cancellationToken);
        
        try
        {
            // Tạo thư mục (nếu cần) - Directory.CreateDirectory đủ thông minh
            // để không làm gì nếu thư mục đã tồn tại.
            if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);

            // 1. Tác vụ nặng CPU: Serialize JSON trên ThreadPool
            string json = await UniTask.RunOnThreadPool(() =>
                    JsonConvert.SerializeObject(data, JsonSettings),
                cancellationToken: cancellationToken
            );

            // 2. Tác vụ nặng I/O: Ghi file async
            await File.WriteAllTextAsync(filePath, json, cancellationToken);
            
            Debug.Log($"[JsonSaveSystem] Đã lưu async {fileName}");
            return true;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"[JsonSaveSystem] Save {fileName} đã bị cancel");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonSaveSystem] Lỗi khi lưu async {fileName}: {e.Message}");
            return false;
        }
        finally
        {
            // Rất quan trọng: Luôn "nhả" khóa ra, kể cả khi lỗi
            fileLock.Release();
        }
    }

    /// <summary>
    /// Tải dữ liệu async (không block main thread)
    /// </summary>
    public static async UniTask<T> LoadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        string filePath = GetFilePath(fileName);
        var fileLock = GetFileLock(filePath);

        // Chờ để "chiếm" được khóa file
        await fileLock.WaitAsync(cancellationToken);
        
        try
        {
            // 1. Tác vụ I/O: Kiểm tra file (chuyển sang async)
            if (!await ExistsInternalAsync(filePath, cancellationToken))
            {
                Debug.LogWarning($"[JsonSaveSystem] File {fileName} không tồn tại!");
                return null;
            }

            // 2. Tác vụ nặng I/O: Đọc file async
            string json = await File.ReadAllTextAsync(filePath, cancellationToken);

            // 3. Tác vụ nặng CPU: Deserialize JSON trên ThreadPool
            T data = await UniTask.RunOnThreadPool(() =>
                    JsonConvert.DeserializeObject<T>(json, JsonSettings),
                cancellationToken: cancellationToken
            );
            
            Debug.Log($"[JsonSaveSystem] Đã tải async {fileName}");
            return data;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"[JsonSaveSystem] Load {fileName} đã bị cancel");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonSaveSystem] Lỗi khi tải async {fileName}: {e.Message}");
            return null;
        }
        finally
        {
            // Luôn "nhả" khóa
            fileLock.Release();
        }
    }

    /// <summary>
    /// Tải hoặc tạo mới async (đã tối ưu createDefault)
    /// </summary>
    public static async UniTask<T> LoadOrCreateAsync(string fileName, Func<T> createDefault,
        CancellationToken cancellationToken = default)
    {
        // Dùng ExistsAsync thay vì Exists
        if (await ExistsAsync(fileName, cancellationToken))
        {
            return await LoadAsync(fileName, cancellationToken);
        }

        // Tác vụ CPU: Chạy hàm tạo data default trên ThreadPool
        T defaultData = await UniTask.RunOnThreadPool(createDefault, cancellationToken: cancellationToken);
        
        // Lưu data default mới
        await SaveAsync(defaultData, fileName, cancellationToken);
        return defaultData;
    }
    
    /// <summary>
    /// Tải nhiều file song song
    /// </summary>
    public static async UniTask<T[]> LoadMultipleAsync(string[] fileNames, CancellationToken cancellationToken = default)
    {
        var tasks = new UniTask<T>[fileNames.Length];
        
        for (int i = 0; i < fileNames.Length; i++)
        {
            tasks[i] = LoadAsync(fileNames[i], cancellationToken);
        }

        // Chờ tất cả các task LoadAsync hoàn thành
        return await UniTask.WhenAll(tasks);
    }
    
    /// <summary>
    /// Xóa file async (không block main thread)
    /// </summary>
    public static async UniTask<bool> DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        string filePath = GetFilePath(fileName);
        var fileLock = GetFileLock(filePath); // Dùng chung lock

        await fileLock.WaitAsync(cancellationToken);
        
        try
        {
            if (!await ExistsInternalAsync(filePath, cancellationToken))
            {
                Debug.LogWarning($"[JsonSaveSystem] File {fileName} không tồn tại!");
                return false;
            }

            // Tác vụ I/O: Xóa file trên ThreadPool
            await UniTask.RunOnThreadPool(() => File.Delete(filePath), cancellationToken: cancellationToken);
            
            Debug.Log($"[JsonSaveSystem] Đã xóa {fileName}");
            return true;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"[JsonSaveSystem] Delete {fileName} đã bị cancel");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonSaveSystem] Lỗi khi xóa {fileName}: {e.Message}");
            return false;
        }
        finally
        {
            fileLock.Release();
        }
    }
    
    /// <summary>
    /// Kiểm tra file tồn tại async (không block main thread)
    /// </summary>
    public static async UniTask<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        string filePath = GetFilePath(fileName);
        // Chạy kiểm tra trên ThreadPool
        return await ExistsInternalAsync(filePath, cancellationToken);
    }
    
    // --- Các hàm tiện ích (Internal) ---

    /// <summary>
    /// Hàm Exists nội bộ, chạy trên ThreadPool (tránh block)
    /// </summary>
    private static async UniTask<bool> ExistsInternalAsync(string filePath, CancellationToken cancellationToken)
    {
        return await UniTask.RunOnThreadPool(() => File.Exists(filePath), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Lấy đường dẫn file đầy đủ và chuẩn hóa tên file
    /// </summary>
    private static string GetFilePath(string fileName)
    {
        if (!fileName.EndsWith(".json"))
        {
            fileName += ".json";
        }
        return Path.Combine(SaveDirectory, fileName);
    }
}