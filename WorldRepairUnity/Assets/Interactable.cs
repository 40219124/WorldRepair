using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableBehaviour
    {
        Pickup,
        Trigger
    }

    public InteractableBehaviour Behaviour;

    [Header("Pickup")]
    public ItemTemplate PickupItemTemplate;
    [NonSerialized]
    public Item PickupItem;

    [Header("Trigger")]
    public string AnimationName;

    private void Start()
    {
        PickupItem = PickupItemTemplate.Generate();
    }

    public void Interact(CharacterInventory character)
    {
        switch (Behaviour)
        {
            case InteractableBehaviour.Pickup:

                character.AddToHotbar(PickupItem);

                Destroy(gameObject);

                break;

            case InteractableBehaviour.Trigger:

                break;
        }
    }
}
