using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMaster : MonoBehaviour {

    int maxQuestNum=30;

    public GameObject canvas;
    public GameObject contents;
    //メニューのプレハブ
    public GameObject menuPref;
    public GameObject OpenMatrixBoardpref;
    //セーブデータリーダー
    public GameObject sdReaderPref;
    SaveDataReader sdReader;
    public GameObject selectPanelPrefab;
    public GameObject secretSelectPrefab;
    GameObject[] selectPanels;
    GameObject[] secretSelectPanels;
    //メニューボタン
    public Button menuB;
    public Button titleB;

    //クエストデータ
    public QuestDataSheet questDataPref;
    public QuestDataSheet secretQuestDataPref;
    QuestDataSheet.Sheet questData;
    QuestDataSheet.Sheet secretQuestData;

    public AudioSource decideSeSource;

    PlayerData playerData;
     void Awake()
    {

        playerData = PlayerData.Instance;
        sdReaderPref = (GameObject)Instantiate(sdReaderPref);
        sdReader = sdReaderPref.GetComponent<SaveDataReader>();
        
        //クエストクリア後だったら
        QuestClearMethod();
        //クエストデータ読み込み
        questData = questDataPref.sheets[0];
        secretQuestData = secretQuestDataPref.sheets[0];

       
        selectPanels = new GameObject[maxQuestNum+1];//未クエスト分
        secretSelectPanels = new GameObject[sdReader.GetHideQuestNum()];
    }

	// Use this for initialization
	void Start () {
        //保存先から読み込むクエスト進行状況
        maxQuestNum = sdReader.GetQuestClearNum(); //クリアしている数を読み取る
        bool[] b = sdReader.GetQuestIsClear();
        for (int i = 0; i < maxQuestNum; i++)
        {
            selectPanels[i] = (GameObject)Instantiate(selectPanelPrefab);
            selectPanels[i].GetComponent<StageSelectPanel>().questNumber = i;
            selectPanels[i].transform.SetParent(contents.transform,false);
            selectPanels[i].GetComponent<StageSelectPanel>().SetPanel(questData.list[i].name,questData.list[i].enemy,questData.list[i].killAmount.ToString(),questData.list[i].memo, b[i]);
        }
        if (!(sdReader.GetQuestClearNum() == sdReader.GetQuestNum()))
        {//最後のクエストをクリアしていなかったら 次のクエストを表示
            selectPanels[maxQuestNum] = (GameObject)Instantiate(selectPanelPrefab);
            selectPanels[maxQuestNum].GetComponent<StageSelectPanel>().questNumber = maxQuestNum;
            selectPanels[maxQuestNum].transform.SetParent(contents.transform, false);
            selectPanels[maxQuestNum].GetComponent<StageSelectPanel>().SetPanel(questData.list[maxQuestNum].name,questData.list[maxQuestNum].enemy, questData.list[maxQuestNum].killAmount.ToString(), questData.list[maxQuestNum].memo, false);
        }
        //隠し

        int maxSecretQuestNum = sdReader.GetHideQuestNum();
        Debug.Log(maxSecretQuestNum);
        if (maxSecretQuestNum > 0)
        {
            bool[] b2 = sdReader.GetHideQuest();
            for (int i = 0; i < maxSecretQuestNum; i++)
            {
                Debug.Log(secretSelectPrefab);
                secretSelectPanels[i] = (GameObject)Instantiate(secretSelectPrefab);
                secretSelectPanels[i].GetComponent<StageSelectPanel>().questNumber = i;
                secretSelectPanels[i].transform.SetParent(contents.transform, false);
                secretSelectPanels[i].GetComponent<StageSelectPanel>().SetPanel(secretQuestData.list[i].name, secretQuestData.list[i].enemy, secretQuestData.list[i].killAmount.ToString(), secretQuestData.list[i].memo, b2[i]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //すべてのstartボタンが押せなくなるメソッド
    public void SetAllSelectButtonState(bool b)
    {
        for(int i = 0; i < maxQuestNum; i++)
        {
            selectPanels[i].transform.FindChild("StartButton").gameObject.GetComponent<Button>().enabled=b;
        }
        menuB.enabled = b;
        titleB.enabled = b;
    }

    public void QuestStartDefault(int i)
    {
        //他のボタンを押せないようにする
        SetAllSelectButtonState(false);
        //プレイヤーのステータスおよびクエスト番号を格納
        sdReader.SetPlayerStatusBeforeQuest(i);
        DecideSePlay();
        //SceneManagerでゲームスタート
        FadeManager.Instance.LoadLevel("GameScene",2f);
        //GC
        System.GC.Collect();
        //使ってないアセットをアンロード
        Resources.UnloadUnusedAssets();
    }

    public void QuestStartSecret(int i)
    {
        //他のボタンを押せないようにする
        SetAllSelectButtonState(false);
        //プレイヤーのステータスおよびクエスト番号を格納
        sdReader.SetPlayerStatusBeforeQuest(i);
        playerData.Secret = true;
        DecideSePlay();
        //SceneManagerでゲームスタート
        FadeManager.Instance.LoadLevel("SecretGameScene", 2f);
        //GC
        System.GC.Collect();
        //使ってないアセットをアンロード
        Resources.UnloadUnusedAssets();
    }

    //ボタンから呼ばれる　メニューを開く
    public void OpenMenu()
    {
        DecideSePlay();
        GameObject menu = (GameObject)Instantiate(menuPref);
        menu.transform.SetParent(canvas.transform,false);    
    }

    public void OpenMatrixBoard()
    {
        GameObject b = (GameObject)Instantiate(OpenMatrixBoardpref);
        b.transform.SetParent(canvas.transform, false);
    }

    void QuestClearMethod()
    {

        if (playerData.Secret)
        {
            playerData.Secret = false;
            if (!playerData.QuestClear) return; //何もせずに終了
            sdReader.SetHideQuestClear(playerData.QUESTNUM);
        }
        else
        {
            if (!playerData.QuestClear) return; //何もせずに終了
            playerData.Secret = false;
            sdReader.SetQuestIsClearTrue(playerData.QUESTNUM); //挑戦したクエスト番号をクリアにセット
        }
        playerData.QuestClear = false; //再びfalseにセット
    }

    public void BackToTitle()
    {
        playerData.saveDataNum = 2; //どのセーブデータでもない
        SetAllSelectButtonState(false);
        DecideSePlay();
        FadeManager.Instance.LoadLevel("TitleScene", 1.5f);
    }

    public void DecideSePlay()
    {
        decideSeSource.Play();
    }
}
