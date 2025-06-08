using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using YooAsset;

public class FileUtility
{
    public static bool CheckFolderOrCreate(string path)
    {
        bool result = Directory.Exists(path);
        if (!result)
        {
            Directory.CreateDirectory(path);
        }

        return result;
    }

    /// <summary>
    /// 写入Json文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="config"></param>
    /// <typeparam name="T"></typeparam>
    public static void WriteFileJson<T>(string path, T config) where T : class, new()
    {
        if (config is null)
        {
            DebugUtility.LogError("数据为null");
        }

        var setting = new Newtonsoft.Json.JsonSerializerSettings()
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
        };
        var content = Newtonsoft.Json.JsonConvert.SerializeObject(config, Newtonsoft.Json.Formatting.Indented, setting);
        File.WriteAllText(path, content, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// Json读取成数据通过资源管理器
    /// </summary>
    /// <param name="fileName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadJsonToData<T>(string fileName) where T : class, new()
    {
        string content = ReadFileToString(fileName);
        return DeserializeJson<T>(content);
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string ReadFileToString(string filePath)
    {
        var textAsset = GlobalManager.Instance.GetModel<ResourcesManager>().LoadAsset<TextAsset>(filePath);
        return System.Text.Encoding.UTF8.GetString(textAsset.bytes).Trim();
    }

    // /// <summary>
    // /// 读取文件异步
    // /// </summary>
    // /// <param name="filePath"></param>
    // /// <param name="callback"></param>
    // public static Task<TextAsset> ReadFileToStringAsync(string filePath, Action<string> callback = null)
    // {
    //     Action<AssetHandle> action = (assetHandle) =>
    //     {
    //         var textAsset = assetHandle.GetAssetObject<TextAsset>();
    //         Debug.Log("异步回调 " + filePath + textAsset.bytes);
    //         var result = System.Text.Encoding.UTF8.GetString(textAsset.bytes).Trim();
    //         callback?.Invoke(result);
    //     };
    //     
    //     var task = GlobalManager.Instance.GetModel<ResourcesManager>().LoadAssetAsync<TextAsset>(filePath, action: action);
    //     return task;
    // }

    /// <summary>
    /// Json读取成数据 资源目录外
    /// </summary>
    /// <param name="fileName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ReadJsonToData_OutAsset<T>(string fileName) where T : class, new()
    {
        string content = ReadFileToString_OutAsset(fileName);
        return DeserializeJson<T>(content);
    }

    /// <summary>
    /// 读取文件 资源目录外
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static string ReadFileToString_OutAsset(string filePath)
    {
        // 直接同步读取了
        var fs = File.ReadAllText(filePath);
        return fs;

        // try
        // {
        //     // Create an instance of StreamReader to read from a file.
        //     // The using statement also closes the StreamReader.
        //     using (StreamReader sr = new StreamReader(filePath))
        //     {
        //         // Read and display lines from the file until the end of
        //         // the file is reached.
        //         while (sr.Peek() > 0)
        //         {
        //             sr.ReadLine();
        //         }
        //         
        //     }
        // }
        // catch (Exception e)
        // {
        //     // Let the user know what went wrong.
        //     Console.WriteLine("The file could not be read:");
        //     Console.WriteLine(e.Message);
        // }
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="content"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T DeserializeJson<T>(string content) where T : class, new()
    {
        var setting = new Newtonsoft.Json.JsonSerializerSettings()
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
        };
        var config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content, setting);
        return config;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    private IEnumerator UrlDownLoadFile(string _path)
    {
        string _webUrl = "https://bjxp.studio/0516/Debug/ServerList/Servers.json";

        UnityWebRequest request = UnityWebRequest.Get(_webUrl);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            DebugUtility.LogError("下载失败", request.error);
        }
        else
        {
            string jsonText = request.downloadHandler.text;
        }
    }
}