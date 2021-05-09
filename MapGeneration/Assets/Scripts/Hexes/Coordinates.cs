using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coordinates : MonoBehaviour
{
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
