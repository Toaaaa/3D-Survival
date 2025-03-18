using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : Singleton<BuildUI>
{
    public RadialMenu radialMenu;
    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        canvas = transform.GetComponent<Canvas>();
    }

    public void CanvasSort(bool isOpen)
    {
        canvas.sortingOrder = isOpen ? 101 : 0;
    }
}
