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

        if (interact != null)
        {
            if (interact.Behaviour == Interactable.InteractableBehaviour.Pickup)
            {
                HeaderText.text = $"Pickup {interact.PickupItemTemplate.Name}";
            }
            else
            {
                HeaderText.text = $"Use {interact.name}";
            }
            DescriptionText.gameObject.SetActive(false);

            RootGroup.alpha = 1.0f;
        }
        else
        {
            RootGroup.alpha = 0.0f;
        }
    }
}
