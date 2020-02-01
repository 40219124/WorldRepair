using System;
using UnityEngine;

[Serializable]
public struct Combination
{
	public ItemTemplate OtherItem;
	public GameObject BehaviourObject;
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
	public Combination[] Combinations;

	[Space]
	public bool DestroyOnUse;
	public GameObject WorldInteraction;

	public Item Generate()
	{
		return new Item()
		{
			Template = this
		};
	}
}
