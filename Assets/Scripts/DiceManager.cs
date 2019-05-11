using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class DiceManager : MonoBehaviour
{
    public GameObject dice;
    Rigidbody2D rb2D;
    private Image diceImage;
    private int preNum;
    float posY;

    Sprite[] images;

    private bool isRandom = false;

    void Start()
    {
        diceImage = dice.GetComponent<Image>();
        rb2D = dice.GetComponent<Rigidbody2D>();
        images = Resources.LoadAll<Sprite>("DiceImage");
    }

    void Update()
    {
        if (isRandom)
        {
            if (!rb2D.IsSleeping())
            {
                int num = Random.Range(0, 6);
                diceImage.sprite = images[num];
                preNum = num;
            }
            else
            {
                Debug.Log(preNum + 1);
                isRandom = false;
            }
        }
    }

    public void OnClickDice()
    {
        if (rb2D.IsSleeping() && !isRandom)
        {
            rb2D.AddForce(new Vector2(0f, 1000f));
            isRandom = true;
        }
    }

}
