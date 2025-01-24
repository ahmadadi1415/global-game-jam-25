using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public static event Action OnUpdateGrid;
    public static event Action OnTurnEnd;
    public static event Action<OnUsePowerUpEvent> OnUsePowerUp;
    public class OnUsePowerUpEvent
    {
        public PowerUp powerUp;
    }

    public GridTileBase bubblePrefab; // Bubble prefab

    public List<GridTileBase> tiles = new List<GridTileBase>();

    [SerializeField] private Vector2 _gap = new Vector2(1f, 1f);
    [SerializeField] private int rows = 5; // Number of rows
    [SerializeField] private int columns = 5; // Number of columns
    [SerializeField] private List<Vector2Int> holes = new();
    [SerializeField] private Vector2 _cellSize;

    private bool _requiresGeneration = true;

    private int _tripleRemain = 3;

    private Vector3 _cameraPositionTarget;
    private float _cameraSizeTarget;
    private Vector3 _moveVel;
    private float _cameraSizeVel;

    private Vector2 _currentGap;
    private Vector2 _gapVel;

    private Camera _cam;
    private Grid _grid;

    private void Awake()
    {
        _cam = Camera.main;
        _grid = GetComponent<Grid>();
        _currentGap = _gap;

        _grid.cellSize.Set(_cellSize.x, _cellSize.y, 0);
        _grid.cellGap = _currentGap;
        _grid.cellLayout = GridLayout.CellLayout.Rectangle;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnValidate()
    {
        _requiresGeneration = true;
    }

    private void OnEnable()
    {
        GridTileBase.OnGridClick += GridTileBase_OnGridClick;
    }

    private void OnDisable()
    {
        GridTileBase.OnGridClick -= GridTileBase_OnGridClick;
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GridTileBase_OnGridClick(GridTileBase.OnGridClickEvent obj)
    {
        switch (GameManager.Instance.GetPowerUp())
        {
            case PowerUp.Basic:
                PowerUpBasic(obj.GridTileBase);
                break;
            case PowerUp.Vertical:
                PowerUpVerticalLine(obj.GridTileBase);
                break;
            case PowerUp.Horizontal:
                PowerUpHorizontalLine(obj.GridTileBase);
                break;
            case PowerUp.Cross:
                PowerUpCross(obj.GridTileBase);
                break;
            case PowerUp.Surround:
                PowerUpSurrounding(obj.GridTileBase);
                break;
            case PowerUp.Triple:
                PowerUpTripleClick(obj.GridTileBase);
                break;
        }
    }

    private void LateUpdate()
    {
        if (Vector2.Distance(_currentGap, _gap) > 0.01f)
        {
            _currentGap = Vector2.SmoothDamp(_currentGap, _gap, ref _gapVel, 0.1f);
            _requiresGeneration = true;
        }

        if (_requiresGeneration) GenerateGrid();

        //     _cam.transform.position = Vector3.SmoothDamp(_cam.transform.position, _cameraPositionTarget, ref _moveVel, 0.8f);
        // _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _cameraSizeTarget, ref _cameraSizeVel, 0.8f);
    }

    void GenerateGrid()
    {
        var coordinates = new List<Vector3Int>();
        rows = GameManager.Instance.GetSceneConfigSO().Width;
        columns = GameManager.Instance.GetSceneConfigSO().Height;
        holes = GameManager.Instance.GetSceneConfigSO().Holes;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                coordinates.Add(new Vector3Int(col, row, 0));
            }
        }

        Bounds bound = new();

        if (coordinates.Count > 0)
        {
            Vector3 firstPosition = _grid.GetCellCenterWorld(coordinates[0]);
            bound = new Bounds(firstPosition, Vector3.zero);
        }

        foreach (Vector3Int coord in coordinates)
        {
            bool isHole = holes.Contains((Vector2Int)coord);
            if (isHole) continue;

            Vector3 position = _grid.GetCellCenterWorld(coord);
            GridTileBase spawned = Instantiate(bubblePrefab, position, Quaternion.identity, transform);
            spawned.Init(coord);
            tiles.Add(spawned);
            bound.Encapsulate(position);
        }

        // SetCamera(bound);

        _requiresGeneration = false;
    }

    public void RemoveGrid(GridTileBase gridTileBase)
    {
        OnUpdateGrid?.Invoke();
        gridTileBase.Destroy();
    }

    private void SetCamera(Bounds bounds)
    {
        bounds.Expand(2);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        _cameraPositionTarget = bounds.center + Vector3.back;
        _cameraSizeTarget = Mathf.Max(horizontal, vertical) * 0.5f;
    }

    public void PowerUpBasic(GridTileBase tile)
    {
        RemoveGrid(tile);
        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
    }

    public void PowerUpVerticalLine(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetPositionTile();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int y = 0; y < rows; y++)
        {
            Vector3Int checkCoord = new Vector3Int(tileCoord.x, y, 0);

            GridTileBase tileToRemove = tiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetPositionTile());
                return tileGridPosition == checkCoord;
            });

            if (tileToRemove != null)
            {
                tiles.Remove(tileToRemove);
                RemoveGrid(tileToRemove);
            }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
    }

    public void PowerUpHorizontalLine(GridTileBase tile)
    {

        Vector3 tilePosition = tile.GetPositionTile();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int x = 0; x < columns; x++)
        {
            Vector3Int checkCoord = new Vector3Int(x, tileCoord.y, 0);

            GridTileBase tileToRemove = tiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetPositionTile());
                return tileGridPosition == checkCoord;
            });

            if (tileToRemove != null)
            {
                tiles.Remove(tileToRemove);
                RemoveGrid(tileToRemove);
            }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
    }

    public void PowerUpTripleClick(GridTileBase tile)
    {
        if (_tripleRemain > 0)
        {
            _tripleRemain--;
            RemoveGrid(tile);
            OnUpdateGrid?.Invoke();
        }

        OnUpdateGrid?.Invoke();

        if (_tripleRemain <= 0) OnTurnEnd?.Invoke();
    }

    public void PowerUpCross(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetPositionTile();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int y = 0; y < rows; y++)
        {
            Vector3Int checkCoord = new Vector3Int(tileCoord.x, y, 0);

            GridTileBase tileToRemove = tiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetPositionTile());
                return tileGridPosition == checkCoord;
            });

            if (tileToRemove != null)
            {
                tiles.Remove(tileToRemove);
                RemoveGrid(tileToRemove);
            }
        }

        for (int x = 0; x < columns; x++)
        {
            Vector3Int checkCoord = new Vector3Int(x, tileCoord.y, 0);

            GridTileBase tileToRemove = tiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetPositionTile());
                return tileGridPosition == checkCoord;
            });

            if (tileToRemove != null)
            {
                tiles.Remove(tileToRemove);
                RemoveGrid(tileToRemove);
            }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
    }

    public void PowerUpSurrounding(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetPositionTile();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                Vector3Int checkCoord = new Vector3Int(tileCoord.x + colOffset, tileCoord.y + rowOffset, 0);

                if (checkCoord.x >= 0 && checkCoord.x < columns && checkCoord.y >= 0 && checkCoord.y < rows)
                {
                    GridTileBase tileToRemove = tiles.Find(t =>
                    {
                        Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetPositionTile());
                        return tileGridPosition == checkCoord;
                    });

                    if (tileToRemove != null)
                    {
                        tiles.Remove(tileToRemove);
                        RemoveGrid(tileToRemove);
                    }
                }
            }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
    }
}