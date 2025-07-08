using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadialMenu : MonoBehaviour
{
    public List<RectTransform> menuItems;
    public float radius = 300f;
    public float rotateSpeed = 5f;
    private float angleStep;

    private void Start()
    {
        angleStep = 360f / menuItems.Count;
        PositionMenuItems();
    }

    private void PositionMenuItems()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            menuItems[i].anchoredPosition = pos;
        }
    }

    public void RotateMenu(float delta)
    {
        transform.Rotate(0f, 0f, delta * rotateSpeed);
    }
}
