using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MemoPanel : MonoBehaviour {

    public Text text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenMemoPanel(string memo)
    {
        text.text = memo;
        iTween.MoveFrom(this.gameObject, iTween.Hash("y", this.transform.position.y + 100f, "time", 1f));
        this.gameObject.SetActive(true);
    }

    public void closeMemoPanel()
    {
        this.gameObject.SetActive(false);
    }
}
