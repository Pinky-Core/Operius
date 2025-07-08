using UnityEngine;

public class TouchRotator : MonoBehaviour
{
    public Transform radialMenu;
    private Vector2 startTouch;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
                startTouch = t.position;

            else if (t.phase == TouchPhase.Moved)
            {
                float deltaX = t.position.x - startTouch.x;
                radialMenu.Rotate(0, 0, -deltaX * 0.3f);
                startTouch = t.position;
            }
        }
    }
}
