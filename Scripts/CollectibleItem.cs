using System;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public bool IsCollected { get; protected set; }
    public virtual void DeliverInstant(int i_Amount,eDropStyle i_DropStyle)
    {

    }
    public virtual void Drop(int i_Amount, eDropStyle i_DropStyle,Action i_OnReachToDestination)
    {

    }

    public virtual void Drop(Vector3 i_StartPosition, int i_Amount, eDropStyle i_DropStyle)
    {

    }

}
