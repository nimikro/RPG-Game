using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerContoller : MonoBehaviour
{
    public Interactable focus;

    public LayerMask movementMask;

    Camera cam;
    PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Send a ray from the camera towards the mouse pointer and move the player
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                // Move the player to what we hit
                motor.MoveToPoint(hit.point);
                // Stop focusing any objects
                RemoveFocus();
            }
        }

        // Send a ray from the camera towards the mouse pointer, move the player and interact
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                // Check if we hit an interactable
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                // If we did set it as focus
                if(interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        // If our focus has changed
        if(newFocus != focus && focus != null)
        {
            // Remove previous focus 
            focus.OnDefocused(); 
        }

        // Set new focus
        focus = newFocus;
        // Let new focus know that it's being focused
        focus.OnFocused(transform);
        motor.FollowTarget(newFocus);
    }

    void RemoveFocus()
    {
        if(focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
        motor.StopFollowingTarget();
    }

}
