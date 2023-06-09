using System;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    public event EventHandler<EventArgs> OnClick;

    public Image SpriteImage;

    public Sprite Closed;

    public Sprite Flag;

    public Sprite Mine;

    public Sprite MineExploded;

    public Sprite[] Values;

    public void Start()
    {
        SpriteImage.sprite = Closed;
    }

    public void ClickHandler()
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }


}