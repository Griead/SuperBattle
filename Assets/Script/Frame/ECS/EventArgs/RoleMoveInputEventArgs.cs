using UnityEngine;

public class RoleMoveInputEventArgs : ComponentEventArgs
{
    public Vector2 Direction { get; set; }

    public float DeltaTime {get; set;}

}