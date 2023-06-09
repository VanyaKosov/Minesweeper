using UnityEngine;

public class Settings : MonoBehaviour
{
    public int Mines;

    public int Width;

    public int Height;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
