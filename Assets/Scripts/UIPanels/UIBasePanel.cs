using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBasePanel : MonoBehaviour
{
    public abstract void HidePanel();

    public abstract void ShowPanel();

    public virtual void HidePanelBehaviour()
    {
    }

    public virtual void ShowPanelBehaviour()
    {
    }
}
