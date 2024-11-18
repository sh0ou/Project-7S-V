using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ParameterableCharacter : UdonSharpBehaviour
    {
        private void OnEnable()
        {
            LoadJson();
        }

        private void LoadJson()
        {
            if (!Utilities.IsValid(parameterJson))
            {
                Debug.LogError($"{DEBUG_HEAD} ParameterableCharacter is not set.");
                enabled = false;

            }
            ResetParameter();
        }

        /// <summary>
        /// パラメータを一括ロードします
        /// </summary>
        /// <param name="parameter"></param>
        private void LoadParameters(CharacterParameter parameter)
        {
            Debug.Log($"{DEBUG_HEAD} LoadParameters: {parameter}");
            SetParameter(PlayerParameterType.Lv, parameter.Lv(), true);
            SetParameter(PlayerParameterType.MaxHp, parameter.MaxHp(), true);
            SetParameter(PlayerParameterType.Hp, parameter.MaxHp(), true);
            SetParameter(PlayerParameterType.MaxMp, parameter.MaxMp(), true);
            SetParameter(PlayerParameterType.Mp, parameter.MaxMp(), true);
            SetParameter(PlayerParameterType.MaxTp, parameter.MaxTp(), true);
            SetParameter(PlayerParameterType.Tp, parameter.MaxTp(), true);
            SetParameter(PlayerParameterType.Atk, parameter.Atk(), true);
            SetParameter(PlayerParameterType.Def, parameter.Def(), true);
            SetParameter(PlayerParameterType.Spd, parameter.Spd(), true);
            SetParameter(PlayerParameterType.Mat, parameter.Mat(), true);
            SetParameter(PlayerParameterType.Mdf, parameter.Mdf(), true);
            SetParameter(PlayerParameterType.Luk, parameter.Luk(), true);
            Debug.Log($"{DEBUG_HEAD} LoadParameters...OK");
        }

        private void ResetParameter()
        {
            parameter = CharacterParameter.New(parameterJson.text);
            if (Utilities.IsValid(parameter)) LoadParameters(parameter);
        }

        /// <summary>
        /// パラメータを設定します
        /// </summary>
        /// <param name="type">種類</param>
        /// <param name="value">設定値</param>
        /// <param name="isOverwrite">上書きするか</param>
        public void SetParameter(PlayerParameterType type, int value, bool isOverwrite = false)
        {
            // Debug.Log($"{DEBUG_HEAD} SetParameter: {type} / {value}");

            switch (type)
            {
                case PlayerParameterType.Hp:
                    hp = isOverwrite ? value : hp + value;
                    break;
                case PlayerParameterType.MaxHp:
                    parameter.MaxHp(isOverwrite ? value : parameter.MaxHp() + value);
                    break;
                case PlayerParameterType.Mp:
                    mp = isOverwrite ? value : mp + value;
                    break;
                case PlayerParameterType.MaxMp:
                    parameter.MaxMp(isOverwrite ? value : parameter.MaxMp() + value);
                    break;
                case PlayerParameterType.Tp:
                    tp = isOverwrite ? value : tp + value;
                    break;
                case PlayerParameterType.MaxTp:
                    parameter.MaxTp(isOverwrite ? value : parameter.MaxTp() + value);
                    break;
                case PlayerParameterType.Atk:
                    parameter.Atk(isOverwrite ? value : parameter.Atk() + value);
                    break;
                case PlayerParameterType.Def:
                    parameter.Def(isOverwrite ? value : parameter.Def() + value);
                    break;
                case PlayerParameterType.Mat:
                    parameter.Mat(isOverwrite ? value : parameter.Mat() + value);
                    break;
                case PlayerParameterType.Mdf:
                    parameter.Mdf(isOverwrite ? value : parameter.Mdf() + value);
                    break;
                case PlayerParameterType.Spd:
                    parameter.Spd(isOverwrite ? value : parameter.Spd() + value);
                    break;
                case PlayerParameterType.Luk:
                    parameter.Luk(isOverwrite ? value : parameter.Luk() + value);
                    break;
            }
        }

        /// <summary>
        /// パラメータを取得します
        /// </summary>
        /// <param name="type">種類</param>
        /// <returns>値</returns>
        public int GetParameter(PlayerParameterType type)
        {
            switch (type)
            {
                case PlayerParameterType.Hp:
                    return hp;
                case PlayerParameterType.MaxHp:
                    return parameter.MaxHp();
                case PlayerParameterType.Mp:
                    return mp;
                case PlayerParameterType.MaxMp:
                    return parameter.MaxMp();
                case PlayerParameterType.Tp:
                    return tp;
                case PlayerParameterType.MaxTp:
                    return parameter.MaxTp();
                case PlayerParameterType.Atk:
                    return parameter.Atk();
                case PlayerParameterType.Def:
                    return parameter.Def();
                case PlayerParameterType.Mat:
                    return parameter.Mat();
                case PlayerParameterType.Mdf:
                    return parameter.Mdf();
                case PlayerParameterType.Spd:
                    return parameter.Spd();
                case PlayerParameterType.Luk:
                    return parameter.Luk();
                default:
                    return 0;
            }
        }

        [SerializeField] private TextAsset parameterJson;
        [SerializeField] private UserType userType;
        public UserType GetUserType() => userType;

        // 固定ステータス
        private CharacterParameter parameter
        {
            get => (CharacterParameter)d_parameter;
            set => d_parameter = value;
        }
        private DataList d_parameter;

        // 変動ステータス
        [SerializeField] private int exp;
        [SerializeField] private int hp;
        [SerializeField] private int mp;
        [SerializeField] private int tp;

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