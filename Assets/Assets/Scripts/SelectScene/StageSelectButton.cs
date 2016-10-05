using UnityEngine;
using System.Collections;

public class StageSelectButton : MonoBehaviour {

    //panelから受け取る。
    public int questNumber;
    public GameObject parentPanel;
    StageSelectPanel stageSelectPanel;
    SelectMaster master;

	// Use this for initialization
	void Start () {
        stageSelectPanel= parentPanel.GetComponent<StageSelectPanel>();
        master = FindObjectOfType<SelectMaster>();
        this.questNumber =stageSelectPanel.questNumber;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnClick()
    {
        master.QuestStartDefault(questNumber);
        FindObjectOfType<GUIMoveManager>().fadeOut();//なんか困ったら消してよい
    }
}
