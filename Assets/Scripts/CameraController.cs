using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 360*4f;

    void LateUpdate()
    {
        float mousey = Input.GetAxis("Mouse Y"); 
        float rotY = -mousey*rotationSpeed*Mathf.Deg2Rad*Time.deltaTime*4;
        float funny = (transform.eulerAngles.x + 180)%360 - 180;
        Debug.Log(funny + " " + rotY);
        if(funny > 71 && rotY > 0) {
            return;
        } else if(funny < -50 && rotY < 0) {
            return;
        }
        transform.Rotate(new Vector3(rotY, 0f, 0f));
    }
}