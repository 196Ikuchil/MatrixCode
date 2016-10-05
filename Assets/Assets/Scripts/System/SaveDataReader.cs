using UnityEngine;
using System.Collections;

//プレハブで持ってもらって呼び出すときに使う感じ
public class SaveDataReader : MonoBehaviour {
    PlayerData playerData;

    public int saveDataNum = 2;    //ここにセーブデータの番号を格納　awake時に読み込んでくる  0,1がセーブデータ　2は全体のマトリックス管理

    //Player初期値（デフォルトで設定する）
    int dHP = 100;
    int dPOWER = 10;
    int dMPOWER = 10;
    int dDEF = 10;
    int dSP = 100;
    int dSPREC = 1;
    const int dCOMMAND = 2;

    //コマンド所持の最大数
    const int maxcommand = 10;
    //技の数
    const int skillNum = 200;
    //マトリックスの大きさ
    const int matrixSize = 1000;
    //総クエスト数
    public static int questNum = 30;
    //隠しクエスト数
    public static int hideQuestNum = 9;
    public int GetQuestNum()
    {
        return questNum;
    } 

    string first = "FIRST_VISIT";//ニューゲームかどうか
    public bool GetFirstVisit()
    {
        return PlayerPrefsX.GetBool(first + saveDataNum, true);
    }
    public void SetFirstVisit(bool b)
    {
        PlayerPrefsX.SetBool(first + saveDataNum, b);
    }

    //キーストリング
    string allMatrixIsOpenBool = "MATRIX_IS_OPEN";     //マトリックス解放されているかbool
    public bool[] GetAllMatrixIsOpenBool() {
       return GetAllMatrixIsOpenBool(saveDataNum);
    }
    public bool[] GetAllMatrixIsOpenBool(int i) //セーブ番号を指定
    {
        return PlayerPrefsX.GetBoolArray(allMatrixIsOpenBool +i);
    }
    public void SetAllMatrixIsOpenBool(bool[] b) 
    {
        SetAllMatrixIsOpenBool(b, saveDataNum);
    }
    public void SetAllMatrixIsOpenBool(bool[] b,int i)//配列ごとしまう、セーブ番号を指定
    {
        PlayerPrefsX.SetBoolArray(allMatrixIsOpenBool + i, b);
    }
    //指定した番号をオープンにする 最後に現在空いている数を設定
    public void SetMatrixIsOpenTrue(int b)
    {
        SetMatrixIsOpenTrue(b, saveDataNum);
    }
    public void SetMatrixIsOpenTrue(int b,int i)  
    {
        bool[] bNow = GetAllMatrixIsOpenBool(i);
        bNow[b] = true;
        PlayerPrefsX.SetBoolArray(allMatrixIsOpenBool + i, bNow);
        if (i!=2)
        {
            SetMatrixIsOpenTrue(b, 2);  //全体のマトリックスデータも解放
        }
        SetMatrixOpenNumBer(i);
    }
    public int[] GetMatrixOpenNumbers()
    {
        return GetMatrixOpenNumbers(saveDataNum);
    }
    public int[] GetMatrixOpenNumbers(int num) //空いているマトリックスの番号を返す
    {
        int[] nums=new int[GetMatrixOpenNum(num)];
        bool[] matrix = GetAllMatrixIsOpenBool(num);
        int j=0;
        for (int i = 0; i < matrix.Length; i++)
        {
            if (matrix[i] == true) {
                nums[j] = i;
                j++;
            }
        }
        return nums;
    }

    string questIsClear = "QUEST_IS_CLEAR";     //現在のクエストの達成状況
    public bool[] GetQuestIsClear()
    {
        return PlayerPrefsX.GetBoolArray(questIsClear + saveDataNum);
    }
    public void SetQuestIsClear(bool[] b)
    {
        PlayerPrefsX.SetBoolArray(questIsClear + saveDataNum, b);
    }
    public void SetQuestIsClearTrue(int b) //指定したクエスト番号のものをクリアにする
    {
        bool[] bNow = GetQuestIsClear();
        bNow[b] = true;
        PlayerPrefsX.SetBoolArray(questIsClear + saveDataNum, bNow);
    }
    public int GetQuestClearNum()//現在のクリアしているクエストの数　
    {
        int k=0;
        bool[] b=PlayerPrefsX.GetBoolArray(questIsClear + saveDataNum);
        for (int i = 0; i < b.Length; i ++) {
            if (b[i] == true) k++;
                }
        return k;
    }
    public int GetQuestClearNum(int sdNum)//現在のクリアしているクエストの数　データ指定
    {
        int k = 0;
        bool[] b = PlayerPrefsX.GetBoolArray(questIsClear + sdNum);
        for (int i = 0; i < b.Length; i++)
        {
            if (b[i] == true) k++;
        }
        return k;
    }

