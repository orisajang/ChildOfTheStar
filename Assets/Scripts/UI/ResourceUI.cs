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
                red.text = $"Red: {value}";
                break;
            case TileColor.Blue:
                blue.text = $"Blue: {value}";
                break;
            case TileColor.Green:
                green.text = $"Green: {value}";
                break;
            case TileColor.White:
                white.text = $"White: {value}";
                break;
            case TileColor.Black:
                black.text = $"Black: {value}";
                break;
        }
    }
    public void UpdateRed(int a)
    {
        red.text = $"Red: {a}";
    }
    public void UpdateBlue(int a)
    {
        blue.text = $"Blue: {a}";
    }
    public void UpdateGreen(int a)
    {
        green.text = $"Green: {a}";
    }
    public void UpdateWhite(int a)
    {
        white.text = $"White: {a}";
    }
    public void UpdateBlack(int a)
    {
        black.text = $"Black: {a}";
    }
}