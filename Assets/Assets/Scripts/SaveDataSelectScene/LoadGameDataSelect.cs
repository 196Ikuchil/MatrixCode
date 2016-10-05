using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGameDataSelect : MonoBehaviour {

    public Text[] text;
    int saveDatanum = 2;
    void Awake()
    {
        for(int i = 0; i < saveDatanum; i++)
        {
            bool b=PlayerPrefsX.GetBool("FIRST_VISIT" + i,true);
            if (!b) text[i].text = "続きから";
        }
    }
    //データを選択し、保存、そしてスタート
    public void SetSaveDataAndStart(int i)
    {
        PlayerData pData = PlayerData.Instance;
        pData.saveDataNum = i;
        FadeManager.Instance.LoadLevel("SelectScene",2f);
    }
}
