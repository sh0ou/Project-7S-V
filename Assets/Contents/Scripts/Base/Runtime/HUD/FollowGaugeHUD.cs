
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    /// <summary>
    /// ゲージUIに追従するクラス
    /// </summary>
    public class FollowGaugeHUD : UdonSharpBehaviour
    {
        [SerializeField] private Image followGaugeImage;
        [SerializeField] private Image myGaugeImage;
        [SerializeField] private float leapSpeed = 1.0f;
        private float thresholdValue = 0.005f;

        public override void PostLateUpdate()
        {
            // 双方のfillAmountの差が閾値より大きい場合
            if (Mathf.Abs(followGaugeImage.fillAmount - myGaugeImage.fillAmount) > thresholdValue)
            {
                myGaugeImage.fillAmount = GetLeapGaugeValue();
            }
            else
            {
                myGaugeImage.fillAmount = followGaugeImage.fillAmount;
            }
        }

        /// <summary>
        /// 減衰後の値を取得する
        /// </summary>
        private float GetLeapGaugeValue() => Mathf.Lerp(myGaugeImage.fillAmount, followGaugeImage.fillAmount, Time.deltaTime * leapSpeed);
    }
}