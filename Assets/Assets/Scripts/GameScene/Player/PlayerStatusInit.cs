using UnityEngine;
using System.Collections;

public class PlayerStatusInit : MonoBehaviour {

    CharacterStatus status;
    AbilityReader aReader;
    PlayerData playerData;

    // Use this for initialization
    int HP;
    int POWER;
    int MAJICPOWER;
    int DEFENCE;
    int SP;
    int SPRECOVER;

    void Awake()
    {
        playerData = PlayerData.Instance;
        aReader = FindObjectOfType<AbilityReader>();
        //キャラクターのステータスを読み込んでくる くえすとを始める前に保存しておいてそこから読み取る
        HP = playerData.HP;
        POWER = playerData.POWER;
        MAJICPOWER = playerData.MAJICPOWER;
        DEFENCE = playerData.DEFENCE;
        SP = playerData.SP;
        SPRECOVER = playerData.SPRECOVER;
    }

	void Start () {

        status=gameObject.GetComponent<CharacterStatus>();
        //初期化
        status.SetHP(HP);
        status.SetPower(POWER);
        status.SetMPower(MAJICPOWER);
        status.SetSP(SP);
        status.SetSPRe(SPRECOVER);
        status.SetDefence(DEFENCE);

        //コマンド系をここで配列にしてステータスに渡す
        int commandNum= playerData.CommandNum;
        int[] list=playerData.CommandList;
        Ability[] abi = new Ability[commandNum];


        for(int i=0;i<abi.Length;i++)
        {
            abi[i] = aReader.GetAbility(list[i]);
        }
        //アビリティをステータスに渡す これでステータスに一通り技のことが保存された。
        status.SetAbility(abi);

	}
	
	
}
