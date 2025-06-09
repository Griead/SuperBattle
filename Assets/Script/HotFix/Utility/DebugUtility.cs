using System.Text;
using UnityEngine;

/// <summary>
/// 调试信息方法
/// </summary>
public class DebugUtility
{
    public static void Log(params object[] parames)
    {
        StringBuilder _sb = new StringBuilder();
        foreach (var parame in parames)
        {
            _sb.Append(parame);
        }
        Debug.Log(_sb.ToString());
    }
    
    public static void LogError(params object[] parames)
    {
        StringBuilder _sb = new StringBuilder();
        foreach (var parame in parames)
        {
            _sb.Append(parame);
        }
        Debug.LogError(_sb.ToString());
    }
}