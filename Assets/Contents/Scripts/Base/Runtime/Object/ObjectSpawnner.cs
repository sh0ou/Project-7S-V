using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ObjectSpawnner : UdonSharpBehaviour
    {
        private void Start()
        {
            if (Utilities.IsValid(targetObject))
            {
                // 対象のオブジェクトを表示
                targetObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (targetObject.activeSelf) return;

            spawnIntervalTimer += Time.deltaTime;
            if (spawnIntervalTimer >= spawnInterval)
            {
                spawnIntervalTimer = 0f;
                SpawnObject();
            }
        }

        public void SpawnObject()
        {
            if (Utilities.IsValid(targetObject))
            {
                // 対象のオブジェクトを表示する
                targetObject.SetActive(true);
                targetObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }
        }

        [SerializeField] private GameObject targetObject;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spawnInterval = 30f;
        [SerializeField] private float spawnIntervalTimer = 0f;
    }
}