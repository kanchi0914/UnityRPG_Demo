using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveEnemyData : MonoBehaviour {

	// Use this for initialization
	void Start () {
        save();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void save()
    {
        EnemyClass Data = new EnemyClass
        {
            
            id = "slime",
            name = "slime",
            hp = 11,
            attack = 4
        };
        string jsonString = JsonUtility.ToJson(Data);

        string path = Application.dataPath + "/data/enemySaveData.json";

        using (var file = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
        {
            // 文字列をbyte配列に変換
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

            // ファイルに保存
            file.Write(bytes, 0, bytes.Length);
        }
    }

}
