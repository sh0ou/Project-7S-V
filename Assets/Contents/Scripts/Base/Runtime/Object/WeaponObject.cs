
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class WeaponObject : ParameterableObject
    {
        [SerializeField] private Collider hitBoxCollider;
        [SerializeField] private Collider hitAreaCollider;

        public override void OnPickup()
        {
            hitBoxCollider.enabled = false;
        }
        public override void OnDrop()
        {
            hitBoxCollider.enabled = true;
        }
    }
}