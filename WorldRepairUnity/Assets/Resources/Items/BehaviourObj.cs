using UnityEngine;
using System.Collections.Generic;


public abstract class BehaviourObj : MonoBehaviour
{
	public abstract IEnumerator<YieldInstruction> Run(PlayerController player, Interactable target);
}
