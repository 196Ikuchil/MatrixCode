using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UseSPGUIScript : MonoBehaviour
{

    CharacterStatus playerStatus;

    // ライフバー.
    public Image frontSPBar;
    public Image backSPBar;
    Color playerFrontUseSPBarColor = Color.cyan;
    Color playerFrontOverUseSPColor = Color.red;

    void Awake()
    {
        PlayerCtrl player_ctrl = (PlayerCtrl)GameObject.FindObjectOfType(typeof(PlayerCtrl));
        playerStatus = player_ctrl.GetComponent<CharacterStatus>();
    }

    // Use this for initialization
    void Start()
    {
        this.initParameter();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].ability == 0)
        {
            frontSPBar.color = Color.blue;
            frontSPBar.fillAmount = (float)playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp* playerStatus.SPRecoverPower  *playerStatus.GetSpboostMag() / playerStatus.MaxSP;
            backSPBar.fillAmount = (float)playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp * playerStatus.SPRecoverPower * playerStatus.GetSpboostMag() / playerStatus.MaxSP;
        }
        else { 
            if (playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp > playerStatus.MaxSP)
                frontSPBar.color = Color.red;
            else
                frontSPBar.color = playerFrontUseSPBarColor;
            frontSPBar.fillAmount = (float)playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp / playerStatus.MaxSP;
            backSPBar.fillAmount = (float)playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp / playerStatus.MaxSP;
        }
    }

    private void initParameter()
    {
        frontSPBar.fillAmount = playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp / playerStatus.MaxSP;
        frontSPBar.color = playerFrontUseSPBarColor;
        backSPBar.fillAmount= playerStatus.abilityList[playerStatus.currentAbilitySelectNumbe].Sp / playerStatus.MaxSP;
    }
}
