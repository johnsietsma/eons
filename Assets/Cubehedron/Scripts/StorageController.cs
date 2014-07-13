using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class MoveToParams
{
    public Vector3 position;
    public bool isLocal;
    public float time;
    public iTween.EaseType easeType;

    public Hashtable ToHash()
    {
        return iTween.Hash(
                   "position", position,
                   "isLocal", isLocal,
                   "time", time,
                   "easeType", easeType
               );
    }
}

public class StorageController : MonoBehaviour
{

    [SerializeField] private Transform slotsParent;
    [SerializeField] private Transform currentSlot;
    [SerializeField] private MoveToParams dockTweenParams;

    private Transform[] slots;

    void Awake()
    {
        slots = new Transform[slotsParent.childCount];
        for ( int i = 0; i < slots.Length; i++ ) {
            slots[i] = slotsParent.GetChild( i );
        }
        D.Assert( Array.IndexOf( slots, currentSlot ) >= 0, "currentSlot is not a child of slotsParent" );
    }

    public void Store( GameObject prop )
    {
        prop.transform.parent = currentSlot;
        iTween.MoveTo( prop, dockTweenParams.ToHash() );
    }

    public void TurnLeft()
    {
        int index = Array.IndexOf( slots, currentSlot );
        index--;
        if ( index < 0 ) { index = slots.Length - 1; }
        currentSlot = slots[index];
    }

    public void TurnRight()
    {
        int index = Array.IndexOf( slots, currentSlot );
        currentSlot = slots[++index % slots.Length];
    }

}
