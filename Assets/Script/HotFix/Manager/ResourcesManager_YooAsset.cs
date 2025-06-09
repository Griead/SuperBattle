using System.Collections;
using YooAsset;

public partial class ResourcesManager
{
    private static ResourcePackage YooAssetPackage;

    public IEnumerator YooAssetInit()
    {
        YooAssetPackage = YooAssets.GetPackage("DefaultPackage");
#if UNITY_WEBGL
        YooAssets.SetCacheSystemDisableCacheOnWebGL();
#endif
        yield break;
        // int downloadingMaxNum = 10;
        // int failedTryAgain = 3;
        // var downloader = YooAssetPackage.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        // //没有需要下载的资源
        // if (downloader.TotalDownloadCount == 0)
        // {        
        //     yield break;
        // }
        //
        // //需要下载的文件总数和总大小
        // int totalDownloadCount = downloader.TotalDownloadCount;
        // long totalDownloadBytes = downloader.TotalDownloadBytes;   
        //
        // //开启下载
        // downloader.BeginDownload();
        // yield return downloader;
        //
        // //检测下载结果
        // if (downloader.Status == EOperationStatus.Succeed)
        // {
        //     //下载成功
        //     Debug.Log("下载全部资源包成功 " + totalDownloadCount + "个文件，" + totalDownloadBytes + "字节");
        // }
        // else
        // {
        //     //下载失败
        //     Debug.Log("下载全部资源包失败" + downloader.Error);
        // }
    }

    private void YooAssetRelease()
    {
#if UNITY_WEBGL

#else
        YooAssets.DestroyPackage("DefaultPackage");
        YooAssetPackage = null;
#endif
    }
}