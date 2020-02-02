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

        var interaction = controller.CharacterInteractionZone.GetInteraction();
        ItemAction combo = default;
        if (interaction != null
            && interaction.Behaviour == Interactable.InteractableBehaviour.Pickup
            && controller.HeldItem != null)
        {
            combo = controller.HeldItem.Template.CanCombineInWorld(interaction.PickupItemTemplate);
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
        else if (combo.BehaviourObject != null
            && combo.CanUse())
        {
            HeaderText.text = combo.ActionText;
            RootGroup.alpha = 1.0f;
        }
        else
        {
            RootGroup.alpha = 0.0f;
        }
    }
}
