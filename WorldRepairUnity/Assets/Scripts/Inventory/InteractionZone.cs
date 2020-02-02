using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
	private CharacterInventory Owner;
	public List<Interactable> InArea = new List<Interactable>();

	public void ClaimOwnership(CharacterInventory owner)
	{
		Owner = owner;
	}

	private void OnTriggerEnter(Collider other)
	{
		var otherInteractable = other.GetComponent<Interactable>();
		if (otherInteractable != null)
		{
			InArea.Add(otherInteractable);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var otherInteractable = other.GetComponent<Interactable>();
		if (otherInteractable != null)
		{
			InArea.Remove(otherInteractable);
		}
	}

	public bool CanInteract()
	{
		Prune();

		return InArea.Count > 0;
	}

	public Interactable GetInteraction()
	{
		Prune();

		if (InArea.Count == 0)
		{
			return null;
		}
		foreach (var inArea in InArea)
		{
			if (inArea.Behaviour == Interactable.InteractableBehaviour.Pickup
				&& Owner.GetComponent<PlayerController>().IsVoided)
			{
				continue;
			}
			return inArea;
		}

		return null;
	}

	private void Prune()
	{
		for (int i = InArea.Count - 1; i >= 0; i--)
		{
			if (InArea[i] == null)
			{
				InArea.RemoveAt(i);
			}
		}
	}
}
