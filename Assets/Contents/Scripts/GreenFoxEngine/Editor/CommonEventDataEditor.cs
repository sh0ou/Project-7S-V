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
        [SerializeField] private VisualTreeAsset rootAsset;
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

            rootAsset.CloneTree(root);

            //アクションリストの要素数を出力
            var actionListView = root.Q<ListView>();
            Debug.Log($"actionListView: {actionListView.name}");
            var commonEventData = target as CommonEventData;
            if (actionListView != null)
            {
                //各アクションの要素を生成
                foreach (var action in commonEventData.actions)
                {
                    var actionType = action.actionType;

                    switch (actionType)
                    {
                        case EventActionType.Talk:
                            break;
                        case EventActionType.Choose:
                            break;
                        case EventActionType.ParameterBranch:
                            break;
                        case EventActionType.ParameterChange:
                            break;
                        default:
                            break;
                    }
                }
            }


            // var talkView = itemView.Q<ListView>();
            // talkView.makeItem = actionTalkAsset.CloneTree;

            // var chooseView = itemView.Q<ListView>();
            // chooseView.makeItem = actionChooseAsset.CloneTree;
            // var chooseItemView = chooseView.Q<ListView>();
            // chooseItemView.makeItem = actionChooseItemAsset.CloneTree;

            // var paramBranchView = itemView.Q<ListView>();
            // paramBranchView.makeItem = actionParamBranchAsset.CloneTree;

            // var paramChangeView = itemView.Q<ListView>();
            // paramChangeView.makeItem = actionParamChangeAsset.CloneTree;

            return root;
        }
    }
}