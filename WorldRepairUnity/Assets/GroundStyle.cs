using UnityEngine;

[CreateAssetMenu]
public class GroundStyle : ScriptableObject
{
	public Sprite[] Sprites;

	public Sprite RandomSprite()
	{
		return Sprites[Random.Range(0, Sprites.Length)];
	}
}
