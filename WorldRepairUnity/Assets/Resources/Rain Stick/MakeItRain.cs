﻿using UnityEngine;
using System.Collections.Generic;


public class MakeItRain : MonoBehaviour
{
    public ParticleSystem RainEffect;
    public float RainDuration = 12.0f;
    public bool BringTheRain = false;

    private void Start()
    {
        RainEffect.gameObject.SetActive(false);
        StartCoroutine(DelayedRain());
    }

    private IEnumerator<YieldInstruction> DelayedRain()
    {
        FindObjectOfType<PAnimController>()?.SetRaindance(true);
        while (true)
        {
            yield return null;
            if (BringTheRain)
            {
                break;
            }
        }

        RainEffect.gameObject.SetActive(true);
        Invoke("StopRaining", RainDuration);

        var camera = Camera.main.transform;

        gameObject.transform.position = camera.transform.position;
        gameObject.transform.SetParent(camera.transform);

        gameObject.transform.localRotation = Quaternion.identity;

        RainEffect.gameObject.SetActive(true);
    }

    private void StopRaining()
    {
        RainEffect.Stop();
    }
}
