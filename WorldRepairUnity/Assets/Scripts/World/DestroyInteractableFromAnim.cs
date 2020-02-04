using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInteractableFromAnim : MonoBehaviour
{
    public void DestroyInteractable()
    {
        Destroy(GetComponentInChildren<Interactable>(true)?.gameObject);
    }

    public void FinishRaindance()
    {
        GetComponentInParent<PlayerController>()?.InteractionFinished();
        MakeItRain mir = FindObjectOfType<MakeItRain>();
        if (mir != null)
        {
            mir.BringTheRain = true;
        }
        GetComponentInParent<PAnimController>()?.SetRaindance(false);
    }
}
