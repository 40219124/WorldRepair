using UnityEngine;

public class MakeItRain : MonoBehaviour
{
    public ParticleSystem RainEffect;
    public float RainDuration = 6.0f;

    private void Start()
    {
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
