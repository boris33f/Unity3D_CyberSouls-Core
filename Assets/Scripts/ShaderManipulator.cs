using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderManipulator : MonoBehaviour
{
    Material initMaterial;
    Material thisMaterial;
    public float minClip;
    public float maxClip;
    public float dissolveRing;

    void Start()
    {
        initMaterial = gameObject.GetComponent<MeshRenderer>().materials[0];
        thisMaterial = initMaterial;
        gameObject.GetComponent<MeshRenderer>().materials[0] = thisMaterial;
        thisMaterial.SetFloat("_minClip", minClip);
        thisMaterial.SetFloat("_maxClip", maxClip);
        thisMaterial.SetFloat("_dissolveRing", dissolveRing);
        thisMaterial.SetFloat("_dissolveAmount", 0);
    }
}
