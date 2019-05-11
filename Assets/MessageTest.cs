using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG;
using DG.Tweening;

public class MessageTest : MonoBehaviour
{
    TextMeshProUGUI text;
    Transform delta;
    float posY;
    Vector3 pos;
    Sequence seq;

    Transform textObj;

    // Start is called before the first frame update
    void Start()
    {
        posY = transform.position.y;
        transform.DOMoveY(posY, 3f);
        pos = transform.position;
        delta = transform.Find("delta");
        textObj = transform.Find("Panel/one");
        delta.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set()
    {
        delta.gameObject.SetActive(false);
        seq = DOTween.Sequence();
        text = transform.Find("Text").GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";
        string s = "時間を固定にしてしまうと、" +
            "文字数が少ないときと多いときで表示されるスピード感が全然ちがってしまうので、" +
            "文字数 × 1文字表示にかかる時間 という感じでやってます。";

        seq.Append(text.DOText(s, s.Length * 0.05f).SetEase(Ease.Linear)
            .OnComplete(() => { delta.gameObject.SetActive(true); }));
;
    }

    public void stop()
    {
        seq.Complete();
        //textObj.DOMoveY(300f, 10f).SetRelative();
    }

    public void Move()
    {
        Debug.Log(pos);
        transform.DOMoveY(posY, 0f);
        CanvasGroup cg = transform.GetComponent<CanvasGroup>();
        cg.DOFade(1f, 0f);

        Sequence seq2 = DOTween.Sequence();
        seq2.Append(transform.DOLocalMoveY(600f, 1f).SetRelative());

        seq2.Join(cg.DOFade(0f, 1f));
    }

}
