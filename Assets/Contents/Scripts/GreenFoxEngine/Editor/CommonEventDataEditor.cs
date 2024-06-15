using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UdonSharpEditor;
using System.Linq;

namespace sh0uRoom.GFE
{
    [CustomEditor(typeof(CommonEventData))]
    public class CommonEventDataEditor : Editor
    {
        // [SerializeField] private VisualTreeAsset uxmlAsset;
        // private VisualElement rootVisualElement;

        // private PopupField<EventActionType> actionTypeField;
        private VisualElement actionContainer;

        public override VisualElement CreateInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return base.CreateInspectorGUI();
            // if (uxmlAsset == null) return base.CreateInspectorGUI();

            // rootVisualElement = new VisualElement();
            // var uxmlRoot = uxmlAsset.CloneTree();
            // rootVisualElement.Add(uxmlRoot);

            // popupフィールドの初期化
            // actionTypeField = rootVisualElement.Q<PopupField<EventActionType>>("#ActionType");
            // actionTypeField.RegisterValueChangedCallback(evt => UpdateActionUI(evt.newValue));

            // actionContainerの初期化
            // actionContainer = rootVisualElement.Q<VisualElement>("ActionContainer");
            actionContainer = new VisualElement();

            // UpdateActionUI(actionTypeField.value);

            // return rootVisualElement;
            return base.CreateInspectorGUI();
        }

        // private void UpdateActionUI(EventActionType actionType)
        // {
        //     actionContainer.Clear();

        //     switch (actionType)
        //     {
        //         case EventActionType.Talk:
        //             var talkAction = new TextField("Talk Action");
        //             actionContainer.Add(talkAction);
        //             break;
        //         case EventActionType.Choose:
        //             var chooseAction = new TextField("Choose Action");
        //             actionContainer.Add(chooseAction);
        //             break;
        //         case EventActionType.ParameterChange:
        //             var paramChangeAction = new TextField("Parameter Change Action");
        //             actionContainer.Add(paramChangeAction);
        //             break;
        //         case EventActionType.ParameterBranch:
        //             var paramBranchAction = new TextField("Parameter Branch Action");
        //             actionContainer.Add(paramBranchAction);
        //             break;
        //         default:
        //             break;
        //     }
        // }
    }
}