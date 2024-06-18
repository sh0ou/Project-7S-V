using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UdonSharpEditor;
using System.Linq;
using UnityEditor.UIElements;

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

            //Listを紐づけ
            var actionList = actionRoot.Q<ListView>();
            // var actionItem = actionItemAsset.CloneTree();
            actionList.makeItem = () => actionItemAsset.CloneTree();

            var commonEventData = target as CommonEventData;
            // foreach (var action in commonEventData.actions)
            // {
            //     //List内に追加
            //     var actionItemView = actionItem.Children().First();
            //     actionItemView.Add(actionItem);
            // }

            return root;
        }
    }
}