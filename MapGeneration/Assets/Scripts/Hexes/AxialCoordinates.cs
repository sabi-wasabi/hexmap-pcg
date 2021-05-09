using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AxialCoordinates : MonoBehaviour
{
    // debug class for showing the axial coordinates of the hex on screen and saving them somewhere on the hex
    [SerializeField] private TextMeshProUGUI coords;
    private int q;
    private int r;

    public void SetCoords(int q, int r)
    {
        this.q = q;
        this.r = r;
        coords.text = $"({q},{r})";
    }
}
