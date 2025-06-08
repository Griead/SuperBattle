using UnityEngine;

public class BaseMono : MonoBehaviour
{
    protected virtual void OnAwake()
    {
        
    }

    protected virtual void OnStart()
    {
        
    }
    
    protected virtual void OnUpdate(float deltaTime)
    {
        
    }
    
    protected virtual void OnFixedUpdate(float fixedDeltaTime)
    {
        
    }
    
    protected virtual void OnRelease()
    {
        
    }
    
    private void Awake()
    {
        OnAwake();
    }
    
    private void Start()
    {
        OnStart();
    }
    
    private void Update()
    {
        OnUpdate(Time.deltaTime);
    }
    
    private void FixedUpdate()
    {
        OnFixedUpdate(Time.fixedDeltaTime);
    }
    
    private void OnDestroy()
    {
        OnRelease();
    }
}