    //マトリックスがオープンされている数
    string matrixOpenNum = "MATRIX_OPEN_NUM";   
    public int GetMatrixOpenNum() 
    {
        return GetMatrixOpenNum(saveDataNum);
    }
    public int GetMatrixOpenNum(int i)
    {
        //SetMatrixOpenNumBer(i);
        return PlayerPrefs.GetInt(matrixOpenNum + i);
    }

    public void SetMatrixOpenNumBer()
    {
        SetMatrixOpenNumBer(saveDataNum);
    }
    public void SetMatrixOpenNumBer(int num) //現在空いているマトリックスの数を数えて入れてくれる
    {
        bool[] matrix = GetAllMatrixIsOpenBool(num);
        int j = 0;
        for (int i = 0; i < matrix.Length; i++)
        {
            if (matrix[i] == true)
            {
                j++;
            }
        }
        PlayerPrefs.SetInt(matrixOpenNum + num, j);
    }

    string questCharengeNum = "QUEST_CHARENGE_NUM"; //累計クエスト挑戦数
    public int GetQuestCharengeNum()
    {
        return PlayerPrefs.GetInt(questCharengeNum + saveDataNum);
    }
    public void SetQuestCharengeNum(int i)
    {
        PlayerPrefs.SetInt(questCharengeNum+ saveDataNum, i);
    }

    //プレイヤーステータス
    string HP = "PLAYER_HP";
    public int GetHP()
    {
        return PlayerPrefs.GetInt(HP + saveDataNum);
    }
    public void SetHP(int i)
    {
        PlayerPrefs.SetInt(HP + saveDataNum, this.GetHP()+i);
    }

    string POWER = "PLAYER_POWER";
    public int GetPOWER()
    {
        return PlayerPrefs.GetInt(POWER + saveDataNum);
    }
    public void SetPOWER(int i)
    {
        PlayerPrefs.SetInt(POWER + saveDataNum, this.GetPOWER()+i);
    }

    string MAJICPOWER = "PLAYER_MAJICPOWER";
    public int GetMAJICPOWER()
    {
        return PlayerPrefs.GetInt(MAJICPOWER + saveDataNum);
    }
    public void SetMAJICPOWER(int i)
    {
        PlayerPrefs.SetInt(MAJICPOWER + saveDataNum, this.GetMAJICPOWER()+ i);
    }

    string DEFENCE = "PLAYER_DEFENCE";
    public int GetDEFENCE()
    {
        return PlayerPrefs.GetInt(DEFENCE+ saveDataNum);
    }
    public void SetDEFENCE(int i)
    {
        PlayerPrefs.SetInt(DEFENCE + saveDataNum, this.GetDEFENCE()+ i);
    }

    string SP = "PLAYER_SP";
    public int GetSP()
    {
        return PlayerPrefs.GetInt(SP + saveDataNum);
    }
    public void SetSP(int i)
    {
        PlayerPrefs.SetInt(SP + saveDataNum, this.GetSP()+i);
    }

    string SPRECOVER = "PLAYER_SPRECOVER";
    public int GetSPRECOVER()
    {
        return PlayerPrefs.GetInt(SPRECOVER + saveDataNum);
    }
    public void SetSPRECOVER(int i)
    {
        PlayerPrefs.SetInt(SPRECOVER + saveDataNum,this.GetSPRECOVER()+ i);
    }


    string haveCommand = "HAVE_COMMAND";    //その技を取得しているかbool配列
    public bool[] GetHaveCommand()
    {
        return PlayerPrefsX.GetBoolArray(haveCommand + saveDataNum);
    }
    public int GetHavetotalCommandNum()    //現在持っているコマンドの総数
    {
        bool[] b = GetHaveCommand();
        int j = 0;
        for (int i = 0; i < b.Length; i++)
        {
            if (b[i] == true)
                j++;
        }
        return j;
    }
    public int[] GetHaveCommandwithNumber() //持ってるコマンド一覧をint[]で受けろる
    {
        bool[] b = GetHaveCommand();
        int maxNum = GetHavetotalCommandNum();
        int[] i = new int[maxNum];
        int j=0;
        for (int k = 0; k < b.Length; k++) {
            if (b[k] == true) {
                i[j] = k;
                j++;
            }
        }
        return i;
    }
    public void SetHaveCommand(bool[] i)
    {
        PlayerPrefsX.SetBoolArray(haveCommand + saveDataNum, i);
    }
    public void SetHaveCommandTrue(int i) //指定した番号のコマンドを所持していることにする
    {
        bool[] bNum = GetHaveCommand();
        bNum[i]=true;
        PlayerPrefsX.SetBoolArray(haveCommand + saveDataNum, bNum);
    }

