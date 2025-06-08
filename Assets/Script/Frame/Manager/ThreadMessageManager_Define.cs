/// <summary>
///  线程消息
/// </summary>
public struct ThreadMessageDefine
{
    /// <summary> 获得门服务器地址 </summary>
    public const string ReceiveGateAddr = "ReceiveGateAddr";

    /// <summary> 获得账号下的角色列表 </summary>
    public const string ReceiveAccountRole = "ReceiveAccountRole";

    /// <summary> 登录成功 </summary>
    public const string ReceiveLoginSucceed = "ReceiveLoginSucceed";

    /// <summary> 打开服务器等待 </summary>
    public const string OpenServerLoadingPanel = "OpenServerLoadingPanel";

    /// <summary> 关闭服务器等待 </summary>
    public const string CloseServerLoadingPanel = "CloseServerLoadingPanel";

    /// <summary> 登录 </summary>
    public const string Login = "Login";

    /// <summary> 退出登录 </summary>
    public const string LoginOut = "LoginOut";
    
    /// <summary> 注册 </summary>
    public const string SignIn = "SignIn";

    /// <summary> 保存存档 </summary>
    public const string SaveArchive = "SaveArchive";

    /// <summary> 创建角色 </summary>
    public const string CreateRoleHandle = "CreateRoleHandle";

    /// <summary> 选择角色 </summary>
    public const string ChooseRoleHandle = "CreateRoleChooseHandle";
    
    /// <summary> 创建行为记录 </summary>
    public const string CreateBehaviorRecord = "CreateBehaviorRecord";
    
    /// <summary> 获取行为记录 </summary>
    public const string ReceiveBehaviorRecord = "ReceiveBehaviorRecord";

    /// <summary> 请求行为记录 </summary>
    public const string CheckBehaviorRecord = "CheckBehaviorRecord";

    /// <summary> 请求主页微博 </summary>
    public const string PlatBlogHomeRequest = "PlatBlogHomeRequest";
    
    /// <summary> 请求热门微博 </summary>
     public const string PlatBlogHotRequest = "PlatBlogHotRequest";
    
    /// <summary> 请求历史微博 </summary>
     public const string PlatBlogHistoryRequest = "PlatBlogHistoryRequest";
    
    /// <summary> 请求搜索微博 </summary>
     public const string PlatBlogSearchRequest = "PlatBlogSearchRequest";
    
    /// <summary>
    /// 请求搜索微博关键字
    /// </summary>
     public const string PlatBlogSearchKeyRequest = "PlatBlogSearchKeyRequest";
    
     /// <summary> 请求发现微博 </summary>
     public const string PlatBlogExploreRequest = "PlatBlogExploreRequest";
  
     /// <summary> 微博喜欢列表请求 </summary>
     public const string PlatBlogLikeListRequest = "PlatBlogLikeListRequest";
  
     /// <summary> 微博评论列表请求 </summary>
     public const string PlatBlogCommentListRequest = "PlatBlogCommentListRequest";
}