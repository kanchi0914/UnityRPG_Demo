using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingDelta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(-50f, 0.8f).SetEase(Ease.OutCubic).SetRelative());
        seq.Append(transform.DOLocalMoveY(50f, 0.1f).SetRelative().OnComplete(Move));
        //seq.Append(transform.DOLocalMoveY(100f, 5f).OnComplete(Move));
    }

}
