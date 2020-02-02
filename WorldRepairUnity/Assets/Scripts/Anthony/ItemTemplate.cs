using System;
using UnityEngine;

[Serializable]
public struct ItemAction
{
	public ItemTemplate ActionTarget;
	public BehaviourObj BehaviourObject;
	public string ActionText;

	public bool RequiresWater;
	public bool InWorld;
	public bool InInventory;

	public bool CanUse()
	{
		if (RequiresWater)
		{
			if (!WorldManager.Instance.HasRained)
			{
				return false;
			}
		}

		return true;
	}
}

[CreateAssetMenu]
public class ItemTemplate : ScriptableObject
{
	public string Name;
	public string Description;

	[Space]
	public Sprite Icon;

	[Space]
	public GameObject DroppedPrefab;
	
	[Space]
	public ItemAction[] Combinations;

	[Space]
	public bool DestroyOnUse;

	public ItemAction WorldInteraction;

	public Item Generate()
	{
		return new Item()
		{
			Template = this
		};
	}

	public ItemAction CanCombineInInventory(ItemTemplate other)
	{
		foreach (var combination in Combinations)
		{
			if (!combination.InInventory)
			{
				continue;
			}
			if (combination.ActionTarget == other)
			{
				return combination;
			}
		}

		foreach (var combination in other.Combinations)
		{
			if (!combination.InInventory)
			{
				continue;
			}
			if (combination.ActionTarget == this)
			{
				return combination;
			}
		}

		return default;
	}

	public ItemAction CanCombineInWorld(ItemTemplate other)
	{
		foreach (var combination in Combinations)
		{
			if (!combination.InWorld)
			{
				continue;
			}
			if (combination.ActionTarget == other)
			{
				return combination;
			}
		}

		foreach (var combination in other.Combinations)
		{
			if (!combination.InWorld)
			{
				continue;
			}
			if (combination.ActionTarget == this)
			{
				return combination;
			}
		}

		return default;
	}
}
