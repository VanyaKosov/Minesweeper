using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControlller : MonoBehaviour
{
    public Settings Settings;

    public Slider Slider;

    private void CreateGame(int width, int height)
    {
        Settings.Width = width;
        Settings.Height = height;
        Settings.Mines = (int)(width * height * Slider.value);

        SceneManager.LoadScene("Game");
    }

    public void SmallField()
    {
        CreateGame(15, 10);
    }

    public void MediumField()
    {
        CreateGame(25, 20);
    }

    public void BigField()
    {
        CreateGame(35, 25);
    }
}
