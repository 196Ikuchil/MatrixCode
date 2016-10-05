using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordEffect : MonoBehaviour {

    public ParticleSystem[] effect;
    int currentNum = 0;

    public void  Start()
    {
        foreach(ParticleSystem p in effect)
        {
            p.Stop();
        }
    }
    public void StartEffect(int i)
    {
        currentNum = i;
        effect[i].Play();
    }
    public void StopEffect()
    {
        effect[currentNum].Stop();
    }
}
