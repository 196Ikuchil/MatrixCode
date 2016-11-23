using UnityEngine;
using System.Collections;

public class QuestStartLogo : MonoBehaviour {
    public AudioSource startSeSource;
	// Use this for initialization
	IEnumerator Start () {

        startSeSource.Play();
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(this.gameObject, iTween.Hash("x", this.transform.position.x + 100f, "time", 1f));
        Destroy(this.gameObject,1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
