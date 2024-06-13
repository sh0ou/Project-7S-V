
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;

namespace sh0uRoom.GFE
{
    public class JsonToCEV : UdonSharpBehaviour
    {
        [SerializeField] private TextAsset jsonFile;
        [SerializeField] private CommonEventData targetCEV;
        void Start()
        {
            Convert2CEV(jsonFile.text);
        }

        public void Convert2CEV(string json)
        {
            Debug.Log(json);
            if (VRCJson.TryDeserializeFromJson(json, out DataToken deserializedObject))
            {
                if (deserializedObject.DataDictionary.GetKeys().Count == 0)
                {
                    Debug.LogError("Failed to deserialize json");
                    return;
                }
                else
                {
                    //talkキーの内容をCEVに反映
                    foreach (var keyArray in deserializedObject.DataDictionary.GetKeys().ToArray())
                    {
                        if (keyArray == "action")
                        {
                            var actions = deserializedObject.DataDictionary[keyArray].DataList;
                            Debug.Log($"actions: {actions.Count}");
                            foreach (var action in actions)
                            {
                                var actionDict = action.DataDictionary;
                                var commonEventAction = new CommonEventAction();
                                if (actionDict.ContainsKey("talk"))
                                {
                                    commonEventAction.actionType = EventActionType.Talk;
                                    commonEventAction.talkAction = ExtractTalkAction(actionDict["talk"].DataDictionary);
                                }
                                else if (actionDict.ContainsKey("choose"))
                                {
                                    commonEventAction.actionType = EventActionType.Choose;
                                    commonEventAction.chooseAction = ExtractChooseAction(actionDict["choose"].DataDictionary);
                                }
                                else if (actionDict.ContainsKey("paramChange"))
                                {
                                    commonEventAction.actionType = EventActionType.ParameterChange;
                                    commonEventAction.paramChangeAction = ExtractParameterChangeAction(actionDict["paramChange"].DataDictionary);
                                }
                                else if (actionDict.ContainsKey("paramBranch"))
                                {
                                    commonEventAction.actionType = EventActionType.ParameterBranch;
                                    commonEventAction.paramBranchAction = ExtractParameterBranchAction(actionDict["paramBranch"].DataDictionary);
                                }
                                AddActionToArray(ref targetCEV.actions, commonEventAction);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to deserialize json");
            }
        }

        private void AddActionToArray(ref CommonEventAction[] array, CommonEventAction newAction)
        {
            var newArray = new CommonEventAction[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            newArray[array.Length] = newAction;
            array = newArray;
        }

        private TalkAction ExtractTalkAction(DataDictionary talkDict)
        {
            TalkAction talkAction = new TalkAction();
            talkAction.name = talkDict["name"].String;
            talkAction.text = talkDict["text"].String;
            talkAction.windowColor = talkDict["windowColor"].String;
            return talkAction;
        }

        private ChooseAction ExtractChooseAction(DataDictionary chooseDict)
        {
            var textList = chooseDict["text"].DataList;
            var eventList = chooseDict["event"].DataList;

            var textArray = new string[textList.Count];
            for (int i = 0; i < textList.Count; i++)
            {
                textArray[i] = textList[i].String;
            }

            var eventArray = new string[eventList.Count];
            for (int i = 0; i < eventList.Count; i++)
            {
                eventArray[i] = eventList[i].String;
            }

            var chooseAction = new ChooseAction();
            chooseAction.text = textArray;
            chooseAction.defaultId = chooseDict["defaultId"].Int;
            chooseAction.eventNames = eventArray;
            return chooseAction;
        }

        private ParameterChangeAction ExtractParameterChangeAction(DataDictionary paramChangeDict)
        {
            var paramChangeAction = new ParameterChangeAction();
            paramChangeAction.inputId = paramChangeDict["inputId"].String;
            paramChangeAction.outputId = paramChangeDict["outputId"].String;
            return paramChangeAction;
        }

        private ParameterBranchAction ExtractParameterBranchAction(DataDictionary paramBranchDict)
        {
            var paramBranchAction = new ParameterBranchAction();
            paramBranchAction.refId = paramBranchDict["refId"].String;
            paramBranchAction.ifValue = paramBranchDict["ifValue"].String;
            paramBranchAction.ifSign = (IfSign)System.Enum.Parse(typeof(IfSign), paramBranchDict["ifSign"].String);
            paramBranchAction.trueEventName = paramBranchDict["trueEvent"].String;
            paramBranchAction.falseEventName = paramBranchDict["falseEvent"].String;
            return paramBranchAction;
        }
    }
}