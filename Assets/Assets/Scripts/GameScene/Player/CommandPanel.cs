using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommandPanel : MonoBehaviour {
    public RawImage[] CommandPart;
    public Text text;
    public GameObject objectText;
    Vector3 objectpos;

    AbilityReader ability;
    int commandNum;
    int beforeSelect = 0;
    int[] commandList;
    public int currentSelect;
    void Awake()
    {
        PlayerData pData = PlayerData.Instance;
        ability = FindObjectOfType<AbilityReader>();
        //ability = GetComponent<AbilityReader>();
        commandNum = pData.CommandNum;
        commandList = pData.CommandList; //データのロード
        currentSelect=0;
    }
	// Use this for initialization
	void Start () {
        //アニメーションが不安定だからここで保存しておく
        objectpos = objectText.transform.position;

        this.SetCommandColor();
        SetSelectCommandColor(); //選択が変わったら、色を半透明に
        this.SetCommandNameToDisplay(); //表示名を変更

        
    }

    void SetCommandColor()//色を変更する
    {
        for(int i = 0; i < commandList.Length; i++)
        {
            CommandPart[i].color = ability.GetAbilityColor(commandList[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (beforeSelect != currentSelect)
        {
            SetSelectCommandColor(); //選択が変わったら、色を半透明に
            this.SetCommandNameToDisplay(); //表示名を変更
            beforeSelect = currentSelect;
        }
    }
    
    void SetCommandNameToDisplay()//表示しているアビリティ名を現在の選択しているものに変える
    {
        text.text = ability.GetAbilityName(commandList[currentSelect]);
        objectText.transform.position = objectpos;
        iTween.MoveFrom(objectText, iTween.Hash("z", objectText.transform.position.z+ 50,"time", 0.4f));
    }
    
    void SetSelectCommandColor()//選択しているもの以外半透明に
    {
        for(int i = 0; i < commandList.Length; i++)
        {
            float alpha = 0.5f;
            if (i == currentSelect) alpha=1f;
            CommandPart[i].color = new Color(CommandPart[i].color.r, CommandPart[i].color.g, CommandPart[i].color.b, alpha);
        }
    }

    public void SetCurrentSelectToNext()//選択を次に
    {
        currentSelect++;
        if (currentSelect >= commandNum) currentSelect = 0;
    }

    public Ability GetAbility() //現在の選択しているコマンドの情報をそのまま受け取る
    {
        return ability.GetAbility(this.currentSelect);
    }
    
    public int GetCurrentAbility()
    {
        return currentSelect;
    }
}
