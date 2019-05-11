using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <---- 追加1
using TMPro;
using System.Linq;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    public Transform panel;

    Queue queue = new Queue();

    List<string> list = new List<string>();

    public TextMeshProUGUI[] textList = new TextMeshProUGUI[5];
    public TextMeshProUGUI[] temp = new TextMeshProUGUI[5];

    int count = 0;

    private string[] messageList = { "新しい朝が来た", "希望の浅田" , "あーああ", "新しい朝が来た", "希望の浅田", "あーああ",
    "新しい朝が来た", "希望の浅田" , "あーああ", "新しい朝が来た", "希望の浅田", "あーああ"};
    //List list = new List(messageList);
    private string[] tempMessageList = new string[4];

    //private Text[] messageList = new Text[4];
    //private Text[] tempMessage = new Text[4];

    // Use this for initialization
    void Start () {

        for (int i = 0; i < 5; i++)
        {
            textList[i].text = "";
            textList[i].transform.SetParent(panel, false);
            //GameObject listtext = Instantiate(textList[i].gameObject) as GameObject;
            //listtext.transform.SetParent(panel, false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendMessage()
    {
        if (count < textList.Count())
        {
            Debug.Log(textList[count]);
            //textList[count].text = count.ToString();

            if (count < messageList.Length)
            {
                textList[count].text = messageList[count];
            }
            else
            {
                textList[count].text = messageList[count - messageList.Length];
            }

        }
        else
        {

            for (int i = 0; i < textList.Length - 1; i++)
            {
                textList[i].text = textList[i + 1].text;
            }

            //string[] temp = new string[textList.Length];
            //Array.Copy(textList, temp, textList.Length);
            //for (int i = 0; i < textList.Length - 1; i++)
            //{
            //    textList[i].text = temp[i+1];
            //}

            textList[textList.Length - 1].text = count.ToString();
        }

        count++;
    }

    public void OnClick()
    {

        //引数にテキストをとる
        SendMessage();

        if (false)
        {
            if (count < messageList.Count())
            {

                string connected = "";
                //list.Add(count.ToString());
                queue.Enqueue(messageList[count]);

                if (queue.Count >= 6)
                {
                    queue.Dequeue();
                }

                foreach (string s in queue)
                {
                    connected += s;
                    connected += "\r\n";
                }

                //proText.text = connected;

                Debug.Log(connected);

            }

            count++;
        }
    }

}
