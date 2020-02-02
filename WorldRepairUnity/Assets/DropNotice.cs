using UnityEngine;

public class DropNotice : MonoBehaviour
{
    public CharacterInventory Target;

    public CanvasGroup RootGroup;

    public static float ActivateTime = -100.0f;

    private void Start()
    {
        RootGroup.alpha = 0.0f;
    }

    private void Update()
    {
        if (ActivateTime + 3.0f < Time.time)
        {
            RootGroup.alpha = 0.0f;
        }
        else
        {
            RootGroup.alpha = 1.0f;
        }
    }
}
