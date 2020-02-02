using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public ItemSlot[] Slots = new ItemSlot[5];

	public void Remove(Item item)
	{
		foreach (var slot in Slots)
		{
			if (slot.Contents == item)
			{
				slot.Contents = null;
			}
		}
	}
}

public class CharacterInventory : MonoBehaviour
{
	public Inventory Hotbar;
	public EventField<int> SelectedHotbarIcon = new EventField<int>();

	private float scrollLockout;

	private void Awake()
	{
		Hotbar = new Inventory()
		{
			Slots = new ItemSlot[]
			{
				new ItemSlot(),
				new ItemSlot(),
				new ItemSlot(),
				new ItemSlot(),
				new ItemSlot()
			}
		};
	}

	public Dictionary<KeyCode, int> Mappings = new Dictionary<KeyCode, int>()
	{
		[KeyCode.Alpha1] = 0,
		[KeyCode.Alpha2] = 1,
		[KeyCode.Alpha3] = 2,
		[KeyCode.Alpha4] = 3,
		[KeyCode.Alpha5] = 4,
		[KeyCode.Alpha6] = 5,
		[KeyCode.Alpha7] = 6,
		[KeyCode.Alpha8] = 7,
		[KeyCode.Alpha9] = 8,
	};

	public Item CurrentItem
	{
		get
		{
			var slot = CurrentSlot;

			return slot?.Contents;
		}
	}

	public ItemSlot CurrentSlot
	{
		get
		{
			if (SelectedHotbarIcon.Value < 0 || SelectedHotbarIcon.Value >= Hotbar.Slots.Length)
			{
				return null;
			}

			var slot = Hotbar.Slots[SelectedHotbarIcon.Value];

			return slot;
		}
	}

	private void Update()
	{
		foreach (var mapping in Mappings)
		{
			if (Input.GetKeyDown(mapping.Key))
			{
				SelectedHotbarIcon.Value = mapping.Value;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SelectedHotbarIcon.Value = -1;
		}

		bool scrollRight = false;
		bool scrollLeft = false;

		if (Time.unscaledTime > scrollLockout)
		{
			float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
			if (scroll > 0.01f)
			{
				scrollLeft = true;
				scrollLockout = Time.unscaledTime + 0.015f;
			}
			else if (scroll < -0.01f)
			{
				scrollRight = true;
				scrollLockout = Time.unscaledTime + 0.015f;
			}
		}

		if (Input.GetButtonDown("InvScrollRight"))
		{
			scrollRight = true;
		}
		else if (Input.GetButtonDown("InvScrollLeft"))
		{
			scrollLeft = true;
		}

		if (scrollRight)
		{
			int currentSlot = SelectedHotbarIcon.Value;
			currentSlot++;

			if (currentSlot >= Hotbar.Slots.Length)
			{
				currentSlot = 0;
			}
			SelectedHotbarIcon.Value = currentSlot;
		}

		if (scrollLeft)
		{
			int currentSlot = SelectedHotbarIcon.Value;
			currentSlot--;

			if (currentSlot < 0)
			{
				currentSlot = Hotbar.Slots.Length - 1;
			}
			SelectedHotbarIcon.Value = currentSlot;
		}
	}

	public void AddToHotbar(Item item)
	{
		foreach (var slot in Hotbar.Slots)
		{
			if (slot.Contents == null)
			{
				slot.Contents = item;
				return;
			}
		}
	}

	public bool CanPickupItem(Item item)
	{
		foreach (var slot in Hotbar.Slots)
		{
			if (slot.Contents == null)
			{
				return true;
			}
		}
		return false;
	}
}
