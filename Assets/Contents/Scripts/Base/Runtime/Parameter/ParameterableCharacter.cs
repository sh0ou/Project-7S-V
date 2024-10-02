using System;
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
                case PlayerParameterType.Atk:
                    atk = isOverwrite ? value : atk + value;
                    break;
                case PlayerParameterType.Def:
                    def = isOverwrite ? value : def + value;
                    break;
                case PlayerParameterType.Spd:
                    spd = isOverwrite ? value : spd + value;
                    break;
                case PlayerParameterType.Vit:
                    vit = isOverwrite ? value : vit + value;
                    break;
                case PlayerParameterType.Mnd:
                    mnd = isOverwrite ? value : mnd + value;
                    break;
                case PlayerParameterType.Dex:
                    dex = isOverwrite ? value : dex + value;
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
                case PlayerParameterType.Atk:
                    return atk;
                case PlayerParameterType.Def:
                    return def;
                case PlayerParameterType.Spd:
                    return spd;
                case PlayerParameterType.Vit:
                    return vit;
                case PlayerParameterType.Mnd:
                    return mnd;
                case PlayerParameterType.Dex:
                    return dex;
                default:
                    return 0;
            }
        }

        // 固定ステータス
        [SerializeField] private int maxHp;
        [SerializeField] private int maxMp;
        private int atk;
        private int def;
        private int spd;
        private int vit;
        private int mnd;
        private int dex;

        // 変動ステータス
        [SerializeField] private int hp;
        [SerializeField] private int mp;

        [SerializeField] private UserType userType;
        public UserType GetUserType() => userType;
        private const string DEBUG_HEAD = "[<color=yellow>ParameterableCharacter</color>]";
    }
    [System.Serializable]
    public enum PlayerParameterType
    {
        Hp,
        MaxHp,
        Mp,
        MaxMp,
        Atk,
        Def,
        Spd,
        Vit,
        Mnd,
        Dex,
    }
    [System.Serializable]
    public enum UserType
    {
        Player,
        Enemy
    }
}