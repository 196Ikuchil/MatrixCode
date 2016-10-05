using UnityEngine;
using System.Collections;

public class MatrixCodeReader : MonoBehaviour {
    MatrixCode matrix;
    SaveDataReader sdReader;

    AbilityReader aReader;
    void Awake()
    {
        matrix = Resources.Load("Data/Code") as MatrixCode;
        GameObject sdReaderObj = (GameObject)Instantiate(Resources.Load("Prefabs/SaveDataReader"));
        sdReader = sdReaderObj.GetComponent<SaveDataReader>();
        aReader = GetComponent<AbilityReader>();
    }
	// Use this for initialization
	void Start () {
	    Debug.Log(matrix.sheets[0].list[2].col0);   //こんなんで呼べるよ
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int GetNumInMatrixCode(int i)
    {
        int code=0;
        int front = (int)(i / 10);    //前二つの数字をもらう。
        int rear = (int)i % 10; //一の位
        switch (rear) {
            case 0:
                code=(int)matrix.sheets[0].list[front].col0;
                break;
            case 1:
                code = (int)matrix.sheets[0].list[front].col1;
                break;
            case 2:
                code = (int)matrix.sheets[0].list[front].col2;
                break;
            case 3:
                code = (int)matrix.sheets[0].list[front].col3;
                break;
            case 4:
                code = (int)matrix.sheets[0].list[front].col4;
                break;
            case 5:
                code = (int)matrix.sheets[0].list[front].col5;
                break;
            case 6:
                code = (int)matrix.sheets[0].list[front].col6;
                break;
            case 7:
                code = (int)matrix.sheets[0].list[front].col7;
                break;
            case 8:
                code = (int)matrix.sheets[0].list[front].col8;
                break;
            case 9:
                code = (int)matrix.sheets[0].list[front].col9;
                break;
            default:
                code = -1;
                break;
        }

        return code;
    }

    public void DebugFullAbilityOpen()
    {
        for(int i = 7000; i < 7200; i++)
        {
            sdReader.SetHaveCommandTrue(i % 1000);
        }
    }
    public void DebugSetOpenNum()
    {
        sdReader.SetMatrixOpenNumBer();
    }
    public string GetMatrixBenefit(int i) //コードからステータス獲得し、その後のためにstringを返す。
    {
        
        string ReString="Error";
        int code=GetNumInMatrixCode(i);//コードを値に変換
        sdReader.SetMatrixIsOpenTrue(i);//マトリックスをオープンに

        int front = (int)(code / 1000);
        int rear = code % 1000;

        if (1 <= front && front <= 6) //ステータスアップ系
        {

            switch (front)
            {
                case 1:
                    sdReader.SetHP(rear);
                    ReString = "HP+" + rear.ToString();
                    break;
                case 2:
                    sdReader.SetPOWER(rear);
                    ReString = "力+" + rear.ToString();
                    break;
                case 3:
                    sdReader.SetMAJICPOWER(rear);
                    ReString = "魔力+" + rear.ToString();
                    break;
                case 4:
                    sdReader.SetSPRECOVER(rear);
                    ReString = "魔素回復力+" + rear.ToString();
                    break;
                case 5:
                    sdReader.SetSP(rear);
                    ReString = "魔素+" + rear.ToString();
                    break;
                case 6:
                    sdReader.SetDEFENCE(rear);
                    ReString = "防御力+" + rear.ToString();
                    break;
            }
        }
        else if (front == 7)//コマンド系
        {
            sdReader.SetHaveCommandTrue(code%1000);
            ReString = "技\n" + aReader.GetAbilityName(rear);

        }
        else if (front == 8)//コマンド枠増加
        {
            if (rear == 999)
                sdReader.SetCommandNumPlus(1); //一こ増やす
                ReString = "コマンド装備枠+" + 1;
        }
        else if (front == 9)//隠しステージ
        {
            if (rear == 999)
            {
                sdReader.SetHideQuestNum(1); //0隠しクエスト解放
                ReString = "隠しステージ";
            }
            else if (rear == 000)
            {
                ReString = "残念！はずれ";
            }
        }
        return ReString;
    }
    public string GetMatrixBenefit(int i,bool flag) //その後のためにstringを返す。 値はほぞんせず、確認用
    {
        string ReString = "Error";
        int code = GetNumInMatrixCode(i);//コードを値に変換
        sdReader.SetMatrixIsOpenTrue(i);//マトリックスをオープンに

        int front = (int)(code / 1000);
        int rear = code % 1000;

        if (1 <= front && front <= 6) //ステータスアップ系
        {

            switch (front)
            {
                case 1:
                    ReString = "HP+" + rear.ToString();
                    break;
                case 2:
                    ReString = "攻撃力+" + rear.ToString();
                    break;
                case 3:
                    ReString = "魔力+" + rear.ToString();
                    break;
                case 4:
                    ReString = "SP回復力+" + rear.ToString();
                    break;
                case 5:
                    ReString = "SP+" + rear.ToString();
                    break;
                case 6:
                    ReString = "防御力+" + rear.ToString();
                    break;
            }
        }
        else if (front == 7)//コマンド系
        {
            ReString = "技:" + aReader.GetAbilityName(rear);

        }
        else if (front == 8)//コマンド枠増加
        {
            if (rear == 999)
                ReString = "コマンド装備枠+1";
        }
        else if (front == 9)//隠しステージ
        {
            if (rear == 999)
            {
                
                ReString = "隠しステージ ";
            }
            else if (rear == 000)
            {
                ReString = "残念！はずれ";
            }
        }
        return ReString;
    }

    public bool isOpenMatrix(int i)//sdReaderからそれが解放済みか確認してくる。
    {
        bool[] b=sdReader.GetAllMatrixIsOpenBool();
        return b[i];
    }

    public int GetMatSortNum(int i)     //ソートするためにそれが何を解放する物なのかを番号で返す
    {
        int code = GetNumInMatrixCode(i);//コードを値に変換
        int front = (int)(code / 1000);

        return front;
    }
}
