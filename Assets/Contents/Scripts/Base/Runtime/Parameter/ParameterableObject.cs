using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.PJ7S
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ParameterableObject : UdonSharpBehaviour
    {
        [SerializeField] private int power;
        public int GetPower() => power;
        [SerializeField] private int durability;
        public int GetDurability() => durability;
        [SerializeField] private UserType userType;
        public UserType GetUserType() => userType;
        [SerializeField] private ObjectType weaponType;
    }

    public enum ObjectType
    {
        Sword,
        Gun,
        Magic,
        Item,
        Obstacles
    }
}