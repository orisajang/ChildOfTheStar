using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI red;
    [SerializeField] private TextMeshProUGUI blue;
    [SerializeField] private TextMeshProUGUI green;
    [SerializeField] private TextMeshProUGUI white;
    [SerializeField] private TextMeshProUGUI black;

    public void UpdateResource(TileColor color, int value)
    {
        switch (color)
        {
            case TileColor.Red:
                red.text = $"x{value}";
                break;
            case TileColor.Blue:
                blue.text = $"x{value}";
                break;
            case TileColor.Green:
                green.text = $"x{value}";
                break;
            case TileColor.White:
                white.text = $"x{value}";
                break;
            case TileColor.Black:
                black.text = $"x{value}";
                break;
        }
    }
    public void UpdateRed(int a)
    {
        red.text = $"x{a}";
    }
    public void UpdateBlue(int a)
    {
        blue.text = $"x{a}";
    }
    public void UpdateGreen(int a)
    {
        green.text = $"x{a}";
    }
    public void UpdateWhite(int a)
    {
        white.text = $"x{a}";
    }
    public void UpdateBlack(int a)
    {
        black.text = $"x{a}";
    }
}