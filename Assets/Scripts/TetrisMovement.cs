using UnityEngine;

public class TetrisMovement : MonoBehaviour
{
    private float PreviousTime;
    public float FallTime = 0.8f;
    public static int Height = 20;
    public static int Width = 10;
    public Vector3 RotationBlock;
    private static Transform[,] grid = new Transform[Width, Height];

    void Start()
    {
        // Initialize PreviousTime to the current time
        PreviousTime = Time.time;
    }

    void Update()
    {
        // Handle left arrow key press
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveHorizontal(-1);
        }
        // Handle right arrow key press
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveHorizontal(1);
        }
        // Handle up arrow key press for rotation
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateBlock();
        }

        // Handle downward movement
        if (Time.time - PreviousTime > (Input.GetKey(KeyCode.DownArrow) ? FallTime / 10 : FallTime))
        {
            MoveVertical();
            PreviousTime = Time.time;
        }
    }

    void MoveHorizontal(int direction)
    {
        transform.position += new Vector3(direction, 0, 0);
        if (!IsValidGridPos())
        {
            transform.position -= new Vector3(direction, 0, 0);
        }
    }

    void MoveVertical()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!IsValidGridPos())
        {
            transform.position += new Vector3(0, 1, 0);
            AddToGrid();
            CheckTheLine();
            this.enabled = false;
            FindObjectOfType<SpawnArea>().NewTetromino();
        }
    }

    void CheckTheLine()
    {
        for (int i = Height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < Height; y++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    void RotateBlock()
    {
        transform.RotateAround(transform.TransformPoint(RotationBlock), Vector3.forward, 90);
        if (!IsValidGridPos())
        {
            transform.RotateAround(transform.TransformPoint(RotationBlock), Vector3.forward, -90);
        }
    }

    void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.transform.position;
            int roundedX = Mathf.RoundToInt(pos.x);
            int roundedY = Mathf.RoundToInt(pos.y);

            // Add the block to the grid
            grid[roundedX, roundedY] = child;
        }
    }

    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.transform.position;
            int roundedX = Mathf.RoundToInt(pos.x);
            int roundedY = Mathf.RoundToInt(pos.y);

            // Check if the position is within the grid bounds
            if (roundedX < 0 || roundedX >= Width || roundedY < 0 || roundedY >= Height)
            {
                return false;
            }

            // Check if the position is already occupied by another block
            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
}
