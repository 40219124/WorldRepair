using UnityEngine;
using System.Collections.Generic;

public class TriggerGlobalEvent : BehaviourObj
{
    public static HashSet<string> Used = new HashSet<string>();

    public string OneTimeUseIdentifier;
    public GameObject Scatter;
    public int Count;
    public float Over;

    public override IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target)
    {
        if (!string.IsNullOrEmpty(OneTimeUseIdentifier))
        {
            if (!Used.Add(OneTimeUseIdentifier))
            {
                yield break;
            }
        }

        WorldManager.Instance.StartCoroutine(WorldManager.Instance.Scatter(Scatter, Count, Over));

        yield break;
    }
}
