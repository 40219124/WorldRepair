using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [Space]
    public InteractionZone CharacterInteractionZone;
    public Transform DropPoint;
    public Transform PickupPoint;

    private PAnimController panim;
    private CharacterInventory inventory;
    private bool IsPickingUp;

    private void Awake()
    {
        inventory = GetComponent<CharacterInventory>();
        panim = GetComponent<PAnimController>();

        CharacterInteractionZone.ClaimOwnership(inventory);
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsPickingUp)
        {
            return;
        }

        if (Input.GetButtonDown("IntWorld"))
        {
            var interaction = CharacterInteractionZone.GetInteraction();
            if (interaction != null)
            {
                StartCoroutine(PickupRoutine(interaction));
            }
        }

        if (Input.GetButtonDown("Drop"))
        {
            if (inventory.SelectedHotbarIcon.Value >= 0 && inventory.SelectedHotbarIcon.Value < inventory.Hotbar.Slots.Length)
            {
                var slot = inventory.Hotbar.Slots[inventory.SelectedHotbarIcon.Value];

                if (slot.Contents != null)
                {
                    var clone = Instantiate(slot.Contents.Template.DroppedPrefab, DropPoint.position, Quaternion.identity, null);

                    var interactable = clone.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.PickupItemTemplate = slot.Contents.Template;
                    }

                    slot.Contents = null;
                }
            }
        }
    }

    private IEnumerator<YieldInstruction> PickupRoutine(Interactable interactable)
    {
        IsPickingUp = true;
        interactable.transform.SetParent(PickupPoint);
        interactable.transform.localPosition = Vector3.zero;

        yield return StartCoroutine(panim.PickupAnimation());

        interactable.Interact(inventory);

        IsPickingUp = false;
    }

    private void FixedUpdate()
    {
        if (IsPickingUp == false)
        {
            var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move = move.normalized;
            panim.Direction(move);
            move *= speed;
            transform.position += move * Time.fixedDeltaTime;
        }
    }
}
