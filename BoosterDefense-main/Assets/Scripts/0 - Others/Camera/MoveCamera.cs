using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float rotationSpeed;
    public float rotationSpeedBoost;
    public float rotationSpeedMouse;

    private Vector3 MaxMinAngle(Vector3 vec)
    {
        if (vec.x < 345 && vec.x >= 195)
            vec.x = 345;
        if (vec.x > 45 && vec.x < 195)
            vec.x = 45;
        return vec;
    }

    private void UpdateMoveKeys()
    {
        float speed = rotationSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed += rotationSpeedBoost;
        Vector3 vec = new Vector3(  transform.eulerAngles.x + Input.GetAxis("Vertical") * Time.unscaledDeltaTime * speed,
                                    transform.eulerAngles.y - Input.GetAxis("Horizontal") * Time.unscaledDeltaTime * speed,
                                    transform.eulerAngles.z);
        
        transform.eulerAngles = MaxMinAngle(vec);
    }

    private void UpdateMoveMouse()
    {
        Vector3 vec = new Vector3(  transform.eulerAngles.x + (-Input.GetAxis("Mouse Y")) * Time.unscaledDeltaTime * rotationSpeedMouse,
                                    transform.eulerAngles.y - (-Input.GetAxis("Mouse X")) * Time.unscaledDeltaTime * rotationSpeedMouse,
                                    transform.eulerAngles.z);
        
        transform.eulerAngles = MaxMinAngle(vec);
    }

    void Update()
    {
        if (!GameManager.instance.GameEnded) {
            //move with keys
            UpdateMoveKeys();
            //move with right click
            if (Input.GetMouseButton(1))
                UpdateMoveMouse();
        }
    }
}
