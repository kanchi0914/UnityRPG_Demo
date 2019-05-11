using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EnumHolder;

public class Event
{
    public delegate void CallBack();
    public CallBack OnSelected;
    private CallBack onEnd;

    private string eventType = "";

    private bool isGotItem = false;

    public struct GotItemInfo
    {
        public int Num { get; set; }
        public (int min, int max) Rarity { get; set; }
    }

    private GotItemInfo gotItemInfo;

    private string id = "";
    private string topText = "";
    EventScriptType eventScriptType = EventScriptType.text;
    private string nextID = "";
    //List<EventScript> eventScripts = new List<EventScript>();

    private List<string> texts = new List<string>();

    public Event(string id = "")
    {
        this.Id = id;
    }

    public string Id { get => id; set => id = value; }
    //public List<EventScript> EventScripts { get => eventScripts; private set => eventScripts = value; }
    public string NextID { get => nextID; set => nextID = value; }
    public List<string> Texts { get => texts; set => texts = value; }
    public EventScriptType EventScriptType { get => eventScriptType; set => eventScriptType = value; }
    public string TopText { get => topText; set => topText = value; }
    public List<string> Options { get => options; set => options = value; }
    public CallBack OnEnd { get => onEnd; set => onEnd = value; }
    public string EventType { get => eventType; set => eventType = value; }
    public bool IsGotItem { get => isGotItem; set => isGotItem = value; }
    public GotItemInfo ItemInfo { get => gotItemInfo; set => gotItemInfo = value; }
    public List<(string text, string ID)> OptionIDs { get => optionIDs; set => optionIDs = value; }
    public List<(string text, CallBack onSelected)> Optionlists { get => optionlists; set => optionlists = value; }

    //public List<string> OptionIDs { get => optionIDs; set => optionIDs = value; }

    private List<string> options = new List<string>();
    private List<(string text, string ID)> optionIDs = new List<(string, string)>();
    private List<(string text, CallBack onSelected)> optionlists = new List<(string, CallBack)>();

    public bool IsNull()
    {
        if (id == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetEvent(string script, EventScriptType type)
    {

    }

    public void SetTexts()
    {

    }

    public void AddText(string script)
    {
        this.eventScriptType = EventScriptType.text;
        this.texts.Add(script);
    }

    public void SetOptions(string script)
    {
        string[] splitted = script.Split('/');
        foreach (string s in splitted)
        {
            Options.Add(s);
        }
        this.eventScriptType = EventScriptType.option;
    }

    public void SetTopText(string text)
    {
        TopText = text;
        this.eventScriptType = EventScriptType.option;
    }


    public void SetNextID(string id)
    {
        this.NextID = id;
    }

    public void GetEffect()
    {

    }

}

