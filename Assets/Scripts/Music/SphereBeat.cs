using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBeat : MonoBehaviour
{
    Animator thisAnimator;
    Material initMaterial;
    Material finalMaterial;
    [SerializeField] private Color emissionColor;


    public float gbColor;
    private void OnEnable()
    {
        MusicManager.OnBeatDetect += UpdateColor;
    }

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
        initMaterial = GetComponent<Renderer>().materials[0];
        finalMaterial = initMaterial;
        GetComponent<Renderer>().materials[0] = finalMaterial;
    }

    private void FixedUpdate()
    {
        emissionColor.r = 1;
        emissionColor.g = gbColor;
        emissionColor.b = gbColor;
        emissionColor.a = 0;
        GetComponent<Renderer>().materials[0].SetColor("_color", emissionColor);
    }

    void UpdateColor()
    {
        thisAnimator.SetBool("Should Beat", true);
    }
}
