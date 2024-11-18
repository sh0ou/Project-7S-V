using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Enemy : UdonSharpBehaviour
    {
        private void OnEnable()
        {
            // モデルを生成
            if (modelRoot.childCount == 0)
            {
                var obj = Instantiate(model, modelRoot);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
            }

            statusHUD.SetActive(true);
        }

        [SerializeField] private GameObject model;
        [SerializeField] private Transform modelRoot;
        [SerializeField] private GameObject statusHUD;
        private const string DEBUG_HEAD = "[<color=yellow>Enemy</color>]";
    }

    public enum EnemyState
    {
        Idle,   // 待機
        Scout,  // 索敵
        Chase,  // 追跡
        Engage  // 交戦
    }
}