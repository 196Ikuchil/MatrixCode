using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameObject statusViewPref;
    public GameObject abilityViewPRef;

    public Canvas canvas { get; set; }

    void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        
    }
	// Use this for initialization
	void Start () {
        iTween.MoveFrom(this.gameObject,iTween.Hash("y",this.transform.position.y+50f,"time",1f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenMatrixCodeMenu()
    {
        FadeManager.Instance.LoadLevel("MatrixCodeRoomScene",1.5f);
    }

    public void OpenAbilityMenu()
    {
        GameObject m = (GameObject)Instantiate(abilityViewPRef);
        m.transform.SetParent(canvas.transform, false);
    }

    public void OpenStatusMenu()
    {
        GameObject m = (GameObject)Instantiate(statusViewPref);
        m.transform.SetParent(canvas.transform,false);
    }

    public void GotoAbilityValueRoom()
    {
        FadeManager.Instance.LoadLevel("AbilityValueRoom",1.5f);
    }

    public void DestroyMenu()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("y", this.transform.position.y + 120f, "time", 1f));
        Destroy(this.gameObject,1f);
    }
}
