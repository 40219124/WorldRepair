using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRandomiser : MonoBehaviour
{
    [SerializeField]
    List<RuntimeAnimatorController> birdVariants = new List<RuntimeAnimatorController>();

    // Start is called before the first frame update
    void Start()
    {
        int r = Random.Range(0, birdVariants.Count);
        if (birdVariants.Count > 1)
        {
            GetComponent<Animator>().runtimeAnimatorController = birdVariants[r];
        }
        if(r == 0)
        {
            GetComponentInParent<AnimalWander>().enabled = false;
        }
    }
    
}
