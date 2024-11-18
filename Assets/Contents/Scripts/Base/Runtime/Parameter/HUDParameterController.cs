using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class HUDParameterController : UdonSharpBehaviour
    {
        private void Update()
        {
            if (!Utilities.IsValid(playerParameter)) return;

            foreach (var image in hpGauge)
            {
                image.fillAmount = (float)playerParameter.GetParameter(PlayerParameterType.Hp) / playerParameter.GetParameter(PlayerParameterType.MaxHp);
            }
            foreach (var text in hpText)
            {
                text.text = $"{playerParameter.GetParameter(PlayerParameterType.Hp)} / {playerParameter.GetParameter(PlayerParameterType.MaxHp)}";
            }
            foreach (var image in mpGauge)
            {
                image.fillAmount = (float)playerParameter.GetParameter(PlayerParameterType.Mp) / playerParameter.GetParameter(PlayerParameterType.MaxMp);
            }
        }

        [SerializeField] private ParameterableCharacter playerParameter;
        [SerializeField] private Image[] hpGauge = new Image[2];
        [SerializeField] private Image[] mpGauge = new Image[2];
        [SerializeField] private TMP_Text[] hpText = new TMP_Text[2];
    }
}