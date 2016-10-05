using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterHPGUI : MonoBehaviour {

    CharacterStatus playerStatus;
    
    // ライフバー.
    public Image frontHPBar;
	Color playerFrontLifeBarColor = Color.green;

    void Awake()
    {
        PlayerCtrl player_ctrl = (PlayerCtrl)GameObject.FindObjectOfType(typeof(PlayerCtrl));
        playerStatus = player_ctrl.GetComponent<CharacterStatus>();
    }
    
    // Use this for initialization
	void Start () {
        this.initParameter();
    }
	
	// Update is called once per frame
	void Update () {
        frontHPBar.fillAmount = (float)playerStatus.HP / playerStatus.MaxHP;
	}

    private void initParameter()
    {
        frontHPBar.fillAmount = 1;
        frontHPBar.color = playerFrontLifeBarColor;
    }
}
