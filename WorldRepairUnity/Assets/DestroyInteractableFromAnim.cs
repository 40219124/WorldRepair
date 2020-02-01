using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInteractableFromAnim : MonoBehaviour
{
    public void DestroyInteractable()
    {
        Destroy(GetComponentInChildren<Interactable>(true)?.gameObject);
    }
}
