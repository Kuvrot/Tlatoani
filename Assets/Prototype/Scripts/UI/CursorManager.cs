using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public static CursorManager instance;

    [Header("States")]
    public Texture2D basic , building;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        setBasicCursor();
    }

    // Update is called once per frame
    void Update()
    {
        


    }


    public void setBuildingCursor()
    {
        //Cursor.SetCursor(building, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void setBasicCursor()
    {
        //.SetCursor(basic, Vector2.zero, CursorMode.ForceSoftware);
    }


}
