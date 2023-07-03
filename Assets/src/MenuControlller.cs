using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControlller : MonoBehaviour
{
    private Settings _settings;

    public Slider Slider;

    private void Start()
    {
        _settings = FindObjectOfType<Settings>();
        if (_settings != null)
        {
            Slider.value = _settings.MinePersentage;
            return;
        }
        var settingsObject = new GameObject();
        settingsObject.name = "Settings";
        _settings = (Settings)settingsObject.AddComponent(typeof(Settings));
    }

    private void CreateGame(int width, int height)
    {
        _settings.Width = width;
        _settings.Height = height;
        _settings.Mines = (int)(width * height * Slider.value);
        _settings.MinePersentage = Slider.value;

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
