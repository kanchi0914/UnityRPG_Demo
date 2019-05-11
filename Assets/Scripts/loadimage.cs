using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 
    
    image : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Sprite sprite = Resources.Load<Sprite>("tako");
        // 空のゲームオブジェクトを生成
        GameObject gameObj = new GameObject();
        // Imageコンポーネントをアタッチ
        Image image = gameObj.AddComponent<Image>();
        // Resources.Load()で生成したSpriteを指定
        image.sprite = sprite;
        // Canvasの子オブジェクトとする
        gameObj.transform.parent = FindObjectOfType<Canvas>().transform;

        float width = sprite.rect.width;
        float height = sprite.rect.height;
        gameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        gameObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        gameObj.GetComponent<RectTransform>().localPosition = new Vector3(150, 0, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
