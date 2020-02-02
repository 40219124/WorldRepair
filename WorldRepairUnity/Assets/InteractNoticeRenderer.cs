using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InteractNoticeRenderer : MonoBehaviour
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

        var interact = controller.CharacterInteractionZone.GetInteraction();

        ItemAction interactCombination = default;
        if (controller.HeldItem != null
            && interact != null
            && interact.Behaviour == Interactable.InteractableBehaviour.Pickup)
        {
            var interactAsItem = interact.PickupItemTemplate;

            interactCombination = controller.HeldItem.Template.Combinations
                .Where(combo => combo.ActionTarget == interactAsItem)
                .FirstOrDefault();
        }

        if (interact != null && controller.HeldItem == null)
        {
            DescriptionText.gameObject.SetActive(false);

            RootGroup.alpha = 1.0f;
            if (interact.Behaviour == Interactable.InteractableBehaviour.Pickup)
            {
                HeaderText.text = $"Pickup {interact.PickupItemTemplate.Name}";
            }
            else if (interact.Behaviour == Interactable.InteractableBehaviour.Trigger)
            {
                HeaderText.text = $"Use {interact.name}";
            }
            else
            {
                RootGroup.alpha = 0.0f;
            }
        }
        else if (controller.HeldItem != null
            && controller.HeldItem.Template.WorldInteraction.BehaviourObject != null
            && controller.HeldItem.Template.WorldInteraction.CanUse())
        {
            HeaderText.text = controller.HeldItem.Template.WorldInteraction.ActionText;
            RootGroup.alpha = 1.0f;
        }
        else if (interactCombination.BehaviourObject != null
            && interactCombination.CanUse())
        {
            HeaderText.text = interactCombination.ActionText;
            RootGroup.alpha = 1.0f;
        }
        else
        {
            RootGroup.alpha = 0.0f;
        }
    }
}
