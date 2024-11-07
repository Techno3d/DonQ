using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 360*4f;

    void LateUpdate()
    {
        float mousey = Input.GetAxis("Mouse Y"); 
        float rotY = -mousey*rotationSpeed*Mathf.Deg2Rad*Time.deltaTime*4;
        rotY = Mathf.Clamp(rotY, -60, 80);
        transform.Rotate(new Vector3(rotY, 0f, 0f));
    }
}