    string commandNum = "COMMAND_NUM";      //現在のコマンド数（初期で2を入れておかないと）
    public int GetCommandNum()
    {
        return PlayerPrefs.GetInt(commandNum + saveDataNum);
    }
    public void SetCommandNum(int i) //初期化用
    {
        PlayerPrefs.SetInt(commandNum + saveDataNum, i);
    }
    public void SetCommandNumPlus(int i) //iの分だけ増やす
    {
        if (GetCommandNum() >= 10) return;
        PlayerPrefs.SetInt(commandNum + saveDataNum, GetCommandNum()+ i);
    }

    string commandList = "COMMAND_LIST";    //int[]　コマンド拡張最大数の配列にしてcommandNumにより読む大きさ制御 現在所持しているコマンドを保存
    public int[] GetCommandList()
    {
        
        return PlayerPrefsX.GetIntArray(commandList + saveDataNum);

    }
    public void SetCommandList(int[] i)
    {
        
        PlayerPrefsX.SetIntArray(commandList + saveDataNum, i);
    }
    public void SetCommandListWithNumber(int i,int k) //指定したコマンドi番に指定したコマンドkを保存
    {
        int[] list = GetCommandList();
        list[i] = k;
        SetCommandList(list);
    }

    string hideQuest = "HIDE_QUEST";
    public bool[] GetHideQuest()    //隠しクエスト
    {
        return PlayerPrefsX.GetBoolArray(hideQuest + saveDataNum);
    }
    public void SetHideQuest(bool[] b)
    {
        PlayerPrefsX.SetBoolArray(hideQuest + saveDataNum, b);
    }
    public void SetHideQuestClear(int i) //i番目をクリアにする
    {
        bool[] b = GetHideQuest();
        b[i] = true;
        SetHideQuest(b);
    }
    string hideQuestN = "HIDE_QUEST_NUM";
    public int GetHideQuestNum()
    {
        return PlayerPrefs.GetInt(hideQuestN + saveDataNum,0);
    }
    public void SetHideQuestNum(int i)   //隠しクエストをi個開放する
    {
        PlayerPrefs.SetInt(hideQuestN + saveDataNum, GetHideQuestNum() + i);
    }
    public void SetHideQuestPartTrue(int i)
    {
        bool[] bNum = this.GetHideQuest();
        bNum[i] = true;
        this.SetHideQuest(bNum);
    }

    //初期化メソッド
    void InitSaveData()
    {
        bool[] bol = new bool[matrixSize];
        for (int k = 0; k < matrixSize; k++) bol[k] = false;
        SetAllMatrixIsOpenBool(bol);

        bool[] bClear = new bool[questNum];
        for (int k = 0; k < questNum; k++) bClear[k] = false;
        SetQuestIsClear(bClear);
        SetMatrixOpenNumBer();
        SetQuestCharengeNum(0);
        SetHP(dHP);
        SetPOWER(dPOWER);
        SetMAJICPOWER(dMPOWER);
        SetDEFENCE(dDEF);
        SetSP(dSP);
        SetSPRECOVER(dSPREC);
        

        bool[] b = new bool[skillNum];
        for (int k = 0; k < skillNum; k++) b[k] = false;
        SetHaveCommand(b);
        SetHaveCommandTrue(1); //たたかうコマンド

        SetCommandNum(dCOMMAND);
        int[] i = new int[maxcommand] { 1, 0,0,0,0,0,0,0,0,0 }; //全てにたたかうをセット
        SetCommandList(i);

        bool[] b2 = new bool[hideQuestNum];
        for (int k = 0; k < hideQuestNum; k++) b2[k] = false;
        SetHideQuest(b2);
        SetHideQuestNum(0);
        //一度最初に読んだらようなし
        SetFirstVisit(false);
    }

    //クエストボタンが押された後、ステータスをまとめてデータに保存するメソッド 引数はクエスト番号
    public void SetPlayerStatusBeforeQuest(int i)
    {
        playerData.QUESTNUM = i;
        playerData.HP = GetHP();
        playerData.POWER = GetPOWER();
        playerData.MAJICPOWER = GetMAJICPOWER();
        playerData.DEFENCE = GetDEFENCE();
        playerData.SP = GetSP();
        playerData.SPRECOVER = GetSPRECOVER();

        //コマンド系
        playerData.CommandNum = GetCommandNum(); //コマんど数
        playerData.CommandList = GetCommandList();//コマンド一覧
}

    void Awake()
    {
        playerData = PlayerData.Instance;
        this.saveDataNum = playerData.saveDataNum;//セーブデータ番号を読み込んでくる
        Debug.Log("DataNum=" + saveDataNum);
            if (this.GetFirstVisit())
                this.InitSaveData();
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
