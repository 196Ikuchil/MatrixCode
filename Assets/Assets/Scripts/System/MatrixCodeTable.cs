using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class MatrixCodeTable : MonoBehaviour {

    public GameObject view;
    public GameObject ButtonPref;
    public GameObject textObj;

    int currentSaveDataNum;

    MatrixCodeReader matrix;
    GameObject[] Buttons;
    SaveDataReader sdReader;

    int[] sortNumList;
    bool[] isOpen;
    // Use this for initialization
	void Start () {
        matrix = FindObjectOfType<MatrixCodeReader>();
        sdReader = FindObjectOfType<SaveDataReader>();
        currentSaveDataNum = sdReader.saveDataNum;
        isOpen = sdReader.GetAllMatrixIsOpenBool(currentSaveDataNum);
        int n = sdReader.GetMatrixOpenNum(currentSaveDataNum);

        Buttons = new GameObject[n];
        sortNumList = new int[n];

        int[] nums = sdReader.GetMatrixOpenNumbers();
        for(int i = 0; i < nums.Length; i++)
        {
            Buttons[i] = (GameObject)Instantiate(ButtonPref);
            Buttons[i].GetComponent<SelectMatrixNum>().SetMatNum(nums[i]);//番号をボタンにセット      
            Buttons[i].GetComponent<SelectMatrixNum>().SetPanel(this.gameObject);
            Buttons[i].transform.SetParent(view.transform,false);
            sortNumList[i]=matrix.GetComponent<MatrixCodeReader>().GetMatSortNum(nums[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetMatrixComponentView(int i)//ボタンから呼んでもらう　引数はそのボタンが持つコード
    {
       string str= matrix.GetComponent<MatrixCodeReader>().GetMatrixBenefit(i, false); //コードの説明を受け取ってくる
        textObj.GetComponent<Text>().text = str;
    }

    public void BackToBeforeScene()
    {
        string sceneName;
        if (currentSaveDataNum == 2)//タイトルへ
            sceneName = "TitleScene";
        else
            sceneName = "SelectScene";

            FadeManager.Instance.LoadLevel(sceneName, 1.5f);
    }
    public void SortMatrixButton(int i)
    {
        if (i == 0)
        {
            for (int k = 0; k < Buttons.Length; k++)
            {
                Buttons[k].SetActive(true);
            }
        }
        else
        {
            for (int k = 0; k < Buttons.Length; k++)
            {
                if (sortNumList[k] == i)
                    Buttons[k].SetActive(true);
                else
                    Buttons[k].SetActive(false);
            }
        }
    }
}
