using UnityEngine;
using System.Collections.Generic;

public class MakeItRain : BehaviourObj
{
    public ParticleSystem RainEffect;
    public float RainDuration = 12.0f;
    public bool BringTheRain = false;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        RainEffect.gameObject.SetActive(false);

        player.IsInteracting = true;
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

        // Position the rain effect
        var camera = Camera.main.transform;
        gameObject.transform.position = camera.transform.position;
        gameObject.transform.SetParent(camera.transform);
        gameObject.transform.localRotation = Quaternion.identity;

        RainEffect.gameObject.SetActive(true);

        yield return new WaitForSeconds(4.0f);

        WorldManager.Instance.HasRained = true;

        // Fade the ground to fertile textures.
        yield return StartCoroutine(WorldManager.Instance.FadeToStyle(WorldManager.Instance.FertileStyle, 10.0f));

        WorldManager.Instance.StartCoroutine(WorldManager.Instance.ManagedFade(() =>
        {
            float percent = ((float)Plant.AllPlants.Count) / 40.0f;


            return percent;
        }));
    }

    private void StopRaining()
    {
        RainEffect.Stop();
    }
}
