using UnityEngine;
using System;

[Serializable]
public class PaintingData
{
    #region JSON Variables
    public string name;
    public string artist;
    public string technique;

    public int date;
    public int sizeX;
    public int sizeY;

    public int price;
    public bool subscription;
    #endregion
}