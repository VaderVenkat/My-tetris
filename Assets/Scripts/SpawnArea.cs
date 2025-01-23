using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public GameObject[] TetrisBlocks;
    public Vector3 SpawnPosition; // Add a spawn position variable

    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino()
    {
        // Instantiate a new Tetromino at the specified spawn position
        Instantiate(TetrisBlocks[Random.Range(0, TetrisBlocks.Length)], SpawnPosition, Quaternion.identity);
    }
}
