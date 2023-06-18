using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlller : MonoBehaviour
{
    public Settings Settings;

    public void SmallField()
    {
        Settings.Width = 15;
        Settings.Height = 10;
        Settings.Mines = 20; // TODO remove

        SceneManager.LoadScene("Game");
    }
}
