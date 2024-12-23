using UnityEngine;
using UnityEditor;
using UdonSharpEditor;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

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
            var container = element.Q<VisualElement>(CONTAINER);
            var action = ((CommonEventData)target).actions[index];
            serializedObject.UpdateIfRequiredOrScript();
            var prop = serializedObject.FindProperty("actions").GetArrayElementAtIndex(index);

            (element as BindableElement).BindProperty(prop);

            var containerElement = new ContainerElement
            {
                Container = container,
                Action = action,
                Property = prop
            };
            UpdateActionContainer(containerElement);

            // アクションタイプの変更を監視
            container.UnregisterCallback((ChangeEvent<SerializedProperty> evt) => ActionChangeCallback(evt, element, index));
            container.RegisterCallback((ChangeEvent<SerializedProperty> evt) => ActionChangeCallback(evt, element, index));

            var enumField = element.Q<EnumField>(ACTION_TYPE);
            enumField.UnregisterValueChangedCallback((ChangeEvent<Enum> evt) => ActionChangeCallback(evt, element, index));
            enumField.RegisterValueChangedCallback((ChangeEvent<Enum> evt) => ActionChangeCallback(evt, element, index));
        }

        private void ActionChangeCallback(ChangeEvent<Enum> evt, VisualElement element, int index)
        {
            if (evt.newValue == null) return;
            if (index < 0 || index >= ((CommonEventData)target).actions.Length) return;

            // Debug.Log("ActionChangeCallback - Enum");
            if (ConvertEventToContainer(element, index) is var container)
            {
                UpdateActionContainer(container);
            }
        }

        private void ActionChangeCallback(ChangeEvent<SerializedProperty> evt, VisualElement element, int index)
        {
            if (evt.newValue == null) return;
            if (index < 0 || index >= ((CommonEventData)target).actions.Length) return;

            // Debug.Log("ActionChangeCallback - SerializedProperty");
            if (ConvertEventToContainer(element, index) is ContainerElement containerElement)
            {
                UpdateActionContainer(containerElement);
            }
        }

        private ContainerElement ConvertEventToContainer(VisualElement element, int index)
        {
            var container = new ContainerElement
            {
                Container = element.Q<VisualElement>(CONTAINER),
                Action = ((CommonEventData)target).actions[index],
                Property = serializedObject.FindProperty("actions").GetArrayElementAtIndex(index)
            };
            return container;
        }

        /// <summary> 
        /// アクションコンテナを更新する
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        private void UpdateActionContainer(ContainerElement element)
        {
            element.Container.Clear();
            VisualTreeAsset asset = null;
            string propertyName = null;

            switch (element.Action.actionType)
            {
                case EventActionType.Talk:
                    UpdateTalkAction(element);
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
                element.Container.Add(clonedAsset);
                // Debug.Log($"Bind Property: {actionProperty} / {propertyName}");
                clonedAsset.BindProperty(element.Property.FindPropertyRelative(propertyName));
            }
        }

        private void UpdateTalkAction(ContainerElement element)
        {
            var asset = actionTalkAsset.CloneTree();
            element.Container.Add(asset);
            asset.BindProperty(element.Property.FindPropertyRelative(nameof(CommonEventAction.talkAction)));
        }

        private class ContainerElement
        {
            public VisualElement Container { get; set; }
            public CommonEventAction Action { get; set; }
            public SerializedProperty Property { get; set; }
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
        // --- Talk Action ---
        private const string TALK_NAME = "Name";
        private const string TALK_TEXT = "Text";
        private const string WINDOW_COLOR = "WindowColor";
    }
}