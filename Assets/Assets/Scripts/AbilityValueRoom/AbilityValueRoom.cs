using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AbilityValueRoom : MonoBehaviour
{
    public GameObject Content;
    public GameObject ButtonPref;
    public Button memoButton;
    public MemoPanel panel;
    public Text[] text;

    int currentNum;

    GameObject[] Buttons;
    Ability[] ability;
    SaveDataReader sdReader;
    AbilityReader aReader;

    int[] haveAbilityNumList;
    // Use this for initialization
    void Start()
    {
        sdReader = FindObjectOfType<SaveDataReader>();
        aReader = GetComponent<AbilityReader>();
        haveAbilityNumList=sdReader.GetHaveCommandwithNumber();
        memoButton.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        ability = new Ability[haveAbilityNumList.Length];
        Buttons = new GameObject[haveAbilityNumList.Length];    //所持しているアビリティ分ボタンを拡張
        
        for (int i = 0; i <ability.Length; i++)//ここで各項目に情報をセット
        {
            ability[i] = aReader.GetAbility(haveAbilityNumList[i]); //アビリティ情報を格納
            Buttons[i] = Instantiate(ButtonPref);
            Buttons[i].GetComponent<AbilityValueViewButton>().SetNameAndNum(i,ability[i].Name);
            Buttons[i].transform.SetParent(Content.transform, false);
        }
    }
    void Update()
    {
        if (panel.gameObject.active)
        {
            memoButton.enabled = false;
        }
        else
            memoButton.enabled = true;
    }

    public void SetAbilityComponentView(int i)//ボタンから呼んでもらう　引数はそのボタンが持つ番号
    {
        text[0].text = ability[i].Name;
        text[1].text = ability[i].Id.ToString();
        string str;
        if (ability[i].ability == 0) str = "魔素吸収型物理攻撃";
        else str = "魔素使用型魔法";
        text[2].text = str;
        text[3].text = ability[i].Attack.ToString();
        switch ((int)ability[i].element) {
            case 0:
                str = "無属性";
                break;
            case 1:
                str = "赤属性";
                break;
            case 2:
                str = "青属性";
                break;
            case 3:
                str = "黄属性";
                break;
            case 4:
                str = "緑属性";
                break;
            case 5:
                str = "黒属性";
                break;
            case 6:
                str = "白属性";
                break;
        }
        text[4].text = str;
        text[5].text = ability[i].value.ToString();
        text[6].text = ability[i].Sp.ToString();

        currentNum = i;
        memoButton.gameObject.SetActive(true);
    }

    public void BackToSelectScene()
    { 
        FadeManager.Instance.LoadLevel("SelectScene", 1.5f);
    }

    public void OpenMemoBoard()
    {
        panel.OpenMemoPanel(ability[currentNum].memo);
    }
}
