using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameRuleCtrl : MonoBehaviour {
    // 残り時間
    public float timeRemaining =5.0f * 60.0f;
	// ゲームオーバーフラグ
	public bool gameOver = false;
	// ゲームクリア
    public bool gameClear = false;
    public float sceneChangeTime = 3.0f;
    //パネルを開けるか消すかしたか
    public bool matrixIsOpened = false;

    int killAmount = 100;
    //オーディオ
    public AudioSource clearSeAudio;
    public AudioSource gOverSeAudio;
    public AudioSource finishSeSource;

    public AudioSource battleBGM;

    PlayerData pData;

    public GameObject canvas;
    public GameObject matrixPanelPref;
    GameObject matrixPanel;

    enum State {
        Battle,
        FinishRug,
        Result,
        End,
        Destroy
    }
    State state = State.Battle;


    void Start()
    {
        pData = PlayerData.Instance;

    }

	void Update()
	{
        if (state == State.Battle)
        {
            //敵を一定数倒したらゲームクリア
            if (killAmount <= 0)
            {
                StartCoroutine( GameClear());
            }

		    // 残り時間が無くなったらゲームオーバー
		    if(timeRemaining<= 0.0f ){
			    StartCoroutine( GameOver());
		    }
            //たぶんゲームの進行時間計測してる
            timeRemaining -= Time.deltaTime;
        }
        if (state == State.FinishRug)
        {
            sceneChangeTime -= Time.deltaTime;
            if (sceneChangeTime <= 0.0f)
            {
                matrixPanel = (GameObject)Instantiate(matrixPanelPref, transform.position, Quaternion.identity);
                matrixPanel.transform.SetParent(canvas.transform,false);
                state = State.Result;       
            }
        }
        if (state == State.Result)
        {
            Time.timeScale = 0f;
            if (matrixPanel == null)
            {
                state = State.End;
            }
        }
        if (state == State.End)
        {
            Time.timeScale = 1.0f;
            FadeManager.Instance.LoadLevel("SelectScene",2f);
            state = State.Destroy;
            //SceneManager.LoadScene("SelectScene");
        }
	}
	
	public IEnumerator GameOver()
	{
        battleBGM.Stop();
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
        gOverSeAudio.Play();
        gameOver = true;
        state = State.FinishRug;
	}
	public IEnumerator GameClear()
	{
        battleBGM.Stop();
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
        gameClear = true;
        clearSeAudio.Play();
        pData.QuestClear = true;
        state = State.FinishRug;
    }

    public int GetCurrentTime()//intで渡す　向こうで分：秒に直してもらう
    {
        return (int)timeRemaining;
    }

    public void SetTime(float t)
    {
        timeRemaining = t;
    }

    public void SetKillAmount(int k)
    {
        killAmount = k;
    }

    public void killEnemy()
    {
        killAmount -= 1;
    }
}
