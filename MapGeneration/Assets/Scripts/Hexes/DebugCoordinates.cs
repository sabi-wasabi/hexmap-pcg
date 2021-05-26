using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCoordinates : MonoBehaviour
{
    // debug class for showing the axial coordinates of the hex on screen and saving them somewhere on the hex
    [SerializeField] private TextMeshProUGUI coordText;

    public void SetCoordinates(Vector2Int coords)
    {
        coordText.text = $"({coords.x},{coords.y})";
    }
} 
