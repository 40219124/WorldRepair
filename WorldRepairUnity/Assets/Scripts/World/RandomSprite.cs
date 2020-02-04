using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] RandomSprites;

    private void Awake()
    {
        var thisInteractable = GetComponent<Interactable>();
        thisInteractable.Renderer.sprite = RandomSprites[Random.Range(0, RandomSprites.Length)];
    }
}
