using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UdonSharpEditor;

namespace sh0uRoom.GFE
{
    [CustomEditor(typeof(CommonEventData))]
    public class CommonEventDataEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var imguiContainer = CreateUdonContainer(root);
            root.Add(imguiContainer);

            var actionRoot = actionRootAsset.CloneTree();
            root.Add(actionRoot);

            var actionList = actionRoot.Q<ListView>();
            actionList.makeItem = actionItemAsset.Instantiate;
            actionList.bindItem = BindActionItem;

            return root;
        }

        /// <summary>
        /// UdonSharpBehaviourのヘッダーを描画する
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private IMGUIContainer CreateUdonContainer(VisualElement root)
        {
            return new IMGUIContainer(() =>
            {
                if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target))
                {
                    root.Clear();
                    root.Add(new IMGUIContainer(base.OnInspectorGUI));
                }
            });
        }

        /// <summary>
        /// アクションリストの要素をバインドする
        /// </summary>
        /// <param name="element"></param>
        /// <param name="index"></param>
        private void BindActionItem(VisualElement element, int index)
        {
            var action = ((CommonEventData)target).actions[index];
            var enumField = element.Q<EnumField>(ACTION_TYPE);
            var container = element.Q<VisualElement>(CONTAINER);

            enumField.value = action.actionType;
            UpdateActionContainer(container, action.actionType);

            enumField.RegisterValueChangedCallback(evt =>
            {
                action.actionType = (EventActionType)evt.newValue;
                EditorUtility.SetDirty(target);
                Debug.Log($"Change Type: {index} / {action.actionType}");

                UpdateActionContainer(container, action.actionType);
            });
        }

        /// <summary>
        /// アクションコンテナを更新する
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        private void UpdateActionContainer(VisualElement container, EventActionType type)
        {
            container.Clear();
            VisualTreeAsset asset = null;
            switch (type)
            {
                case EventActionType.Talk:
                    asset = actionTalkAsset;
                    break;
                case EventActionType.Choose:
                    asset = actionChooseAsset;
                    break;
                case EventActionType.ParameterBranch:
                    asset = actionParamBranchAsset;
                    break;
                case EventActionType.ParameterChange:
                    asset = actionParamChangeAsset;
                    break;
                default:
                    break;
            }

            if (asset != null)
            {
                var clonedAsset = asset.CloneTree();
                container.Add(clonedAsset);
            }
        }

        [SerializeField] private VisualTreeAsset actionRootAsset;
        [SerializeField] private VisualTreeAsset actionItemAsset;
        [SerializeField] private VisualTreeAsset actionTalkAsset;
        [SerializeField] private VisualTreeAsset actionChooseAsset;
        [SerializeField] private VisualTreeAsset actionChooseItemAsset;
        [SerializeField] private VisualTreeAsset actionParamBranchAsset;
        [SerializeField] private VisualTreeAsset actionParamChangeAsset;

        private const string ACTION_TYPE = "ActionType";
        private const string CONTAINER = "Container";
    }
}