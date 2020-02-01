using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableBehaviour
    {
        Pickup,
        Trigger
    }

    public SpriteRenderer Renderer;
    public InteractableBehaviour Behaviour;

    [Header("Pickup")]
    public ItemTemplate PickupItemTemplate;
    [NonSerialized]
    public Item PickupItem;

    [Header("Trigger")]
    public string AnimationName;
    public bool Devoiding;

    private void Start()
    {
        if (Behaviour == InteractableBehaviour.Pickup &&
            PickupItemTemplate != null)
        {
            PickupItem = PickupItemTemplate.Generate();
        }
    }
}
