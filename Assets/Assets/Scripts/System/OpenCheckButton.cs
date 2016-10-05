using UnityEngine;
using System.Collections;

public class OpenCheckButton : MonoBehaviour {
    public GameObject panel;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MatrixOpenToPanel()
    {
        panel.GetComponent<MatrixOpenPanel>().OpenMatrixBoard();
    }
}
