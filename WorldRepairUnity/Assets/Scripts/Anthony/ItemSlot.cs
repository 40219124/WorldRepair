using System;

[Serializable]
public class ItemSlot
{
	public Action OnChanged;

	private Item contents;

	public Item Contents
	{
		get
		{
			return contents;
		}
		set
		{
			contents = value;
			OnChanged?.Invoke();
		}
	}


}
