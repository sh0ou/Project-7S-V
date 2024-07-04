using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace sh0uRoom.GFE
{
    public class CommonEventData : UdonSharpBehaviour
    {
        public string eventName;
        // public List<CommonEventAction> actionList;
        public CommonEventAction[] actions;
    }

    [System.Serializable]
    public class CommonEventAction
    {
        public EventActionType actionType = EventActionType.Talk;
        public TalkAction talkAction;
        public ChooseAction chooseAction;
        public ParameterChangeAction paramChangeAction;
        public ParameterBranchAction paramBranchAction;

        public CommonEventAction()
        {
            talkAction = new TalkAction();
            chooseAction = new ChooseAction();
            paramChangeAction = new ParameterChangeAction();
            paramBranchAction = new ParameterBranchAction();
        }
    }

    public enum EventActionType
    {
        Talk,
        Choose,
        ParameterChange,
        ParameterBranch,
        CallEvent
    };

    [System.Serializable]
    public class TalkAction
    {
        public string name;
        public string text;
        // public string windowColor;
        public Color windowColor;

        public string GetWindowColor()
        {
            var color = windowColor;
            return $"{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
        }

        public void SetWindowColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString($"#{hex}", out var color))
            {
                windowColor = color;
            }
            else
            {
                Debug.LogWarning("Invalid color hex string");
            }
        }
    }

    [System.Serializable]
    public class ChooseAction
    {
        public string[] text;
        public string[] eventNames;
        public uint defaultId;
        public const uint arraySize = 8;

        public ChooseAction()
        {
            text = new string[arraySize];
            eventNames = new string[arraySize];
        }
    }

    [System.Serializable]
    public class ParameterChangeAction
    {
        public string inputId;
        public string outputId;
    }

    [System.Serializable]
    public class ParameterBranchAction
    {
        public string refId;
        public string ifValue;
        public IfSign ifSign;
        public string trueEventName;
        public string falseEventName;
    }

    public enum IfSign
    {
        Equal = 0,
        NotEqual = 1,
        Greater = 2,
        Less = 3,
        GreaterEqual = 4,
        LessEqual = 5
    }
}