using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MessageManager : MonoBehaviour {

    private GameObject messagePanel;
    private TextMeshProUGUI messageText;
    private GameController gameController;
    
    private Transform nextDelta;

    private string stackedText = "";
    float time = 0.0f;
    float timeCount = 0;
    bool isStarted = false;
    bool isWaiting = false;

    bool isTexting = false;

    bool isAutoClose = true;

    // Use this for initialization
    void Start () {
        //this.messageText.text = "aaaaaaaaaa";
    }
	
	// Update is called once per frame
	void Update () {
        
        if (string.IsNullOrEmpty(gameController.CallBackManager.WaitingTarget))
        {
            nextDelta.gameObject.SetActive(false);
        }
        else
        {
            nextDelta.gameObject.SetActive(true);
        }

        if (gameController.CurrentEvent == null && !gameController.IsBattle)
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }

            if (timeCount <= 0)
            {
                SetActive(false);
                messageText.text = "";
            }
        }

        //戦闘中などはオフに
        //if (IsAutoClose)
        //{
        //    if (timeCount > 0)
        //    {
        //        timeCount -= Time.deltaTime;
        //    }

        //    if (timeCount <= 0)
        //    {
        //        SetActive(false);
        //    }
        //}
        
	}

    public void Init(GameController gameController, GameObject messagePanel)
    {
        this.gameController = gameController;
        this.messagePanel = messagePanel;
        messageText = messagePanel.transform.Find("Text").
            GetComponent<TextMeshProUGUI>();
        nextDelta = messagePanel.transform.Find("delta");
        IsAutoClose = false;
    }

    public void SetActive(bool b)
    {
        if (b)
        {
            this.messagePanel.SetActive(true);
        }
        else
        {
            this.messagePanel.SetActive(false);
        }
    }

    public void Reset()
    {
        stackedText = "";
    }

    public void SetAutoText(List<string> messageList)
    {

        Debug.Log("tesss");
        SetActive(true);
        
        if (!isWaiting)
        {
            isWaiting = true;
            StartCoroutine(Delay2(messageList));
        }
        //StartCoroutine(Delay(messageList));
        
        //SetActive(false);
    }

    private int count = 0;

    public bool IsAutoClose { get => isAutoClose; set => isAutoClose = value; }

    IEnumerator Sample()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("yeah");
    }

    IEnumerator Delay2(List<string> messageList)
    {
        foreach (string t in messageList)
        {
            yield return new WaitForSeconds(0.5f);
            AddText(t);
        }
        stackedText = "";

        isWaiting = false;
        yield return new WaitForSeconds(2.0f);
        SetActive(false);
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(2.0f);
        isTexting = false;
    }

    public void SetText(string text)
    {
        this.messageText.text = text;
        timeCount = 2.0f;
    }

    public void addnum()
    {
        count++;
    }

    public void AddText(string text)
    {
        int i = stackedText.Count(c => c == '\n');
        if (i > 4) stackedText = "";
        stackedText += text;
        stackedText += "\n";
        this.messageText.text = stackedText;
    }

}
