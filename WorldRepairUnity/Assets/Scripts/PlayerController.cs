using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [Space]
    public InteractionZone CharacterInteractionZone;
    public Transform DropPoint;
    public Transform PickupPoint;
    public SpriteRenderer HeldRenderer;

    public bool IsVoided = true;

    private PAnimController panim;
    private CharacterInventory inventory;
    private NavMeshAgent agent;

    public bool IsInteracting;

    public Item HeldItem
    {
        get
        {
            return internalHeldItem;
        }
        set
        {
            internalHeldItem = value;

            if (value == null)
            {
                HeldRenderer.gameObject.SetActive(false);
                panim.SetHolding(false);
            }
            else
            {
                HeldRenderer.sprite = HeldItem.Template.Icon;
                HeldRenderer.gameObject.SetActive(true);
                panim.SetHolding(true);
            }
        }
    }

    [NonSerialized]
    private Item internalHeldItem;

    private void Awake()
    {
        inventory = GetComponent<CharacterInventory>();
        panim = GetComponent<PAnimController>();
        agent = GetComponent<NavMeshAgent>();

        CharacterInteractionZone.ClaimOwnership(inventory);

        HeldRenderer.gameObject.SetActive(false);

        IsVoided = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsInteracting)
        {
            if (Input.GetButtonDown("IntWorld"))
            {
                if (HeldItem != null
                    && !IsVoided)
                {
                    // Can I craft my current object with this one in the world.
                    ItemAction combo = default;
                    var interaction = CharacterInteractionZone.GetInteraction();

                    if (interaction != null
                        && interaction.Behaviour == Interactable.InteractableBehaviour.Pickup)
                    {
                        combo = HeldItem.Template.CanCombineInWorld(interaction.PickupItemTemplate);
                    }

                    if (combo.BehaviourObject != null)
                    {
                        var clone = Instantiate(combo.BehaviourObject);
                        StartCoroutine(clone.Run(this, interaction));

                        if (HeldItem.Template.DestroyOnUse)
                        {
                            inventory.Hotbar.Remove(HeldItem);

                            HeldItem = null;
                        }
                    }
                    // Can I use my current object's "World Interaction"
                    else if(HeldItem.Template.WorldInteraction.BehaviourObject != null
                        && HeldItem.Template.WorldInteraction.CanUse())
                    {
                        var clone = Instantiate(HeldItem.Template.WorldInteraction.BehaviourObject);
                        StartCoroutine(clone.Run(this, interaction));

                        if (HeldItem.Template.DestroyOnUse)
                        {
                            inventory.Hotbar.Remove(HeldItem);

                            HeldItem = null;
                        }
                    }
                }
                else
                {
                    // Can I interact with an item infront of me?
                    var interaction = CharacterInteractionZone.GetInteraction();
                    if (interaction != null)
                    {
                        StartCoroutine(InteractRoutine(interaction));
                    }
                }
            }
            if (Input.GetButtonDown("IntInv")
                    && !IsVoided)
            {
                var selectSlot = inventory.CurrentItem;
                if (selectSlot == null)
                {
                    // Deselect
                    HeldItem = null;
                }
                else
                {
                    // Already held item
                    if (selectSlot == HeldItem)
                    {
                        // Deselect
                        HeldItem = null;
                    }
                    else
                    {
                        ItemAction combo = default;
                        if (HeldItem != null)
                        {
                            combo = HeldItem.Template.CanCombineInInventory(selectSlot.Template);
                        }

                        // Can't combine; swap the selected one.
                        if (combo.BehaviourObject == null)
                        {
                            HeldItem = selectSlot;
                        }
                        else
                        {
                            // Consume the two items used in crafting
                            inventory.Hotbar.Remove(selectSlot);
                            inventory.Hotbar.Remove(HeldItem);

                            HeldItem = null;

                            var clone = Instantiate(combo.BehaviourObject);
                            StartCoroutine(clone.Run(this, null));
                        }
                    }
                }
            }

            if (Input.GetButtonDown("Drop"))
            {
                var slot = inventory.CurrentSlot;
                if (slot?.Contents != null)
                {
                    if (slot.Contents == HeldItem)
                    {
                        HeldItem = null;
                    }

                    var clone = Instantiate(slot.Contents.Template.DroppedPrefab, DropPoint.position, Quaternion.identity, null);

                    var interactable = clone.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        interactable.PickupItemTemplate = slot.Contents.Template;
                    }
                    interactable.Renderer.sprite = slot.Contents.Template.Icon;

                    slot.Contents = null;
                }
            }
        }
    }

    public void InteractionFinished()
    {
        IsInteracting = false;
    }

    private IEnumerator<YieldInstruction> InteractRoutine(Interactable interactable)
    {
        IsInteracting = true;
        interactable.transform.SetParent(PickupPoint);
        interactable.transform.localPosition = Vector3.zero;

        yield return StartCoroutine(panim.PickupAnimation());

        if (interactable.Behaviour == Interactable.InteractableBehaviour.Pickup)
        {
            inventory.AddToHotbar(interactable.PickupItem);

            Destroy(interactable.gameObject);
        }
        else if (interactable.Behaviour == Interactable.InteractableBehaviour.Trigger)
        {
            if (interactable.Devoiding)
            {
                yield return StartCoroutine(panim.DevoidAnimation(this));
            }
        }

        IsInteracting = false;
    }

    private void FixedUpdate()
    {
        if (IsInteracting == false)
        {
            var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            move = move.normalized;
            panim.Direction(move);
            move *= speed;
            transform.position += move * Time.fixedDeltaTime;
        }
    }
}
