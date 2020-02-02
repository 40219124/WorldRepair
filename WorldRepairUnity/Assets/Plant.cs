using System.Collections.Generic;
using UnityEngine;

public class Plant : BehaviourObj
{
    public Animator anim;
    public Sprite[] RandomSprites;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        // Place this plant.
        if (target == null)
        {
            transform.position = player.DropPoint.position;
        }
        else
        {
            transform.position = target.transform.position;
        }

        if (target != null)
        {
            // We are a plant being used on something.
            // It's probably another plant.
            // Lets destroy the other plant.
            Destroy(target.gameObject);
        }

        // Have a random graphic.
        var thisInteractable = GetComponent<Interactable>();
        thisInteractable.Renderer.sprite = RandomSprites[Random.Range(0, RandomSprites.Length)];

        yield break;
    }
}
