using UnityEngine;
using System.Collections;

public class MagicMoveScript : MonoBehaviour {
    public float waitTime = 0f;
    public bool move = true;
    // Use this for initialization
	void Start () {
        if (!move){
            Invoke("ChangeBool", waitTime);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(move)
            this.transform.position += (this.transform.forward);
	}
    void ChangeBool()
    {
        move = true;
    }

}
