using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckPanel : MonoBehaviour {

    public Text text;
	// Use this for initialization
	void Start () {
	    
	}

    public void SetText(string str)
    {
        text.text = str;
    }
    public void DestroyPanel()
    {
        FindObjectOfType<MatrixOpenPanel>().DestroyPanel();
        Destroy(this.gameObject);
    }
}
