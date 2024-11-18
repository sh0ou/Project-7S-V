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