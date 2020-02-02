using UnityEngine;
using System.Collections.Generic;

public class NoneBehaviour : BehaviourObj
{
    public ItemTemplate Template;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        yield break;
    }
}
