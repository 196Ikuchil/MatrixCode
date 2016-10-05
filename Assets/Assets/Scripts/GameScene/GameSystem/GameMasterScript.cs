using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour {

    static int QUESTDATANUMBER = 0; //１個目のシート

    public GameObject gameRule;
    GameRuleCtrl gamerule;
    public QuestDataSheet questDataSheet;
    QuestDataSheet.Param questData;
    public QuestDataSheet hideQuestDataSheet;
    QuestDataSheet.Param hideQuestData;
    public GameObject enemyGenePrefab;
    GameObject enemyGene;
    public GameObject pausePanel; //ポーズ画面

    //今回のクエストの仕様
    int questNubner = 0;
    string enemy = "Warg";      //敵の種類
    int killAmount = 0; //倒す量
    int enemyLV = 1;    //敵のレベル（技の量に関係）HP,POWER,DEFENCEもこれに比例
    int mob = 0;       //雑魚的がわくかどうか 
    float time = 100000f;   //クエストの時間
    string memo;            //その他コメントやメモ

    PlayerData playerData;
      
    // Use this for initialization
    void Awake()
    {
        //画面の解像度
        float screenRate = (float)1024*0.7f / Screen.width;
        if (screenRate > 1) screenRate = 1;
        int width = (int)(Screen.width * screenRate);
        int height = (int)(Screen.height * screenRate);
        Screen.SetResolution(width, height, true, 15);
        playerData = PlayerData.Instance;

        //クエストを読み込む
        if (playerData.Secret)
            importHideQuestData();
        else
            importQuestData();
       
        gamerule = gameRule.GetComponent<GameRuleCtrl>();
    }

    void Start () {
        
        //敵のジェネレータ生成
        enemyGene = (GameObject)Instantiate(enemyGenePrefab,new Vector3(0,10,-0),Quaternion.identity);

        //制限時間設定
        gamerule.SetTime(time);

        //倒す敵の量を設定
        gamerule.SetKillAmount(killAmount);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //クエストデータを読み込んで格納
    void importQuestData()
    {
        questData = questDataSheet.sheets[QUESTDATANUMBER].list[playerData.QUESTNUM];
        enemy = questData.enemy;
        killAmount = questData.killAmount;
        enemyLV = questData.enemyLV;
        mob = questData.mob;
        time = questData.time;
        memo = questData.memo;
    }
    void importHideQuestData()
    {
        hideQuestData = hideQuestDataSheet.sheets[QUESTDATANUMBER].list[playerData.QUESTNUM];
        enemy = hideQuestData.enemy;
        killAmount = hideQuestData.killAmount;
        enemyLV = hideQuestData.enemyLV;
        mob = hideQuestData.mob;
        time = hideQuestData.time;
        memo = hideQuestData.memo;
    }

    //敵が倒されたらこいつを読んでもらう　タグを送ってもらう
    public void killEnemy(string tag)
    {
        if (tag == enemy)
            gamerule.killEnemy();
    }
    //一時停止
    public void GamePause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    //再開
    public void GamePlay()
    {
        Time.timeScale = 1f;
    }
    //リタイア
    public void RetireQuest()
    {
        Time.timeScale = 1f;
        FadeManager.Instance.LoadLevel("SelectScene",2f);
        //SceneManager.LoadScene("SelectScene");
    }
    private void OnApplicationPause(bool pauseStatus) //TODO
    {

        //一時停止
        if (pauseStatus)
        {
            GamePause();
        }
        //再開時
        else
        {
            //GC
            System.GC.Collect();
            //使ってないアセットをアンロード
            Resources.UnloadUnusedAssets();
        }
    }

    public string GetTag()
    {
        return enemy;
    }

    public int GetLV()
    {
        return enemyLV;
    }

    public int GetKillAmount()
    {
        return killAmount;
    }

    public int GetEnemyLV()
    {
        return enemyLV;
    }
}
