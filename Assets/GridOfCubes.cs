using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;
///Kompiuterio  versija
public class GridOfCubes : MonoBehaviour
{
    public GameObject[] corners;
    private GameObject grid;
    public GameObject cubePrefab; // Assign your cube prefab in Unity's inspector
    private float prevTime;
    public float fallTime = 0.5f;
    private int gridSizeX = 10;  // Number of cubes in the X-axis of the grid
    private int gridSizeY = 20;  // Number of cubes in the Y-axis of the grid
    private int gridSizeZ = 10;  // Number of cubes in the Z-axis of the grid
    GameObject[,,] cubes = new GameObject[10, 20, 10];
    int[,,] cubeStatus = new int[10, 20, 10];
    private int currentY; // Track the current y-coordinate of the tetromino
    private int currentX; // Declare currentX as a member variable
    private int currentZ; // Declare currentZ as a member variable
    private bool spawnTetromino = true;
    public TetrominoShape shape;
    private bool paused = true;
    private bool gameStarted = false;

    // Tetromino shapes
    public class TetrominoShape
    {
        public int[,,] Shape { get; set; }
        public Color Color { get; set; }
    }

    private TetrominoShape tetrominoLShape = new TetrominoShape
    {
        Shape = new int[,,]
    {
        {
            { 1, 1, 1 },
            { 1, 0, 0 }
        }
    },
        Color = new Color(1.0f, 0.64f, 0.0f)
};
    private TetrominoShape tetrominoJShape = new TetrominoShape
    {
        Shape = new int[,,]
    {
        {
            { 1, 1, 1 },
            { 0, 0, 1 }
        }
    },
        Color = Color.blue
    };

    private TetrominoShape tetrominoOShape = new TetrominoShape
    {
        Shape = new int[,,]
        {
        {
            { 1, 1 },
            { 1, 1 }
        }
        },
        Color = Color.yellow
    };

