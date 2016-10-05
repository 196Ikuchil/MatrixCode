using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityValueViewButton : MonoBehaviour
{
    int AbilityNum;
    // Use this for initialization
    void Start()
    {
       
    }
    //パネルに自分のナンバーを返す
    public void OnClick()
    {
        FindObjectOfType<AbilityValueRoom>().SetAbilityComponentView(AbilityNum); 
    }

    //外から呼ばれる
    public void SetNameAndNum(int num,string name)
    {
        AbilityNum = num;
        SetText(name);
    }
    public void SetText(string str) //自分のtextを編集
    {
        transform.FindChild("Text").GetComponent<Text>().text = str;
    }
}
