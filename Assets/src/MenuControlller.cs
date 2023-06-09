using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlller : MonoBehaviour
{
    public Settings Settings;

    public void SmallField()
    {
        Settings.Width = 7;
        Settings.Height = 5;
        Settings.Mines = 6; // TODO remove

        SceneManager.LoadScene("Game");
    }
}
