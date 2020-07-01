using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public Vector2 TouchDist;
    [HideInInspector]
    public Vector2 PointerOld;
    [HideInInspector]
    protected int PointerId;
    [HideInInspector]
    public bool Pressed;

    // Update is called once per frame
    void Update()
    {
        // If Pressed is true
        if (Pressed)
        {
            // If the TouchField was pressed
            if (PointerId >= 0 && PointerId < Input.touches.Length)
            {
                // It sets the current position of the finger minus the previous position
                TouchDist = Input.touches[PointerId].position - PointerOld;
                // It sets the position of the finger
                PointerOld = Input.touches[PointerId].position;
            }
            else
            {
                // It sets the current position of the mouse menus the previous position
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                // It sets the mouse position
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            // It sets the TouchDist to a Zero
            TouchDist = new Vector2();
        }
    }
    // This method is call when the finger touch the TouchField
    public void OnPointerDown(PointerEventData eventData)
    {
        // It sets the Pressed to true
        Pressed = true;
        // It sets the PointerId
        PointerId = eventData.pointerId;
        // It sets the position
        PointerOld = eventData.position;
	}

    // This method is call when the finger doesn't touch the TouchField
    public void OnPointerUp(PointerEventData eventData)
    {
        // It sets the Pressed to false
        Pressed = false;
    }
    
}