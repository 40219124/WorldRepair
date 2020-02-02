using UnityEngine;
using System.Collections.Generic;

public class DecorateTarget : BehaviourObj
{
    public GameObject Decoration;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        foreach (Transform child in target.transform.GetChild(0))
        {
            if (child.gameObject.name == Decoration.name)
            {
                yield break;
            }
        }

        var clone = Instantiate(Decoration);
        clone.transform.SetParent(target.transform.GetChild(0));
        clone.transform.localPosition = Vector3.zero;
        clone.gameObject.name = Decoration.name;
    }
}
