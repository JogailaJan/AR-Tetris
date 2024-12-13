using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
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
        script.MoveBack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
