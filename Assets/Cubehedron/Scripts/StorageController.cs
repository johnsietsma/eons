using UnityEngine;
using System;
using System.Collections;

public class StorageController : MonoBehaviour
{
    [SerializeField] private GameObject tray;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private Transform currentSlot;
    [SerializeField] private MoveToParams dockTweenParams;
    [SerializeField] private BasicParams turnTweenParams;

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

    public void OnGazeLeftEnter( GazeHit hit )
    {
        int index = Array.IndexOf( slots, currentSlot );
        index--;
        if ( index < 0 ) { index = slots.Length - 1; }
        currentSlot = slots[index];

        var p = turnTweenParams.ToHash();
        p["amount"] = new Vector3( 0, -0.25f, 0 );
        iTween.RotateBy( tray, p );
    }

    public void OnGazeRightEnter( GazeHit hit )
    {
        int index = Array.IndexOf( slots, currentSlot );
        currentSlot = slots[++index % slots.Length];

        var p = turnTweenParams.ToHash();
        p["amount"] = new Vector3( 0, 0.25f, 0 );
        iTween.RotateBy( tray, p );
    }

}
