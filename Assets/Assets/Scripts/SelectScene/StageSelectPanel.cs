using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StageSelectPanel : MonoBehaviour {

    public int questNumber { get; set; }
    //このクエストをクリアしているかどうか
    public GameObject questIsClear;

    public Text questName;
    public Text enemyName;
    public Text enemyNum;
    public Text message;

    public void SetPanel(string qName,string eName,string eNum,string message,bool b) {
        questName.text = qName;
        enemyName.text = eName;
        enemyNum.text = eNum+"体";
        this.message.text = message;
        questIsClear.SetActive(b);
    }
	// Use this for initialization
	void Start () {   
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
