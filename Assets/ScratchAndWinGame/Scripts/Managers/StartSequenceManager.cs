using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to drive initial scrathing when our game starts
public class StartSequenceManager : MonoBehaviour
{
    public static StartSequenceManager instance;

    public GameObject Vertical;
    public GameObject Default;

    public AnimClipPlayer AnimIn;
    public AnimClipPlayer AnimOut;
    public AnimClipPlayer AnimIdle;

    public GameObject InitialBG;
    public TrailRenderer trail;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ResetLocation()
    {
        Vertical.transform.position = Default.transform.position;
    }

    public IEnumerator PlayAnimation()
    {
        DrawingManager.instance.ClearInstances();
        StartCoroutine(AnimIdle.Play());
        yield return StartSequence();
    }

    public IEnumerator StartSequence()
    {
        InitialBG.SetActive(true);
        trail.emitting = true;

        yield return AnimIn.Play();

        AnimIdle.StopPlayback();

        yield return AnimOut.Play();

        trail.emitting = false;
        trail.Clear();
        trail.widthMultiplier = 1;
        InitialBG.SetActive(false);
        ResetLocation();

    }
}
