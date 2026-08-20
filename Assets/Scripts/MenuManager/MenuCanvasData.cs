using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MenuCanvasData
{
    [SerializeField]
    //canvas name
    public string name;
    //all the canvases button
    public  MenuButtonData[] buttons;
}
