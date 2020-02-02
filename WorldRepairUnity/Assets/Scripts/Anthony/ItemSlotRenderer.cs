using UnityEngine;
using UnityEngine.UI;

public class ItemSlotRenderer : MonoBehaviour
{
    public Text ItemName;
    public Image ItemIcon;

    [Space]
    public Image SelectedGraphic;
    public Text SlotIndex;

    [Space]
    public Transform HeldOffsetter;
    public float HeldOffset;
    public float HideOffset = 142.0f;
    public float HoldLerp = 8.0f;

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
        UpdateSelected();

        if (SlotIndex != null)
        {
            SlotIndex.text = (index + 1).ToString();
        }
    }

    private void Start()
    {
        var targetPostion = new Vector3(0, -HideOffset, 0);

        HeldOffsetter.transform.localPosition = targetPostion;
    }

    private void Update()
    {
        var player = TargetCharacter.GetComponent<PlayerController>();

        Vector3 targetPostion;
        if (player.HeldItem == TargetSlot.Contents
            && TargetSlot.Contents != null)
        {
            targetPostion = new Vector3(0, HeldOffset, 0);
        }
        else
        {
            targetPostion = Vector3.zero;
        }

        if (player.IsVoided)
        {
            targetPostion = new Vector3(0, -HideOffset, 0);
        }

        HeldOffsetter.transform.localPosition = Vector3.Lerp(
            HeldOffsetter.transform.localPosition,
            targetPostion,
            Time.deltaTime * HoldLerp);
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
