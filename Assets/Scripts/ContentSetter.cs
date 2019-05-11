using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextColor(TextMeshProUGUI text, Color color, float alpha = 1.0f)
    {
        Color c = color;
        c.a = alpha;
        text.color = color;
    }

    public void SetColorOfButtonObject(Transform buttonObject, Color color, float alpha = 0.3f)
    {
        Image buttonImage = buttonObject.GetComponent<Image>();
        Color c = color;
        c.a = alpha;
        buttonImage.color = c;
    }

    public List<Unit> CastAlliesToUnits(List<Ally> allies)
    {
        List<Unit> units = new List<Unit>();
        foreach (Ally a in allies)
        {
            units.Add(a);
        }
        return units;
    }
}
