using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileBase : MonoBehaviour
{
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
        switch (GameManager.Instance.GetPowerUp())
        {
            case GameManager.PowerUp.Basic:
                GridManager.Instance.PowerUpBasic(this);
                break;
            case GameManager.PowerUp.Vertical:
                GridManager.Instance.PowerUpVerticalLine(this);
                break;
            case GameManager.PowerUp.Horizontal:
                GridManager.Instance.PowerUpHorizontalLine(this);
                break;
            case GameManager.PowerUp.Cross:
                GridManager.Instance.PowerUpCross(this);
                break;
            case GameManager.PowerUp.Surround:
                GridManager.Instance.PowerUpSurrounding(this);
                break;
            case GameManager.PowerUp.Triple:
                GridManager.Instance.PowerUpTripleClick(this);
                break;
        }
    }
}
