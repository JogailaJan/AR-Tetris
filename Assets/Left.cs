using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    private GameObject grid;
    private GridOfCubes script;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
        script = grid.GetComponent<GridOfCubes>();
    }
    public void OnMouseUpAsButton()
    {
        script.MoveLeft();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
