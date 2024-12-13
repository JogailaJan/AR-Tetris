using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Right : MonoBehaviour
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
        script.MoveRight();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
