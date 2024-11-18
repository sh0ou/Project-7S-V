using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ParameterableHitBox : UdonSharpBehaviour
    {
        private void Start()
        {
            playerParameter = GetComponent<ParameterableCharacter>();
            myUserType = playerParameter.GetUserType();
            if (myUserType == UserType.Player)
            {
                capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
                capsuleCollider.isTrigger = true;
            }
            GetLocalPlayer();
        }

        private void LateUpdate()
        {
            // 位置を設定
            switch (myUserType)
            {
                case UserType.Player:
                    gameObject.transform.SetPositionAndRotation(lPlayer.GetPosition(), lPlayer.GetRotation());
                    break;
                default:
                    break;
            }
        }

        private void GetLocalPlayer() => lPlayer = Networking.LocalPlayer;

        private void OnTriggerEnter(Collider collider) => HitCollider(collider);

        private void HitCollider(Collider collider)
        {
            var paramObj = collider.GetComponent<ParameterableObject>();
            if (!Utilities.IsValid(paramObj)) return;

            var power = paramObj.GetPower();
            switch (paramObj.GetUserType())
            {
                case UserType.Player:
                    if (myUserType == UserType.Player) return;
                    // 敵に当たった場合
                    Debug.Log($"{DEBUG_HEAD} HitEnemy: <color=yellow>{name}</color> < {collider.name}");

                    playerParameter.SetParameter(PlayerParameterType.Hp, -power);
                    PopDamageEffect(collider, power);

                    if (playerParameter.GetParameter(PlayerParameterType.Hp) <= 0)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case UserType.Enemy:
                    if (myUserType == UserType.Enemy) return;
                    // プレイヤーに当たった場合
                    Debug.Log($"{DEBUG_HEAD} HitPlayer: <color=yellow>{name}</color> < {collider.name}");

                    playerParameter.SetParameter(PlayerParameterType.Hp, -power);
                    PopDamageEffect(collider, power);
                    break;
                default:
                    break;
            }
        }

        private void PopDamageEffect(Collider collider, int power)
        {
            var effect = Instantiate(damageEffect, collider.transform.position, Quaternion.identity);
            GetLocalPlayer();

            // Y軸のみ回転
            if (myUserType == UserType.Enemy)
            {
                var look = Quaternion.LookRotation(lPlayer.GetPosition() - effect.transform.position);
                effect.transform.rotation = Quaternion.Euler(0, look.eulerAngles.y + 180, 0);
            }

            effect.GetComponentInChildren<TextMeshProUGUI>().text = power.ToString();
            effect.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
            Destroy(effect, 1.0f);
        }

        public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
        {
            if (myUserType != UserType.Player || !Utilities.IsValid(player) || !player.isLocal) return;

            // プレイヤーの高さを取得
            var playerHeight = player.GetAvatarEyeHeightAsMeters();

            // プレイヤーのカプセルコライダーの高さを設定
            capsuleCollider.height = playerHeight;
            capsuleCollider.center = new Vector3(0, playerHeight / 2, 0);
        }
        [SerializeField] private GameObject damageEffect;

        private UserType myUserType;
        private VRCPlayerApi lPlayer;
        private CapsuleCollider capsuleCollider;
        private ParameterableCharacter playerParameter;
        private const string DEBUG_HEAD = "[<color=yellow>ParameterableHitBox</color>]";
    }
}