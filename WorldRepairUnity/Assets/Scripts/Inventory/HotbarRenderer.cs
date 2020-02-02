using System;
using UnityEngine;

[Serializable]
public class ItemSlotRendererPool : UIPool<ItemSlotRenderer> { }

public class HotbarRenderer : MonoBehaviour
{
	public CharacterInventory RenderOnStart;

	[Space]
	public RectTransform SlotHolder;
	public ItemSlotRendererPool RendererPool;

	private CharacterInventory CurrentTarget;

	public void Start()
	{
		if (RenderOnStart != null)
		{
			Render(RenderOnStart);
		}
	}

	public void Render(CharacterInventory player)
	{
		CurrentTarget = player;

		RendererPool.Flush();

		for (int i = 0; i < player.Hotbar.Slots.Length; i++)
		{
			var slot = player.Hotbar.Slots[i];
			var slotRenderer = RendererPool.Grab(SlotHolder);

			slotRenderer.Render(slot, CurrentTarget, i);
		}
	}
}
