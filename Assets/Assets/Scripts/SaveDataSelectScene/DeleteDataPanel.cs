using UnityEngine;
using System.Collections;

public class DeleteDataPanel : MonoBehaviour {

    int deleteNum;

    void Awake()
    {
        this.gameObject.SetActive(false);
    }
    // Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void  SetDeleteNum(int i)
    {
        deleteNum = i;
    }

    public void DeleteData()
    {
        PlayerPrefsX.SetBool("FIRST_VISIT" + deleteNum, true);
        FadeManager.Instance.LoadLevel("SaveDataSelectScene", 2f);
    }

    public void CancelPanel()
    {
        this.gameObject.SetActive(false);
    }
}
