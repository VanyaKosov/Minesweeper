using UnityEngine;

public class Settings : MonoBehaviour
{
    public int Mines;

    public int Width;

    public int Height;

    public float MinePersentage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
