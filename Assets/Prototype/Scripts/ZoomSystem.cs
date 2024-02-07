using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomSystem : MonoBehaviour
{

    CinemachineVirtualCamera cinemachineVirtual;
    public float scrollSpeed = 100;
    float currentSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtual.m_Lens.OrthographicSize = 35;
        currentSize = cinemachineVirtual.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {

        currentSize += -Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;

        //Constraints

        if (currentSize > 60)
            currentSize = 60;
        else if (currentSize < 20)
            currentSize = 20;

        cinemachineVirtual.m_Lens.OrthographicSize = Mathf.Clamp(currentSize, 20, 60);
    }
}
