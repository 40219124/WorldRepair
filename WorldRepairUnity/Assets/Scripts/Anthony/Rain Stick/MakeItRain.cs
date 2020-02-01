using UnityEngine;

public class MakeItRain : MonoBehaviour
{
    public ParticleSystem RainEffect;
    public float RainDuration = 6.0f;

    private void Start()
    {
        Invoke("StopRaining", RainDuration);

        RainEffect.transform.SetParent(Camera.main.transform);
        RainEffect.transform.localPosition = Vector3.zero;

        RainEffect.gameObject.SetActive(true);
    }

    private void StopRaining()
    {
        RainEffect.Stop();
    }
}
