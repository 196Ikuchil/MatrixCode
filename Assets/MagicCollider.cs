using UnityEngine;
using System.Collections;

public class MagicCollider : MonoBehaviour
{
    BoxCollider collider;
    float startTime;
    // Use this for initialization
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        StartCoroutine(vanishCollider());
    }
    IEnumerator vanishCollider()
    {
        for (;;)
        {
            yield return new WaitForSeconds(1f);
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