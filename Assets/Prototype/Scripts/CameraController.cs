using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 zoomLimits;
    [SerializeField] float cameraSpeed;
    Transform mainCamera;
    Vector3 moveInput;

    void Awake()
    {
        mainCamera = Camera.main.transform;
        transform.LookAt(mainCamera);
    }

    void Update()
    {
        moveInput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        Vector3 movementDirection = mainCamera.TransformDirection(moveInput);
        movementDirection.y = 0;
        transform.position += movementDirection.normalized * Time.deltaTime * cameraSpeed;

    }
}
