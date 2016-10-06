using UnityEngine;
using System.Collections;

public class JaraiScript : MonoBehaviour {
    CapsuleCollider collider;
    float startTime;
    // Use this for initialization
	void Start () {
        collider = GetComponent<CapsuleCollider>();
        StartCoroutine(vanishCollider());
    }
	IEnumerator vanishCollider()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.2f);
            colliderChange();
        }
    }
    void colliderChange()
    {
        if (collider.enabled)
        {
            collider.enabled = false;
        }
        else
            collider.enabled = true;
    }
    
}
