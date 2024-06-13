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
        public string windowColor;
    }

    [System.Serializable]
    public class ChooseAction
    {
        public string[] text;
        public int defaultId;
        public string[] eventNames;
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