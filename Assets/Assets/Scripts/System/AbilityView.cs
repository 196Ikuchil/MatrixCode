using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour {
    const int MaxCommandNum = 10;
    public Dropdown[] dropdowns;
    public GameObject sdReaderPref;
    public GameObject checkPref;

    AbilityReader aReader;
    SaveDataReader sdReader;

    GameObject panel;
    int havecommandNum; //現在の所持コマンド数
    int haveabilityNum; //現在の所持技数
    public int[] commandList;  //所持コマンド数一覧
    public int[] command; //装備コマンド
    string[] abilityName;

    public AudioClip saveSeClip;
    AudioSource saveSeSource;

    bool checkPanel=false;

    void Awake()
    {
        sdReader = Instantiate(sdReaderPref).GetComponent<SaveDataReader>();
        aReader = GetComponent<AbilityReader>();
        //これ以降AWAKEではaReaderは使わない　まだ向こうがawakeされていない　えらーが起こる

        havecommandNum = sdReader.GetCommandNum();
        commandList = sdReader.GetHaveCommandwithNumber();
        haveabilityNum = sdReader.GetHavetotalCommandNum(); 
        abilityName = new string[haveabilityNum];
    }
	// Use this for initialization
	void Start () {
        iTween.MoveFrom(this.gameObject, iTween.Hash("y", this.transform.position.y + 50f, "time", 1f));

        //sdreaderから現在のアビリティの装備しているセットをもってくる
        command = sdReader.GetCommandList();

        //for () ここであたらしいスクリプトで技を持ってくるものを作る
        for (int i = 0; i < abilityName.Length; i++)
        {
            abilityName[i] = aReader.GetAbilityName(commandList[i]);
        }
        //havecommandNum以降をアクティブを解除
        for(int i = havecommandNum; i < MaxCommandNum; i++)
        {
            dropdowns[i].gameObject.SetActive(false);
        }
        //stringをセットhavecommandnum分だけ
        for(int i = 0; i < havecommandNum; i++)//たたかうは0にセット固定
        {
            int stack = 0;
            if (i == 0) {
                dropdowns[i].options.Add(new Dropdown.OptionData { text = aReader.GetAbilityName(command[i]) });
                dropdowns[i].value = 1;
            }
            else
            {
                for (int j = 0; j < abilityName.Length; j++)
                {
                    if (aReader.GetAbilityName(command[i]) == abilityName[j]) //装備している技と同じなら先頭にセット
                        stack = j;
                    dropdowns[i].options.Add(new Dropdown.OptionData { text = abilityName[j] });
                }
                dropdowns[i].value = stack; //ここで最初に出てくるものを設定できる
                dropdowns[i].RefreshShownValue();
            }
        }

        saveSeSource = gameObject.AddComponent<AudioSource>();
        saveSeSource.loop = false;
        saveSeSource.clip = saveSeClip;
		saveSeSource.volume = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void DestroyPanel()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("y", this.transform.position.y + 150f, "time", 1f));
        sdReader.DestroyObject();
        Destroy(this.gameObject,1.5f);
    }

    public void SetAbility2(Dropdown i)
    {
        command[1] = commandList[i.value];
    }
    public void SetAbility3(Dropdown i)
    {
        command[2] = commandList[i.value];
    }
    public void SetAbility4(Dropdown i)
    {
        command[3] = commandList[i.value];
    }
    public void SetAbility5(Dropdown i)
    {
        command[4] = commandList[i.value];

    }
    public void SetAbility6(Dropdown i)
    {
        command[5] = commandList[i.value];
    }
    public void SetAbility7(Dropdown i)
    {
        command[6] = commandList[i.value];
    }
    public void SetAbility8(Dropdown i)
    {
        command[7] = commandList[i.value];
    }
    public void SetAbility9(Dropdown i)
    {
        command[8] = commandList[i.value];
    }
    public void SetAbility10(Dropdown i)
    {
        command[9] = commandList[i.value];
    }

    //ここのメソッドが呼ばれたら値をセーブする
    public void SaveCommand()
    {
        sdReader.SetCommandList(command);
        panel = (GameObject)Instantiate(checkPref, transform.position, Quaternion.identity);
        panel.transform.SetParent(this.transform.root.transform,false);
        panel.GetComponent<AbilityCheckPanel>().SetText("セーブしました");
    }

    public void saveSePlay()
    {
        saveSeSource.Play();
    }
}
