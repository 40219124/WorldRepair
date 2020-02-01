﻿using UnityEngine;

[CreateAssetMenu]
public class ItemTemplate : ScriptableObject
{
	public string Name;
	public string Description;

	[Space]
	public Sprite Icon;

	[Space]
	public GameObject DroppedPrefab;

	public Item Generate()
	{
		return new Item()
		{
			Template = this
		};
	}
}
