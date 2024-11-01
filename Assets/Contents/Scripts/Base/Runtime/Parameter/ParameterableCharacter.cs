using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ParameterableCharacter : UdonSharpBehaviour
    {
        public void SetParameter(PlayerParameterType type, int value, bool isOverwrite = false)
        {
            Debug.Log($"{DEBUG_HEAD} SetParameter: {GetParameter(type)} / {value}");
            switch (type)
            {
                case PlayerParameterType.Hp:
                    hp = isOverwrite ? value : hp + value;
                    break;
                case PlayerParameterType.MaxHp:
                    maxHp = isOverwrite ? value : maxHp + value;
                    break;
                case PlayerParameterType.Mp:
                    mp = isOverwrite ? value : mp + value;
                    break;
                case PlayerParameterType.MaxMp:
                    maxMp = isOverwrite ? value : maxMp + value;
                    break;
                case PlayerParameterType.Tp:
                    tp = isOverwrite ? value : tp + value;
                    break;
                case PlayerParameterType.MaxTp:
                    maxTp = isOverwrite ? value : maxTp + value;
                    break;
                case PlayerParameterType.Atk:
                    atk = isOverwrite ? value : atk + value;
                    break;
                case PlayerParameterType.Def:
                    def = isOverwrite ? value : def + value;
                    break;
                case PlayerParameterType.Spd:
                    spd = isOverwrite ? value : spd + value;
                    break;
                case PlayerParameterType.Mat:
                    mat = isOverwrite ? value : mat + value;
                    break;
                case PlayerParameterType.Mdf:
                    mdf = isOverwrite ? value : mdf + value;
                    break;
                case PlayerParameterType.Luk:
                    luk = isOverwrite ? value : luk + value;
                    break;
            }
        }

        public int GetParameter(PlayerParameterType type)
        {
            switch (type)
            {
                case PlayerParameterType.Hp:
                    return hp;
                case PlayerParameterType.MaxHp:
                    return maxHp;
                case PlayerParameterType.Mp:
                    return mp;
                case PlayerParameterType.MaxMp:
                    return maxMp;
                case PlayerParameterType.Tp:
                    return tp;
                case PlayerParameterType.MaxTp:
                    return maxTp;
                case PlayerParameterType.Atk:
                    return atk;
                case PlayerParameterType.Def:
                    return def;
                case PlayerParameterType.Mat:
                    return mat;
                case PlayerParameterType.Mdf:
                    return mdf;
                case PlayerParameterType.Spd:
                    return spd;
                case PlayerParameterType.Luk:
                    return luk;
                default:
                    return 0;
            }
        }

        // 固定ステータス
        [SerializeField] private int lv;
        [SerializeField] private int maxHp;
        [SerializeField] private int maxMp;
        [SerializeField] private int maxTp;
        private int atk;
        private int def;
        private int spd;
        private int mat;
        private int mdf;
        private int luk;

        // 変動ステータス
        [SerializeField] private int exp;
        [SerializeField] private int hp;
        [SerializeField] private int mp;
        [SerializeField] private int tp;

        [SerializeField] private UserType userType;
        public UserType GetUserType() => userType;
        private const string DEBUG_HEAD = "[<color=yellow>ParameterableCharacter</color>]";
    }
    [System.Serializable]
    public enum PlayerParameterType
    {
        Lv,     // レベル
        Exp,    // 経験値
        Hp,     // 現在の HP
        MaxHp,  // 最大 HP
        Mp,     // 現在の MP
        MaxMp,  // 最大 MP
        Tp,     // 現在の TP
        MaxTp,  // 最大 TP
        Atk,    // 攻撃力
        Def,    // 防御力
        Mat,    // 魔法攻撃力
        Mdf,    // 魔法防御力
        Spd,    // 素早さ
        Luk     // 運
    }
    [System.Serializable]
    public enum UserType
    {
        Player,
        Enemy
    }
}