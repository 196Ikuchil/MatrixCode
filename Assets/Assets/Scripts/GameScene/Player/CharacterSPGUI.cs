using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSPGUI : MonoBehaviour
{

    CharacterStatus playerStatus;

    // ライフバー.
    public Image frontSPBar;
    Color playerFrontLifeBarColor = Color.yellow;

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
        frontSPBar.fillAmount = (float)playerStatus.SP / playerStatus.MaxSP;
    }

    private void initParameter()
    {
        frontSPBar.fillAmount = 1;
        frontSPBar.color = playerFrontLifeBarColor;
    }
}
