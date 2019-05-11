using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static EnumHolder;

public class EventManager
{
    private TextAsset csvFile; // CSVファイル
    private List<string[]> itemData = new List<string[]>(); // CSVの中身を入れるリスト
    private List<string[]> equipmentData = new List<string[]>();
    private int height = 0; // CSVの行数

    private string currentEvent = "";
    private string currentEventStatement = "";
    private string selectedCommand = "";

    private List<Event> events = new List<Event>();

    private void Start()
    {
        
    }

    public EventManager()
    {

    }

    public Event GetEventByID(string id)
    {
        Event newEvent = events.Find(e => e.Id == id);
        return newEvent;
    }

    public void SendMessage()
    {

    }


    public Event GetNextEvent(string nextID)
    {
        Event ev = new Event();
        if (string.IsNullOrEmpty(nextID))
        {
            return null;
        }
        else if (events.Exists(e => e.Id == nextID))
        {
            ev = events.Find(e => e.Id == nextID);
            if (ev != null)
            {
                return ev;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

}
