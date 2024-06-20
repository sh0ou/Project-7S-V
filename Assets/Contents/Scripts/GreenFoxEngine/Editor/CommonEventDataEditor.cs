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
            actionList.makeItem = actionItemAsset.Instantiate;
            actionList.bindItem = (element, i) =>
            {
                var action = ((CommonEventData)target).actions[i];
                var enumField = element.Q<EnumField>();
                enumField.value = action.actionType;

                // Remove old children
                var container = element.Q("Container");
                container.Clear();

                switch (action.actionType)
                {
                    case EventActionType.Talk:
                        var talk = actionTalkAsset.CloneTree();
                        container.Add(talk);
                        break;
                    case EventActionType.Choose:
                        var choose = actionChooseAsset.CloneTree();
                        container.Add(choose);
                        break;
                    case EventActionType.ParameterBranch:
                        var paramBranch = actionParamBranchAsset.CloneTree();
                        container.Add(paramBranch);
                        break;
                    case EventActionType.ParameterChange:
                        var paramChange = actionParamChangeAsset.CloneTree();
                        container.Add(paramChange);
                        break;
                    default:
                        break;
                }

                enumField.RegisterValueChangedCallback((evt) =>
                {
                    action.actionType = (EventActionType)evt.newValue;
                    EditorUtility.SetDirty(target);
                    Debug.Log($"Change Type: {i} / {action.actionType}");

                    container.Clear();
                    switch (action.actionType)
                    {
                        case EventActionType.Talk:
                            var talk = actionTalkAsset.CloneTree();
                            container.Add(talk);
                            break;
                        case EventActionType.Choose:
                            var choose = actionChooseAsset.CloneTree();
                            container.Add(choose);
                            break;
                        case EventActionType.ParameterBranch:
                            var paramBranch = actionParamBranchAsset.CloneTree();
                            container.Add(paramBranch);
                            break;
                        case EventActionType.ParameterChange:
                            var paramChange = actionParamChangeAsset.CloneTree();
                            container.Add(paramChange);
                            break;
                        default:
                            break;
                    }
                });
            };

            return root;
        }
    }
}