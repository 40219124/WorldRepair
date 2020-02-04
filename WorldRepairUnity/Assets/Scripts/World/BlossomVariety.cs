using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomVariety : MonoBehaviour
{
    public Sprite[] RandomSprites;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = RandomSprites[GetComponentInParent<Plant>().TreeType];
    }
}
