using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    public Transform optionWindow;

    private TextMeshProUGUI topText;
    private List<Transform> buttonObjects = new List<Transform>();

    private string selectedOption = "";

    public string SelectedOption { get => selectedOption; set => selectedOption = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        topText = optionWindow.Find("Panel/TopText").
    GetComponentInChildren<TextMeshProUGUI>();

        foreach (Transform t in optionWindow.Find("Panel/ButtonPanel"))
        {
            buttonObjects.Add(t);
            t.gameObject.SetActive(false);
        }
        Debug.Log(buttonObjects.Count);

        optionWindow.gameObject.SetActive(false);
    }

    public void SetPanel(Event ev)
    {
        optionWindow.gameObject.SetActive(true);
        topText.text = ev.TopText;

        foreach (Transform t in buttonObjects)
        {
            t.gameObject.SetActive(false);
            Button button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }

        //for (int i = 0; i < ev.Options.Count; i++)
        //{
        //    buttonObjects[i].gameObject.SetActive(true);
        //    Button button = buttonObjects[i].GetComponent<Button>();
        //    TextMeshProUGUI text = buttonObjects[i].GetComponentInChildren<TextMeshProUGUI>();
        //    text.text = ev.Options[i];
        //    string s = ev.Options[i];
        //    button.onClick.AddListener(() => onClickOptionButton(s));
        //}

        for (int i = 0; i < ev.OptionIDs.Count; i++)
        {
            buttonObjects[i].gameObject.SetActive(true);
            Button button = buttonObjects[i].GetComponent<Button>();
            TextMeshProUGUI text = buttonObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            text.text = ev.OptionIDs[i].text;
            //string s = ev.OptionIDs[i].text;
            string ID = ev.OptionIDs[i].ID;
            button.onClick.AddListener(() => { SetID(ev, ID); });
        }

        Debug.Log("");

    }

    public void SetID(Event e, string id)
    {
        Debug.Log(e);
        this.selectedOption = id;
    }

    public void ClosePanel()
    {
        optionWindow.gameObject.SetActive(false);
        topText.text = "";
        foreach (Transform t in buttonObjects)
        {
            t.gameObject.SetActive(false);
            Button button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
    }

    public void OnClickOptionButton(string s)
    {
        this.selectedOption = s;
    }

}
