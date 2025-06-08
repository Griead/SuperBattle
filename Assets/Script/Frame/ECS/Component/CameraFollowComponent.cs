using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

// 首先在ModuleType枚举中添加摄像机类型
public enum CameraFollowType
{
    Smooth, // 平滑跟随
    Lerp, // 线性插值
    Damping, // 阻尼跟随
    Prediction // 预测跟随
}

public class CameraFollowComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.CameraFollow;

    private Camera followCamera;
    private Transform cameraTransform;

    public CameraFollowType FollowType { get; set; } = CameraFollowType.Lerp;
    public float SmoothTime { get; set; } = 1;
    public float MaxSpeed { get; set; } = 10f;
    public Vector3 Offset { get; set; } = new Vector3(0, 0, -40f);
    public Vector2 DeadZone { get; set; } = new Vector2(1f, 1f);
    public bool LookAhead { get; set; } = true;
    public float LookAheadDistance { get; set; } = 2f;
    public float LookAheadSmoothTime { get; set; } = 1f;

    // 边界限制
    public bool UseBounds { get; set; } = false;
    public Bounds CameraBounds { get; set; }

    // 内部变量
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 lastTargetPosition;
    private Vector3 lookAheadOffset;
    private Vector3 lookAheadVelocity;

    // 防撕裂相关
    private bool useFixedUpdate = true;
    private Vector3 smoothedPosition;
    private Vector3 interpolationStartPos;
    private Vector3 interpolationTargetPos;
    private float interpolationProgress;

    public override void Initialize(BaseSprite baseSprite)
    {
        this.Owner = baseSprite;

        this.followCamera = Camera.main;
        if (followCamera == null)
        {
            GameObject cameraObj = new GameObject("FollowCamera");
            followCamera = cameraObj.AddComponent<Camera>();
            followCamera.tag = "MainCamera";
        }

        cameraTransform = this.followCamera.transform;
        lastTargetPosition = Owner.transform.position;

        // 尝试获取最大速度
        if (Owner is RoleSprite roleSprite)
            MaxSpeed = roleSprite.Speed;
        
        // 初始化相机位置
        UpdateCameraPosition(0f);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (!useFixedUpdate)
        {
            UpdateCameraPosition(deltaTime);
        }
        else
        {
            // 在Update中进行平滑插值,防止画面撕裂
            InterpolateCamera(deltaTime);
        }
    }

    public override void FixedUpdate(float deltaTime)
    {
        base.FixedUpdate(deltaTime);

        if (useFixedUpdate)
        {
            CalculateTargetPosition(deltaTime);
        }
    }

    #region FixedUpdate

    /// <summary>
    /// 计算目标位置
    /// </summary>
    /// <param name="fixedDeltaTime"></param>
    private void CalculateTargetPosition(float fixedDeltaTime)
    {
        Vector3 ownerPosition = Owner.transform.position;

        // 计算基础目标位置
        Vector3 baseTarget = ownerPosition + Offset;

        // 预测性跟随 根据移动方向提前移动相机
        if (LookAhead)
        {
            var playerVelocity = (ownerPosition - lastTargetPosition) / fixedDeltaTime;
            var desiredLookAhead = playerVelocity.normalized * LookAheadDistance;

            lookAheadOffset = Vector3.SmoothDamp(
                lookAheadOffset,
                desiredLookAhead,
                ref lookAheadVelocity,
                LookAheadSmoothTime);

            baseTarget += lookAheadOffset;
        }
        
        // 死区处理 移动范围太小 则不去移动
        Vector3 deltaMove = baseTarget - cameraTransform.position;
        if (Mathf.Abs(deltaMove.x) < DeadZone.x)
            baseTarget.x = cameraTransform.position.x;
        if (Mathf.Abs(deltaMove.y) < DeadZone.y)
            baseTarget.y = cameraTransform.position.y;
        
        // 边界限制
        if (UseBounds)
        {
            baseTarget = ClampToBounds(baseTarget);
        }

        // 设置插值
        interpolationStartPos = smoothedPosition;
        interpolationTargetPos = baseTarget;
        interpolationProgress = 0;

        lastTargetPosition = ownerPosition;
    }

    private void InterpolateCamera(float deltaTime)
    {
        // 在Update中进行平滑插值
        interpolationProgress += deltaTime / Time.fixedDeltaTime;
        interpolationProgress = Mathf.Clamp01(interpolationProgress);

        // 根据跟随类型选择插值方法
        switch (FollowType)
        {
            case CameraFollowType.Smooth:
                smoothedPosition = Vector3.SmoothDamp(
                    interpolationStartPos,
                    interpolationTargetPos,
                    ref velocity,
                    SmoothTime,
                    MaxSpeed
                );
                break;
            case CameraFollowType.Lerp:
                float lerpSpeed = 1f / SmoothTime;
                smoothedPosition = Vector3.Lerp(
                    interpolationStartPos,
                    interpolationTargetPos,
                    lerpSpeed * deltaTime
                );
                break;
            case CameraFollowType.Damping:
                smoothedPosition = Vector3.Lerp(
                    interpolationStartPos,
                    interpolationTargetPos,
                    interpolationProgress
                );
                break;
            case CameraFollowType.Prediction:
                // 预测性插值， 考虑玩家速度
                Vector3 predictedPos = interpolationTargetPos;
                if (LookAhead)
                {
                    Vector3 playerVel = (Owner.transform.position - interpolationStartPos) / deltaTime;
                    Vector3 playerVelOutZ = new Vector3(playerVel.x, playerVel.y, Owner.transform.position.z);
                    predictedPos += playerVelOutZ * 0.1f;
                }

                smoothedPosition = Vector3.SmoothDamp(
                    interpolationStartPos,
                    predictedPos,
                    ref velocity,
                    SmoothTime
                );
                break;
        }

        cameraTransform.position = smoothedPosition;
    }

    #endregion

    #region Update

    private void UpdateCameraPosition(float deltaTime)
    {
        Vector3 ownerPosition = Owner.transform.position;
        targetPosition = ownerPosition + Offset;

        // 预测性跟随
        if (LookAhead)
        {
            var playerVelocity = (ownerPosition - lastTargetPosition) / deltaTime;
            var desiredLookAhead = playerVelocity.normalized * LookAheadDistance;

            lookAheadOffset = Vector3.SmoothDamp(
                lookAheadOffset,
                desiredLookAhead,
                ref lookAheadVelocity,
                LookAheadSmoothTime
            );

            targetPosition += lookAheadOffset;
        }

        // 死区处理
        Vector3 deltaMove = targetPosition - cameraTransform.position;
        if (Mathf.Abs(deltaMove.x) < DeadZone.x)
            targetPosition.x = cameraTransform.position.x;
        if (Mathf.Abs(deltaMove.y) < DeadZone.y)
            targetPosition.y = cameraTransform.position.y;

        // 边界限制
        if (UseBounds)
        {
            targetPosition = ClampToBounds(targetPosition);
        }

        // 根据跟随类型移动摄像机
        switch (FollowType)
        {
            case CameraFollowType.Smooth:
                cameraTransform.position = Vector3.SmoothDamp(
                    cameraTransform.position,
                    targetPosition,
                    ref velocity,
                    SmoothTime,
                    MaxSpeed
                );
                break;

            case CameraFollowType.Lerp:
                float lerpSpeed = 1f / SmoothTime;
                cameraTransform.position = Vector3.Lerp(
                    cameraTransform.position,
                    targetPosition,
                    lerpSpeed * deltaTime
                );
                break;

            case CameraFollowType.Damping:
                float dampingFactor = 1f - Mathf.Exp(-deltaTime / SmoothTime);
                cameraTransform.position = Vector3.Lerp(
                    cameraTransform.position,
                    targetPosition,
                    dampingFactor
                );
                break;
        }

        lastTargetPosition = ownerPosition;
    }

    #endregion

    #region 边界


    private Vector3 ClampToBounds(Vector3 position)
    {
        // 获取摄像机的视野大小
        float halfHeight = followCamera.orthographicSize;
        float halfWidth = halfHeight * followCamera.aspect;

        position.x = Mathf.Clamp(position.x,
            CameraBounds.min.x + halfWidth,
            CameraBounds.max.x - halfWidth);

        position.y = Mathf.Clamp(position.y,
            CameraBounds.min.y + halfHeight,
            CameraBounds.max.y - halfHeight);

        return position;
    }

    // 设置摄像机边界
    public void SetCameraBounds(Vector2 minBounds, Vector2 maxBounds)
    {
        CameraBounds = new Bounds();

        CameraBounds.SetMinMax(
            new Vector3(minBounds.x, minBounds.y, 0f)
            , new Vector3(maxBounds.x, maxBounds.y, 0f)
        );
        UseBounds = true;
    }

    #endregion

    #region 震屏

    // 震屏
    public void CameraShake(float duration, float magnitude)
    {
        if (Owner != null)
        {
            Owner.StartCoroutine(ShakeCoroutine(duration, magnitude));
        }
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalOffset = Offset;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            Offset = originalOffset + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Offset = originalOffset;
    }

    #endregion
}