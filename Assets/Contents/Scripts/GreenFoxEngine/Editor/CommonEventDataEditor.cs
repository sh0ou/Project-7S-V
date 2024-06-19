using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UdonSharpEditor;
using UnityEditor.UIElements;
using System.Linq;

namespace sh0uRoom.GFE
{
    [CustomEditor(typeof(CommonEventData))]
    public class CommonEventDataEditor : Editor
    {
        [SerializeField] private VisualTreeAsset actionRootAsset;
        [SerializeField] private VisualTreeAsset actionItemAsset;
        [SerializeField] private VisualTreeAsset actionTalkAsset;
        [SerializeField] private VisualTreeAsset actionChooseAsset;
        [SerializeField] private VisualTreeAsset actionChooseItemAsset;
        [SerializeField] private VisualTreeAsset actionParamBranchAsset;
        [SerializeField] private VisualTreeAsset actionParamChangeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            //UdonSharpBehaviourのヘッダーを表示
            var imguiContainer = new IMGUIContainer(() =>
            {
                if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target))
                {
                    root.Clear();
                    root.Add(new IMGUIContainer(base.OnInspectorGUI));
                }
            });
            root.Add(imguiContainer);

            var actionRoot = actionRootAsset.CloneTree();
            root.Add(actionRoot);

            var actionList = actionRoot.Q<ListView>();
            actionList.makeItem = MakeActionItem;

            return root;
        }

        private VisualElement MakeActionItem()
        {
            VisualElement actionItem = actionItemAsset.CloneTree();
            var actionType = actionItem.Q<EnumField>().value;
            Debug.Log(actionType);
            switch (actionType)
            {
                case EventActionType.Talk:
                    var talk = actionTalkAsset.CloneTree();
                    // actionItem.Add(talk);
                    break;
                case EventActionType.Choose:
                    var choose = actionChooseAsset.CloneTree();
                    actionItem.Add(choose);
                    break;
                case EventActionType.ParameterBranch:
                    var paramBranch = actionParamBranchAsset.CloneTree();
                    actionItem.Add(paramBranch);
                    break;
                case EventActionType.ParameterChange:
                    var paramChange = actionParamChangeAsset.CloneTree();
                    actionItem.Add(paramChange);
                    break;
                default:
                    break;
            }

            return actionItem;
        }
    }
}