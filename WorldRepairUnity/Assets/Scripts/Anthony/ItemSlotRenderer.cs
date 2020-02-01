using UnityEngine;
using UnityEngine.UI;

public class ItemSlotRenderer : MonoBehaviour
{
    public Text ItemName;
    public Image ItemIcon;

    [Space]
    public Image SelectedGraphic;
    public Text SlotIndex;

    private ItemSlot TargetSlot;
    private CharacterInventory TargetCharacter;
    private int Index;

    public void Render(ItemSlot target, CharacterInventory character, int index)
    {
        if (TargetCharacter != null)
        {
            TargetCharacter.SelectedHotbarIcon.OnAfterChanged -= UpdateSelected;
        }
        if (TargetSlot != null)
        {
            TargetSlot.OnChanged -= UpdateGraphics;
        }

        TargetSlot = target;
        TargetCharacter = character;
        Index = index;

        TargetCharacter.SelectedHotbarIcon.OnAfterChanged += UpdateSelected;
        TargetSlot.OnChanged += UpdateGraphics;

        UpdateGraphics();

        if (SelectedGraphic != null)
        {
            SelectedGraphic.gameObject.SetActive(false);
        }

        if (SlotIndex != null)
        {
            SlotIndex.text = (index + 1).ToString();
        }
    }

    private void UpdateGraphics()
    {
        if (ItemName != null)
        {
            if (TargetSlot.Contents == null)
            {
                ItemName.text = "";
            }
            else
            {
                ItemName.text = TargetSlot.Contents.Template.Name;
            }
        }

        if (ItemIcon != null)
        {
            if (TargetSlot.Contents == null)
            {
                ItemIcon.gameObject.SetActive(false);
            }
            else
            {
                ItemIcon.gameObject.SetActive(true);
                ItemIcon.sprite = TargetSlot.Contents.Template.Icon;
            }
        }
    }

    private void UpdateSelected()
    {
        if (SelectedGraphic != null)
        {
            SelectedGraphic.gameObject.SetActive(TargetCharacter.SelectedHotbarIcon.Value == Index);
        }
    }
}
