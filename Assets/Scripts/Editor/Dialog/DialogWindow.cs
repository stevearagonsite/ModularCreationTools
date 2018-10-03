using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindow{

    public Rect myRect;
    public string title;
    public int order;
    public float duration;
    public List<DialogWindow> conections;

    public DialogWindow(float x, float y, float width, float height, string title)
    {
        myRect = new Rect(x, y, width, height);
        conections = new List<DialogWindow>();
        this.title = title;
    }
}
