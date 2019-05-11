using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClickedGameObject : MonoBehaviour {

    GameObject clickedGameObject;
    GameObject clickedGameObject2;
    private Vector3 playerPos;
    private Vector3 mousePos;
    private Vector3 firstplayerPos;
    RaycastHit2D hit2d;

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            clickedGameObject = hit2d.transform.gameObject;

            firstplayerPos = clickedGameObject.transform.position;

            if (clickedGameObject)
            { 
            playerPos = clickedGameObject.transform.position;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

        }


        if (Input.GetMouseButton(0))
        {


            if (hit2d)
            {

                Vector3 prePos = clickedGameObject.transform.position;
                Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePos;

                //タッチ対応デバイス向け、1本目の指にのみ反応
                if (Input.touchSupported)
                {
                    diff = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - mousePos;
                }

                diff.z = 0.0f;
                clickedGameObject.transform.position = playerPos + diff;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            clickedGameObject2 = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            clickedGameObject2 = hit2d.transform.gameObject;

            playerPos = Vector3.zero;
            mousePos = Vector3.zero;

            if (clickedGameObject2)
            {
                Debug.Log(clickedGameObject2);
                clickedGameObject.transform.position = clickedGameObject2.transform.position;
            }
            else {
                clickedGameObject.transform.position = firstplayerPos;
            }
        }



    }
}
