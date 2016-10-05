using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerTextGUI : MonoBehaviour {

    public GameObject gameRuleObj;
    Text timerText;
    GameRuleCtrl gameRuleCtrl;
    int currentTime;

    void Awake()
    {
        gameRuleCtrl = (GameRuleCtrl)GameObject.FindObjectOfType(typeof(GameRuleCtrl));
    }
    // Use this for initialization
    void Start () {
        gameRuleCtrl = gameRuleObj.GetComponent<GameRuleCtrl>();
        timerText=this.GetComponent<Text>();
        setCurrentTimeText();

    }
	
	// Update is called once per frame
	void Update () {
        currentTime = gameRuleCtrl.GetCurrentTime();
        setCurrentTimeText();
    }

    void setCurrentTimeText()
    {
        int min = (int)(currentTime / 60);
        int sec = (int)(currentTime - min * 60);
        timerText.text = min + ":" + sec.ToString("D2");
    }
}
