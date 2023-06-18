using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellController : MonoBehaviour, IPointerClickHandler
{
    public event EventHandler<EventArgs> OnToggleFlagClick;

    public event EventHandler<EventArgs> OnOpenCellClick;

    public Image SpriteImage;

    public Sprite Closed;

    public Sprite Flag;

    public Sprite Mine;

    public Sprite MineExploded;

    public Sprite[] Values;

    public void Start()
    {
        ChangeSprite(Closed);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnToggleFlagClick?.Invoke(this, EventArgs.Empty);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnOpenCellClick?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        SpriteImage.sprite = sprite;
    }

    internal void PlaceFlag()
    {
        ChangeSprite(Flag);
    }

    internal void RemoveFlag()
    {
        ChangeSprite(Closed);
    }

    internal void OpenCell(int value)
    {
        ChangeSprite(Values[value]);
    }
}