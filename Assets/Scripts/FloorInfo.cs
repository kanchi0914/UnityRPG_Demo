using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FloorInfo : MonoBehaviour
{
    public GameObject FloorPanelObject;
    //public GameObject gameControllerObject;

    GameController gameController;
    TextMeshProUGUI areaName;
    TextMeshProUGUI floorNum;
    Image backgroundImage;

    // Start is called before the first frame update
    void Awake()
    {
        //gameObject.SetActive(true);
        //areaName = transform.Find("AreaName").GetComponent<TextMeshProUGUI>();
        //floorNum = transform.Find("FloorNum").GetComponent<TextMeshProUGUI>();
        //backgroundImage = GetComponent<Image>();
        ////Debug.Log(backgroundImage);
        //Color c = Color.black;
        //c.a = 0f;
        //backgroundImage.color = c;
        //gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        FloorPanelObject.SetActive(true);
        areaName = FloorPanelObject.transform.Find("AreaName").GetComponent<TextMeshProUGUI>();
        floorNum = FloorPanelObject.transform.Find("FloorNum").GetComponent<TextMeshProUGUI>();
        backgroundImage = FloorPanelObject.GetComponent<Image>();
        //Debug.Log(backgroundImage);
        Color c = Color.black;
        c.a = 0f;
        backgroundImage.color = c;
        //gameController = gameControllerObject.GetComponent<GameController>();
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1.0f);
        backgroundImage.DOFade(0.0f, 1.0f).OnComplete(() => { FloorPanelObject.SetActive(false); });
        areaName.DOFade(0.0f, 1.0f);
        floorNum.DOFade(0.0f, 1.0f);
        backgroundImage.raycastTarget = false;

    }

    public void Reset()
    {

        Color c = Color.black;
        c.a = 1.0f;
        Debug.Log(backgroundImage);
        backgroundImage.color = c;
        backgroundImage.raycastTarget = true;

        FloorPanelObject.SetActive(true);

        Color c2 = Color.white;
        c2.a = 1.0f;

        areaName.color = c2;
        floorNum.color = c2;

        areaName.text = gameController.AreaName;
        floorNum.text = gameController.Floor.ToString() + "F";

        StartCoroutine(enumerator());
    }

}
