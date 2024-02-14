using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 zoomLimits;
    [SerializeField] float cameraSpeed;
    Transform mainCamera;
    Transform zoomObject;
    Vector2 input;
    bool mouseMove = true;
    Vector3 moveInput;
    Vector3 initialPosition;

    bool isSettlmentOnScene = false;

    public bool cheats = false;

    public bool isPaused = false;

    void Awake()
    {
        mainCamera = Camera.main.transform;
        transform.LookAt(mainCamera);
        zoomObject = transform.GetChild(0);
        initialPosition = transform.position;
    }

    void Update()
    {

        //If there is a settlement, that settlement will be the new inital position

        if (!isSettlmentOnScene)
        {
            if (BuildingManager.instance.allBuildings.Count > 0)
            {
                foreach (Building building in BuildingManager.instance.allBuildings)
                {
                    if (building.buildingName == "Settlement")
                    {
                        initialPosition = new Vector3(building.transform.position.x, initialPosition.y, building.transform.position.z);
                        isSettlmentOnScene = true;
                    }
                }
            }
        }

        moveInput.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (mouseMove)
        {
            Vector2 mousePos = Input.mousePosition;

            if (mousePos.x > Screen.width * 0.95f && mousePos.x < Screen.width)
                moveInput.x = 1;
            else if (mousePos.x < Screen.width * 0.05f && mousePos.x > 0)
                moveInput.x = -1;

            if (mousePos.y > Screen.height * 0.95f && mousePos.y < Screen.height)
                moveInput.z = 1;
            else if (mousePos.y < Screen.height * 0.05f && mousePos.y > 0)
                moveInput.z = -1;
        }

        Vector3 movementDirection = mainCamera.TransformDirection(moveInput);
        movementDirection.y = 0;
        transform.position += movementDirection.normalized * Time.deltaTime * cameraSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = initialPosition;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        if (cheats)
        {
            if (Input.GetKeyDown(KeyCode.U) && Time.timeScale == 1)
            {
                Time.timeScale = 10;
            }
            else if (Input.GetKeyDown(KeyCode.U) && Time.timeScale == 10)
            {
                Time.timeScale = 1;
            }
        }

       // zoomObject.localPosition += new Vector3(0, 0, -Input.mouseScrollDelta.y);
       // zoomObject.localPosition = new Vector3(zoomObject.localPosition.x, zoomObject.localPosition.y, Mathf.Clamp(zoomObject.localPosition.z, zoomLimits.x, zoomLimits.y));

    }



}


