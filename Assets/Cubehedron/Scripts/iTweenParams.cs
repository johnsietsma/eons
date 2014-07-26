using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class BasicParams
{
    public bool isLocal;
    public float time;
    public iTween.EaseType easeType;

    public Hashtable ToHash()
    {
        return iTween.Hash(
                   "isLocal", isLocal,
                   "time", time,
                   "easeType", easeType
               );
    }
}

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


[Serializable]
public class RotateByParams
{
    public Vector3 rotation;
    public bool isLocal;
    public float time;
    public iTween.EaseType easeType;

    public Hashtable ToHash()
    {
        return iTween.Hash(
                   "rotation", rotation,
                   "isLocal", isLocal,
                   "time", time,
                   "easeType", easeType
               );
    }
}
