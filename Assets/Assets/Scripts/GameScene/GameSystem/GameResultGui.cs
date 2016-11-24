using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameResultGui : MonoBehaviour
{
	GameRuleCtrl gameRuleCtrl;

	public GameObject gameOverImage;
	public GameObject gameClearImage;
    bool endFinish=false;
    
	void Awake()
	{
		gameRuleCtrl = GameObject.FindObjectOfType(typeof(GameRuleCtrl)) as GameRuleCtrl;
        gameOverImage.SetActive(false);
        gameClearImage.SetActive(false);
	}

    void Update()
    {
        if (gameRuleCtrl.gameClear && !endFinish)
        {
            gameClearImage.SetActive(true);
            iTween.MoveFrom(gameClearImage,iTween.Hash("y",gameClearImage.transform.position.y+50f,"time",1f));
            endFinish = true;
        }
        else if (gameRuleCtrl.gameOver && !endFinish)
        {
            gameOverImage.SetActive(true);
            iTween.MoveFrom(gameOverImage , iTween.Hash("y", gameOverImage.transform.position.y + 50f, "time", 1f));
            endFinish = true;
        }

    }
}
