using UnityEngine;
using System.Collections.Generic;

public class GiveItem : BehaviourObj
{
    public ItemTemplate Template;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        var item = Template.Generate();

        var inventory = player.GetComponent<CharacterInventory>();

        inventory.AddToHotbar(item);

        yield break;
    }
}
