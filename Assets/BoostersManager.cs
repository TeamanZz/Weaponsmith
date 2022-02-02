using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersManager : MonoBehaviour
{
    public static BoostersManager Instance;

    public List<BoostersItemPanel> panelItemsList = new List<BoostersItemPanel>();

    [ContextMenu("Awake")]
    public void Awake()
    {
        Instance = this;
    }
    public void MakeNextUnknownItemAsUnavailable()
    {
        //WaitingForDrawing
        var nextItem = panelItemsList.Find(x => x.currentState == PanelItemState.Unknown);
        if (nextItem != null)
            nextItem.ChangeState(PanelItemState.WaitingForDrawing);

    }

}
