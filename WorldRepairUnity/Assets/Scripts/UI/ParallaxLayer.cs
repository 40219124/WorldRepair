using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ParallaxLayer : MonoBehaviour
{
    public float ParallaxHorizontal = 0.25f;
    public float ParallaxVertical = 0.25f;

    private RectTransform ThisRectTransform;

    private void Awake()
    {
        ThisRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var normalisedMousePosition = new Vector2(
            Input.mousePosition.x / Screen.width,
            Input.mousePosition.y / Screen.height);

        var remaped = new Vector2(
            (normalisedMousePosition.x * 2) - 1.0f,
            (normalisedMousePosition.y * 2) - 1.0f);

        remaped *= new Vector2(ParallaxHorizontal, ParallaxVertical);

        var unremapped = new Vector2(
            (remaped.x + 1) * 0.5f,
            (remaped.y + 1) * 0.5f);

        ThisRectTransform.pivot = unremapped;
        ThisRectTransform.offsetMin = Vector3.zero;
        ThisRectTransform.offsetMax = Vector3.zero;

        ThisRectTransform.sizeDelta = ThisRectTransform.parent.GetComponent<RectTransform>().sizeDelta;
    }
}
