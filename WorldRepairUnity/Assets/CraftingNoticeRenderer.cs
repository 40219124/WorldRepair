using UnityEngine;
using UnityEngine.UI;

public class CraftingNoticeRenderer : MonoBehaviour
{
    public CharacterInventory Target;

    public CanvasGroup RootGroup;
    public Text HeaderText;
    public Text DescriptionText;

    private void Start()
    {
        RootGroup.alpha = 0.0f;
    }

    private void Update()
    {
        var controller = Target.GetComponent<PlayerController>();
        if (Target.CurrentItem != null && controller.HeldItem != null
            && !controller.IsVoided)
        {
            var combination = Target.CurrentItem.Template.CanCombineInInventory(controller.HeldItem.Template);

            if (combination.ActionTarget != null)
            {
                RootGroup.alpha = 1.0f;
                HeaderText.text = $"Craft an item!";
                DescriptionText.text = $"{Target.CurrentItem.Template.Name} + {controller.HeldItem.Template.Name}";
                DescriptionText.gameObject.SetActive(true);
            }
            else
            {
                RootGroup.alpha = 0.0f;
            }
        }
        else
        {
            RootGroup.alpha = 0.0f;
        }
    }
}
