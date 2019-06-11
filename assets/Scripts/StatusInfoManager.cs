using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusInfoManager : MonoBehaviour
{

    GameController gameController;

    TextMeshProUGUI Text;

    GameObject GoldText;
    GameObject LightText;
    GameObject KarmaText;

    // Start is called before the first frame update
    void Start()
    {
        Text = GameObject.Find("StatusInfoText").GetComponent<TextMeshProUGUI>();
    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = $"{gameController.Gold} G \n" +
            $"{gameController.FuelNum} / 100\n" +
            $"{gameController.KarmaNum} / 100";
    }
}
