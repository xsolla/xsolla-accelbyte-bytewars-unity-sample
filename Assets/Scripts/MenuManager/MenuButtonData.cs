using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MenuButtonData
{
    [SerializeField]
    //name of the button
    public string name;
    [SerializeField]
    //buttton's displayed text string
    public string label;
    //will call this callback upon clicked, can be null
    public UnityAction callback;
    //will go to this canvas upon clicked, can be null
    //public string canvasDestinationName;

}
