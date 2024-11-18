using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LocalPlayerTrackingTracker : UdonSharpBehaviour
{
    [Header("General Settings")]
    public VRCPlayerApi.TrackingDataType trackingPoint = VRCPlayerApi.TrackingDataType.Head;    // 追跡箇所
    public bool enablePosition = true;
    public bool enableRotation = true;

    // Updateで使う変数キャッシュ用
    protected VRCPlayerApi lPlayer;
    protected bool isVR;
    protected Transform selfTransform;

    private void OnEnable()
    {
        selfTransform = transform;
        if (Utilities.IsValid(Networking.LocalPlayer))
        {
            lPlayer = Networking.LocalPlayer;
            isVR = lPlayer.IsUserInVR();

            // 初期位置にリセット
            var selfPos = enablePosition ? lPlayer.GetTrackingData(trackingPoint).position : selfTransform.position;
            var selfRot = enableRotation ? lPlayer.GetTrackingData(trackingPoint).rotation : selfTransform.rotation;
            selfTransform.SetPositionAndRotation(selfPos, selfRot);
        }
    }

    public override void PostLateUpdate()
    {
        if (!enablePosition & !enableRotation) { return; }
        if (!Utilities.IsValid(lPlayer)) { return; }

        var selfPos = enablePosition ? GetTrackingPosition(trackingPoint) : selfTransform.position;
        var selfRot = enableRotation ? GetTrackingRotation(trackingPoint) : selfTransform.rotation;
        selfTransform.SetPositionAndRotation(selfPos, selfRot);
    }

    public void TrackingHead() => trackingPoint = VRCPlayerApi.TrackingDataType.Head;

    public void TrackingLeftHand() => trackingPoint = VRCPlayerApi.TrackingDataType.LeftHand;

    public void TrackingRightHand() => trackingPoint = VRCPlayerApi.TrackingDataType.RightHand;

    public void TrackingOrigin() => trackingPoint = VRCPlayerApi.TrackingDataType.Origin;

    public void TrackingAvatarRoot() => trackingPoint = VRCPlayerApi.TrackingDataType.AvatarRoot;

    protected virtual Vector3 GetTrackingPosition(VRCPlayerApi.TrackingDataType trackingTarget) => lPlayer.GetTrackingData(trackingTarget).position;

    protected virtual Quaternion GetTrackingRotation(VRCPlayerApi.TrackingDataType trackingTarget) => lPlayer.GetTrackingData(trackingTarget).rotation;
}