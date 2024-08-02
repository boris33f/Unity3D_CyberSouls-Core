using UnityEngine;

public class SphereBeat : MonoBehaviour
{
    private Animator thisAnimator;

    private void Start()
    {
        MusicManager.OnBeatDetect.AddListener(BeatTrigger);
        MusicManager.OnMarkerDetect.AddListener(MarkerTrigger);
        thisAnimator = GetComponent<Animator>();
    }

    void BeatTrigger()
    {
        Debug.Log("A");
        thisAnimator.SetTrigger("Beat Trigger");
    }

    void MarkerTrigger()
    {
        Debug.Log("B");
        thisAnimator.SetTrigger("Marker Trigger");
    }
}
