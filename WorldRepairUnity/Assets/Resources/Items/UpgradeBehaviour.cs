using UnityEngine;
using System.Collections.Generic;

public class UpgradeBehaviour : BehaviourObj
{
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

        yield break;
    }
}
