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
        // Play animation here...

        yield return new WaitForSeconds(0.2f);
    }

    public void SetVoid(bool state)
    {
        anim.SetBool("Void", state);
    }

    public void SetRaindance(bool state)
    {
        anim.SetBool("Rain", state);
    }

    public void Direction(Vector3 dir)
    {
        if(dir == Vector3.zero)
        {
            anim.SetBool("Moving", false);
        }
        else
        {
            anim.SetBool("Moving", true);
            int offset = 0;
            if(Mathf.Abs(dir.x) >= Mathf.Abs(dir.z))
            {
                if(dir.x > 0)
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
                if(dir.z > 0)
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
