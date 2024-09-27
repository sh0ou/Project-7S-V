﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class VRFollowHUD : LocalPlayerTrackingTracker
{
    [SerializeField]
    private bool vROnly = true; // VRモードでのみ有効

    [Header("Position Settings")]
    public bool syncPosition = true;  // 位置同期
    [Range(0.0f, 1.0f)]
    public float followMoveSpeed = 0.1f;   // 追跡速度
    public float distanceRange = 1.0f; // 最大離散距離
    public float moveThreshold = 0.0f;  // 追跡開始閾値
    private float pauseThreshold; // 追跡終了閾値、追跡開始閾値の5%を使う

    [Header("Rotation Settings")]
    public bool syncRotation = false;   // 回転同期
    [Range(0.0f, 1.0f)]
    public float followRotateSpeed = 0.02f;    // 追跡回転速度
    [Range(0.0f, 180.0f)]
    public float angleRange = 60.0f;  // 回転速度の加速閾値
    [Range(0.0f, 180.0f)]
    public float RotateThreshold = 0.0f; // 追跡回転開始閾値
    [Range(0.0f, 180.0f)]
    private float restThreshold;  // 追跡回転終了閾値、追跡回転開始閾値の5%を使う
    public bool lockRoll = true, lockPitch = false, lockYaw = false;   // 遅延せず回転同期(軸毎)

    // 計算用
    private float distance, angle;
    private bool isPause, isRest;

    protected override Vector3 GetTrackingPosition(VRCPlayerApi.TrackingDataType trackingTarget)
    {
        if (!vROnly || isVR)
        {
            if (!syncPosition)
            {
                return GetFollowPosition(lPlayer.GetTrackingData(trackingPoint).position);
            }
        }

        return base.GetTrackingPosition(trackingTarget);
    }

    protected override Quaternion GetTrackingRotation(VRCPlayerApi.TrackingDataType trackingTarget)
    {
        if (!vROnly || isVR)
        {
            if (!syncRotation)
            {
                return GetFollowRotation(lPlayer.GetTrackingData(trackingPoint).rotation);
            }
        }

        return base.GetTrackingRotation(trackingTarget);
    }

    // 位置の遅延追従
    private Vector3 GetFollowPosition(Vector3 targetPosition)
    {
        var selfPos = selfTransform.position;

        // moveThresholdの5%を閾値に使う
        pauseThreshold = moveThreshold * 0.05f;

        // 相対距離を算出
        distance = (selfPos - targetPosition).sqrMagnitude;

        // 閾値判定
        if (distance >= moveThreshold * moveThreshold)
        {
            isPause = false;
        }
        else if (distance < pauseThreshold * pauseThreshold)
        {
            isPause = true;
        }

        if (distance > distanceRange * distanceRange)
        {
            // 離散距離の制限
            selfPos = Vector3.MoveTowards(targetPosition, selfPos, Mathf.Abs(distanceRange));
        }
        else if (!isPause)
        {
            // 位置を遅延追従
            selfPos = Vector3.Lerp(selfPos, targetPosition, followMoveSpeed);
        }

        return selfPos;
    }

    // 回転の遅延追従
    private Quaternion GetFollowRotation(Quaternion targetRotation)
    {
        var selfRot = selfTransform.rotation;

        angleRange = Mathf.Clamp(angleRange, 0.0f, 180.0f);
        RotateThreshold = Mathf.Clamp(RotateThreshold, 0.0f, 180.0f);
        restThreshold = RotateThreshold * 0.05f; // angleThresholdの5%を閾値に使う

        // 相対角度を算出
        angle = Quaternion.Angle(selfRot, targetRotation);

        // 閾値判定
        if (angle >= RotateThreshold)
        {
            isRest = false;
        }
        else if (angle < restThreshold)
        {
            isRest = true;
        }

        // 軸毎に遅延しない追従を追加処理
        if (lockRoll) { selfRot = Quaternion.LookRotation(selfRot * Vector3.forward, targetRotation * Vector3.up); }
        if (lockPitch) { selfRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(selfRot * Vector3.forward, targetRotation * Vector3.up), selfRot * Vector3.up); }
        if (lockYaw) { selfRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(selfRot * Vector3.forward, targetRotation * Vector3.right), selfRot * Vector3.up); }

        if (!isRest)
        {
            // 回転を遅延追従
            selfRot = angle > angleRange ? Quaternion.Lerp(selfRot, targetRotation, followRotateSpeed * 4) : Quaternion.Lerp(selfRot, targetRotation, followRotateSpeed);
        }

        return selfRot;
    }
}