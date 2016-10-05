using UnityEngine;
using System.Collections;

public class PlayerData
{

    private static PlayerData mInstance;

    private PlayerData()
    { // Private Constructor

        Debug.Log("Create SampleSingleton instance.");
    }

    public static PlayerData Instance
    {
        get
        {
            if (mInstance == null) mInstance = new PlayerData();
            return mInstance;
        }
    }

    public int HP = 1;
    public int POWER = 1;
    public int MAJICPOWER=1;
    public int DEFENCE=1;
    public int SP = 1;
    public int SPRECOVER = 1;

    //コマンド数
    public int CommandNum = 2;
    //セット技
    public int[] CommandList;

    //今回選んだクエスト
    public int QUESTNUM=0;

    //クエストをクリアしたかどうか
    public bool QuestClear = false;

    //シークレットかどうか
    public bool Secret = false;

    //セーブデータの番号
    public int saveDataNum=2;




}
