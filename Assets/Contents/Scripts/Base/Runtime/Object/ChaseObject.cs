using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ChaseObject : UdonSharpBehaviour
    {
        private void OnEnable()
        {
            if (!Utilities.IsValid(targetObject))
            {
                targetObject = GameObject.Find("PlayerCollision");
            }
        }

        private void Update()
        {
            if (!Utilities.IsValid(targetObject)) return;

            UpdateDistance();
            Chase();
        }

        private void UpdateDistance()
        {
            var targetPos = targetObject.transform.position;
            distance = Vector3.Distance(targetPos, transform.position);

            isChase = distance < maxChaseDistance;
            isFocus = distance < maxFocusDistance;

            if (isChase)
            {
                targetCircle.material.SetColor("_Color", new Color32(255, 100, 100, 255));
            }
            else
            {
                targetCircle.material.SetColor("_Color", Color.yellow);
            }
        }

        /// <summary>
        /// 指定したオブジェクトを追尾する
        /// </summary>
        private void Chase()
        {
            if (!isChase) return;
            if (isFocus) return;

            var targetPos = targetObject.transform.position;
            var direction = targetPos - transform.position;
            var move = direction.normalized * chaseSpeed * Time.deltaTime;

            transform.position += move;

            // 少しずつ向きを変える
            var look = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 0.05f);
        }

        [field: SerializeField] public bool isChase { get; private set; }
        [field: SerializeField] public bool isFocus { get; private set; }

        [SerializeField] private GameObject targetObject;
        [SerializeField] private float maxChaseDistance, maxFocusDistance;
        [SerializeField] private float chaseSpeed = 1f;
        [SerializeField] private float distance;

        [SerializeField] private Renderer targetCircle;
    }
}