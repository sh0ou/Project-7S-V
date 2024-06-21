using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UdonSharpEditor;
using UnityEditor.UIElements;

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

            // 追加するとserializedObjectがまだアップデートされてないので手動的にさせる
            serializedObject.UpdateIfRequiredOrScript();
            var prop = serializedObject.FindProperty("actions").GetArrayElementAtIndex(index);
            (element as BindableElement).BindProperty(prop);

            UpdateActionContainer(container, action, prop);

            //古いの
            enumField.UnregisterValueChangedCallback(ActionChangeCallback);
            //新しいの
            enumField.RegisterValueChangedCallback(ActionChangeCallback);

            void ActionChangeCallback(ChangeEvent<System.Enum> evt)
            {
                Debug.Log($"Change Type: {index} / {action.actionType}");
                UpdateActionContainer(container, action, prop);
            }
        }

        /// <summary>
        /// アクションコンテナを更新する
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        private void UpdateActionContainer(VisualElement container, CommonEventAction action, SerializedProperty actionProperty)
        {
            container.Clear();
            VisualTreeAsset asset = null;
            string propertyName = null;
            switch (action.actionType)
            {
                case EventActionType.Talk:
                    asset = actionTalkAsset;
                    propertyName = nameof(CommonEventAction.talkAction);
                    break;
                case EventActionType.Choose:
                    asset = actionChooseAsset;
                    propertyName = nameof(CommonEventAction.chooseAction);
                    break;
                case EventActionType.ParameterBranch:
                    asset = actionParamBranchAsset;
                    propertyName = nameof(CommonEventAction.paramBranchAction);
                    break;
                case EventActionType.ParameterChange:
                    asset = actionParamChangeAsset;
                    propertyName = nameof(CommonEventAction.paramChangeAction);
                    break;
                default:
                    break;
            }

            if (asset != null)
            {
                var clonedAsset = asset.CloneTree();
                container.Add(clonedAsset);
                clonedAsset.BindProperty(actionProperty.FindPropertyRelative(propertyName));
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