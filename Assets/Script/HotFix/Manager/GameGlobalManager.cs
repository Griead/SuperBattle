public class GameGlobalManager : IGameManager
{
    private RoleSprite RoleSprite;
    
    public void Init()
    {
        
    }

    public void Dispose()
    {
        
    }

    public void SetRoleSprite(RoleSprite roleSprite)
    {
        RoleSprite = roleSprite;
    }

    public RoleSprite GetRoleSprite()
    {
        return RoleSprite;
    }
}