    private TetrominoShape tetrominoSShape = new TetrominoShape
    {
        Shape = new int[,,]
        {
        {
            { 0, 1, 1 },
            { 1, 1, 0 }
        }
        },
        Color = Color.green
    };
    private TetrominoShape tetrominoZShape = new TetrominoShape
    {
        Shape = new int[,,]
        {
        {
            { 1, 1, 0 },
            { 0, 1, 1 }
        }
        },
        Color = Color.red
    };
    private TetrominoShape tetrominoTShape = new TetrominoShape
    {
        Shape = new int[,,]
        {
        {
            { 1, 1, 1 },
            { 0, 1, 0 }
        }
        },
        Color = Color.magenta
    };
    private TetrominoShape tetrominoIShape = new TetrominoShape
    {
        Shape = new int[,,]
   {
        {
            { 1, 1, 1, 1 }
        }
   },
        Color = Color.cyan
    };
    private TetrominoShape tetrominoTestShape = new TetrominoShape
    {
        Shape = new int[,,]
   {
        {
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1}
        },
        {
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1}
        }
   },
        Color = Color.magenta
    };
    private TetrominoShape[] tetrominoShapes;

    void Start()
    {
        grid = GameObject.FindGameObjectWithTag("Grid");
        tetrominoShapes = new TetrominoShape[] { tetrominoLShape, tetrominoJShape, tetrominoOShape, tetrominoIShape, tetrominoZShape, tetrominoTShape, tetrominoSShape };
        //tetrominoShapes = new TetrominoShape[] { tetrominoTestShape };
        deployCubes();
    }

    void Update()
    {
        if (spawnTetromino && !paused && gameStarted)
        {
            currentY = 19;
            currentX = 4;
            currentZ = 4;
            SpawnTetromino();
            prevTime = Time.time;
            spawnTetromino = false;
        }
        else if (Time.time - prevTime > fallTime && !paused && gameStarted)
        {
            MoveY();
            prevTime = Time.time;
        }
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Debug.Log("Left");
                MoveX(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Debug.Log("Right");
                MoveX(Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                Debug.Log("Forward");
                MoveZ(Vector3.back);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log("Back");
                MoveZ(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Debug.Log("Rotate Left");
                RotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Debug.Log("Rotate Right");
                RotateRight();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Paused/Unpaused");
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameStarted = false;
            Debug.Log("Start/Restart");
            StartGame();
        }
    }
    public void MoveLeft()
    {
        Debug.Log("Left");
        MoveX(Vector3.left);
    }
    public void MoveRight()
    {
        Debug.Log("Right");
        MoveX(Vector3.right);
    }
    public void MoveForward()
    {
        Debug.Log("Forward");
        MoveZ(Vector3.back);
    }
    public void MoveBack()
    {
        Debug.Log("Back");
        MoveZ(Vector3.forward);
    }
    public void Pause()
    {
        if (paused)
        {
            paused = false;
        }
        else
        {
            paused = true;
        }
    }
    public void StartGame()
    {
        Debug.Log("Start/Restart");
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    cubeStatus[x, y, z] = 0;
                    cubes[x, y, z].GetComponent<MeshRenderer>().enabled = false;

                }
            }
        }
        currentY = 19;
        currentX = 4;
        currentZ = 4;
        SpawnTetromino();
        prevTime = Time.time;
        spawnTetromino = false;
        gameStarted = true;
        paused = false;
    }
    private void deployCubes()
    {
        float length = corners[1].transform.position.z - corners[0].transform.position.z;
        float width = corners[2].transform.position.x - corners[1].transform.position.x;
        float height = corners[4].transform.position.y - corners[0].transform.position.y;
        float cubeSize = length / 10f;

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    // Calculate the position of each cube
                    Vector3 cubePosition = corners[1].transform.position + new Vector3(cubeSize * (x + 0.5f), cubeSize * (y + 0.5f), -cubeSize * (z + 0.5f));

                    // Create a cube GameObject and set its position, size, and other properties
                    GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                    cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

                    cube.GetComponent<MeshRenderer>().enabled = false;
                    cube.transform.SetParent(grid.transform);
                    cube.GetComponent<MeshRenderer>().enabled = false;
                    // Save the cube in the cubes array
                    cubes[x, y, z] = cube;
                    cubeStatus[x, y, z] = 0;
                    cubes[x, y, z].GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        currentY = 19; // Set the initial y-coordinate of the tetromino
    }
    private void SpawnTetromino()
    {
        int randomIndex = Random.Range(0, tetrominoShapes.Length);
        shape = tetrominoShapes[randomIndex];
        if (CanSpawn(shape))
        {
        SpawnTetromino(shape);
    }
        else
        {
            spawnTetromino = false;
            gameStarted = false;
            //StartGame();
        }
    }
    private bool CanSpawn(TetrominoShape shape)
    {
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        // ...
                        int gridX = currentX + x;
                        int gridY = currentY - y;
                        int gridZ = currentZ + z;
                        if(cubeStatus[gridX, gridY, gridZ] == 1)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    private void SpawnTetromino(TetrominoShape shape)
    {
        // ...
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        // ...
                        int gridX = currentX + x;
                        int gridY = currentY - y;
                        int gridZ = currentZ + z;
                        cubeStatus[gridX, gridY, gridZ] = 1;
                        cubes[gridX, gridY, gridZ].GetComponent<MeshRenderer>().enabled = true;
                        cubes[gridX, gridY, gridZ].GetComponent<MeshRenderer>().material.color = shape.Color;
                    }
                }
            }
        }
    }
    private void MoveY()
    {
        // Check if the tetromino can move down
        if (CanMoveDown(shape))
        {
            // Clear the current tetromino position
            ClearTetromino(shape);
            // Move the tetromino down by decrementing the currentY variable
            currentY--;
            // Spawn the tetromino at the new position
            SpawnTetromino(shape);
        }
        else
        {
            // Check if any rows are filled and clear them if necessary
            ClearFilledRows();
            // Set spawnTetromino to true to spawn a new tetromino
            spawnTetromino = true;
        }
    }
    private void MoveX(Vector3 direction)
    {
        int newX = currentX + (int)direction.x;
        ClearTetromino(shape);
        if (CanMoveX(newX, shape))
        {
            currentX = newX;
        }
        SpawnTetromino(shape);
    }
    private void MoveZ(Vector3 direction)
    {
        int newZ = currentZ + (int)direction.z;
        ClearTetromino(shape);
        if (CanMoveZ(newZ, shape))
        {
            currentZ = newZ;
        }
        SpawnTetromino(shape);
    }
    private bool CanMoveX(int newX, TetrominoShape shape)
    {
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        int gridX = newX + x;
                        int gridY = currentY - y;
                        int gridZ = currentZ + z;

                        // Check if the cell is out of bounds or occupied by another cube
                        if (gridX < 0 || gridX >= gridSizeX || cubeStatus[gridX, gridY, gridZ] == 1)
                        {
                            return false; // Tetromino cannot move horizontally
                        }
                    }
                }
            }
        }

        return true; // Tetromino can move horizontally
    }

    private bool CanMoveZ(int newZ, TetrominoShape shape)
    {
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        int gridX = currentX + x;
                        int gridY = currentY - y;
                        int gridZ = newZ + z;

                        // Check if the cell is out of bounds or occupied by another cube
                        if (gridZ < 0 || gridZ >= gridSizeZ || cubeStatus[gridX, gridY, gridZ] == 1)
                        {
                            return false; // Tetromino cannot move in depth direction
                        }
                    }
                }
            }
        }

        return true; // Tetromino can move in depth direction
    }
    private bool CanMoveDown(TetrominoShape shape)
    {
        //Debug.Log("Befoe the loop X = " + shape.Shape.GetLength(0) + " Y = " + shape.Shape.GetLength(1) + " Z = " + shape.Shape.GetLength(2));
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = shape.Shape.GetLength(1) - 1; y >= 0; y--)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        int gridX = currentX + x;
                        int gridY = currentY - y - 1;
                        int gridZ = currentZ + z;
                        //Debug.Log("gridX = " + gridX + " gridY = " + gridY + " gridZ = " + gridZ);
                        //Debug.Log("X = " + x + " Y = " + y + " Z = " + z);
                        //Debug.Log(cubeStatus[gridX, gridY, gridZ]);
                        // Check if the cell is out of bounds or occupied by another cube
                        if (shape.Shape.GetLength(1) > 1 && y != shape.Shape.GetLength(1) - 1)
                        {
                            //Debug.Log("1a");
                            if (gridY >= 0 && shape.Shape[x, y + 1, z] != 1 && cubeStatus[gridX, gridY, gridZ] == 1)
                            {
                                //Debug.Log("1b");
                                return false; // Tetromino cannot move down
                            }
                        }
                        else if (gridY < 0 || (gridY >= 0 && cubeStatus[gridX, gridY, gridZ] == 1))
                        {
                            //Debug.Log("2");
                            return false; // Tetromino cannot move down
                        }
                    }
                }
            }
        }

        return true; // Tetromino can move down
    }
    public void RotateLeft()
    {
        int[,,] rotatedShape = new int[shape.Shape.GetLength(0), shape.Shape.GetLength(2), shape.Shape.GetLength(1)];
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    rotatedShape[x, z, shape.Shape.GetLength(1) - 1 - y] = shape.Shape[x, y, z];
                }
            }
        }
        ClearTetromino(shape);
        if (CanRotate(rotatedShape))
        {
            shape.Shape = rotatedShape;
        }
        SpawnTetromino(shape);
    }

    public void RotateRight()
    {
        int[,,] rotatedShape = new int[shape.Shape.GetLength(2), shape.Shape.GetLength(1), shape.Shape.GetLength(0)];
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    rotatedShape[shape.Shape.GetLength(2) - 1 - z, y, x] = shape.Shape[x, y, z];
                }
            }
        }
        ClearTetromino(shape);
        if (CanRotate(rotatedShape))
        {
            shape.Shape = rotatedShape;
        }
        SpawnTetromino(shape);
    }
    private bool CanRotate(int[,,] rotatedShape)
    {
        for (int x = 0; x < rotatedShape.GetLength(0); x++)
        {
            for (int y = 0; y < rotatedShape.GetLength(1); y++)
            {
                for (int z = 0; z < rotatedShape.GetLength(2); z++)
                {
                    if (rotatedShape[x, y, z] == 1)
                    {
                        int gridX = currentX + x;
                        int gridY = currentY - y;
                        int gridZ = currentZ + z;
                        if (gridX < 0 || gridX >= gridSizeX || gridY < 0 || gridY >= gridSizeY || gridZ < 0 || gridZ >= gridSizeZ || cubeStatus[gridX, gridY, gridZ] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }


    private void ClearTetromino(TetrominoShape shape)
    {
        for (int x = 0; x < shape.Shape.GetLength(0); x++)
        {
            for (int y = 0; y < shape.Shape.GetLength(1); y++)
            {
                for (int z = 0; z < shape.Shape.GetLength(2); z++)
                {
                    if (shape.Shape[x, y, z] == 1)
                    {
                        int gridX = currentX + x;
                        int gridY = currentY - y;
                        int gridZ = currentZ + z;

                        cubeStatus[gridX, gridY, gridZ] = 0;
                        cubes[gridX, gridY, gridZ].GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
    }

    private void ClearFilledRows()
    {
        for (int y = 0; y < 20; y++)
        {
            bool rowFilled = true;
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if (cubeStatus[x, y, z] == 0)
                    {
                        rowFilled = false;
                        break;
                    }
                }
                if (!rowFilled)
                    break;
            }

            if (rowFilled)
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
                y--;
            }
        }
    }

    private void ClearRow(int rowIndex)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                cubeStatus[x, rowIndex, z] = 0;
                cubes[x, rowIndex, z].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void ShiftRowsDown(int startRow)
    {
        for (int y = startRow; y < 20; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if (cubeStatus[x, y, z] == 1)
                    {
                        cubeStatus[x, y - 1, z] = 1;
                        cubeStatus[x, y, z] = 0;

                        cubes[x, y - 1, z].GetComponent<MeshRenderer>().enabled = true;
                        cubes[x, y, z].GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
    }
}