using UnityEngine;
using System.Collections;

public class TouchSeScript : MonoBehaviour {

    public AudioSource clickSeSource;
	// Use this for initialization
	void Start () {
       //clickSeSource = gameObject.AddComponent<AudioSource>();
        //clickSeSource.clip = clickSeClip;
        //clickSeSource.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase==TouchPhase.Ended)
                ClickSePlay();
        }
	}

    void ClickSePlay()
    {
        clickSeSource.Play();
    }
}
