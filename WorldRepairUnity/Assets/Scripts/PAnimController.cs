using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAnimController : MonoBehaviour
{
    Animator anim;
    string[] layers = { "Right", "Left", "Up", "Down" };

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator<YieldInstruction> PickupAnimation()
    {
        anim.SetTrigger("Pickup");
        int layerIndex = anim.GetLayerIndex("Pickup");
        while (true)
        {
            yield return null;
            if (anim.GetCurrentAnimatorStateInfo(layerIndex).IsName("Empty"))
            {
                yield break;
            }
        }
    }

    public IEnumerator<YieldInstruction> DevoidAnimation()
    {
        anim.SetBool("Void", false);
        
        int layerIndex = anim.GetLayerIndex("VoidForm");
        while (true)
        {
            yield return null;
            if (anim.GetCurrentAnimatorStateInfo(layerIndex).IsName("NotVoid"))
            {
                Direction(Vector3.back);
                yield break;
            }
        }
    }

    public void SetVoid(bool state)
    {
        anim.SetBool("Void", state);
    }

    public void SetHolding(bool state)
    {
        anim.SetBool("Holding", state);
    }

    public void SetRaindance(bool state)
    {
        anim.SetBool("Rain", state);
    }

    public void Direction(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
            int offset = 0;
            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.z))
            {
                if (dir.x > 0)
                {
                    offset = 0;
                }
                else
                {
                    offset = 1;
                }
            }
            else
            {
                if (dir.z > 0)
                {
                    offset = 2;
                }
                else
                {
                    offset = 3;
                }
            }
            if (anim.GetLayerWeight(anim.GetLayerIndex(layers[offset])) != 1.0f)
            {
                anim.SetLayerWeight(anim.GetLayerIndex(layers[offset]), 1.0f);
                for (int i = 1; i < layers.Length; ++i)
                {
                    anim.SetLayerWeight(anim.GetLayerIndex(layers[(i + offset) % layers.Length]), 0.0f);
                }
            }
        }
    }
}
