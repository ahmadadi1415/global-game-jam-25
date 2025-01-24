using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileBase : MonoBehaviour, IDisposable
{
    public static event Action<OnGridClickEvent> OnGridClick;
    public class OnGridClickEvent
    {
        public GridTileBase GridTileBase;
    }
    
    private Vector3 _position;
    public void Init(Vector3 t)
    {
        transform.position = t;
        _position = t;
    }

    public Vector3 GetPositionTile()
    {
        return _position;
    }

    public GridTileBase GetTile()
    {
        return this;
    }

    public void Destroy()
    {
        // Debug.Log("Destryo" + GetPositionTile().ToString());
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        OnGridClick?.Invoke(new OnGridClickEvent { GridTileBase = this });
    }

    public void Dispose()
    {
        OnGridClick = null;
    }
}
