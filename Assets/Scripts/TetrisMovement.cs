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
            this.enabled = false;
           
            
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
           
        }
        return true;
    }
}
