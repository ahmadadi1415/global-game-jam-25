using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public static event Action OnUpdateGrid;
    public static event Action OnTurnEnd;
    public static event Action OnGameStateChecked;
    public static event Action<OnUsePowerUpEvent> OnUsePowerUp;
    public class OnUsePowerUpEvent
    {
        public PowerUpType powerUp;
    }

    public GridTileBase bubblePrefab; // Bubble prefab

    public List<GridTileBase> bubbleTiles = new List<GridTileBase>();

    [SerializeField] private Vector2 _gap = new Vector2(1f, 1f);
    [SerializeField] private int height = 5; // Number of rows
    [SerializeField] private int width = 5; // Number of columns
    [SerializeField] private Vector2 gridOffset = Vector2.zero;
    [SerializeField] private List<Vector2Int> bubbles = new();
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
        EventManager.Subscribe<OnLevelLoadedMessage>(OnLevelLoaded);
        EventManager.Subscribe<OnBubbleClickedMessage>(OnBubbleClicked);
        // GridTileBase.OnGridClick += GridTileBase_OnGridClick;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<OnLevelLoadedMessage>(OnLevelLoaded);
        EventManager.Unsubscribe<OnBubbleClickedMessage>(OnBubbleClicked);
        // GridTileBase.OnGridClick -= GridTileBase_OnGridClick;
    }

    private void OnLevelLoaded(OnLevelLoadedMessage message)
    {
        // DO: Update the grids
        GenerateGrid(message.Level);
    }

    private void Start()
    {
        // GenerateGrid(GameManager.Instance.sceneConfig);
    }

    private void OnBubbleClicked(OnBubbleClickedMessage message)
    {
        GridTileBase bubble = message.Bubble;
        switch (GameManager.Instance.GetPowerUp())
        {
            case PowerUpType.BASIC:
                PowerUpBasic(bubble);
                break;
            case PowerUpType.VERTICAL:
                PowerUpVerticalLine(bubble);
                break;
            case PowerUpType.HORIZONTAL:
                PowerUpHorizontalLine(bubble);
                break;
            case PowerUpType.CROSS:
                PowerUpCross(bubble);
                break;
            case PowerUpType.SURROUND:
                PowerUpSurrounding(bubble);
                break;
            case PowerUpType.TRIPLE:
                PowerUpTripleClick(bubble);
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

        if (_requiresGeneration) GenerateGrid(LevelManager.CurrentLevel);

        //     _cam.transform.position = Vector3.SmoothDamp(_cam.transform.position, _cameraPositionTarget, ref _moveVel, 0.8f);
        // _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _cameraSizeTarget, ref _cameraSizeVel, 0.8f);
    }

    private void InitBubbleGrid()
    {
        // DO: Reset tiles
        foreach (GridTileBase bubble in bubbleTiles)
        {
            bubble.gameObject.SetActive(false);
            // bubble.Destroy();
            Destroy(bubble.gameObject);
        }

        bubbleTiles.Clear();
    }

    void GenerateGrid(SceneConfigSO level)
    {
        InitBubbleGrid();

        List<Vector3Int> coordinates = new();
        width = level.Width;
        height = level.Height;
        bubbles = level.Bubbles;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                coordinates.Add(new Vector3Int(x, y, 0));
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
            bool isBubble = bubbles.Contains((Vector2Int)coord);

            Vector3 position = _grid.GetCellCenterWorld(coord);

            GridTileBase spawnedBubble = Instantiate(bubblePrefab, position, Quaternion.identity, transform);

            spawnedBubble.Init(coord, isBubble ? BubbleState.UNPOPPED : BubbleState.POPPED, gridOffset);

            bubbleTiles.Add(spawnedBubble);
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

    public void CheckGameState()
    {
        // DO: Notify game manager to win the game and continue to next level
        EventManager.Publish<OnBubblesCheckedMessage>(new() { IsAllPopped = IsAllBubblePopped() });
    }

    public bool IsAllBubblePopped()
    {
        bool isAllPopped = true;

        if (bubbleTiles.Count <= 0)
        {
            return isAllPopped;
        }

        foreach (GridTileBase bubble in bubbleTiles)
        {
            if (bubble.State == BubbleState.UNPOPPED)
            {
                isAllPopped = false;
                break;
            }
        }

        return isAllPopped;
    }

    private void SetCamera(Bounds bounds)
    {
        bounds.Expand(2);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        _cameraPositionTarget = bounds.center + Vector3.back;
        _cameraSizeTarget = Mathf.Max(horizontal, vertical) * 0.5f;
    }

    public void PowerUpBasic(GridTileBase bubbleToPop)
    {
        bubbleToPop.StartPop();

        // RemoveGrid(bubbleToPop);
        OnTurnEnd?.Invoke();
        CheckGameState();
    }

    public void PowerUpVerticalLine(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetTilePosition();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int y = 0; y < height; y++)
        {
            Vector3Int checkCoord = new Vector3Int(tileCoord.x, y, 0);

            GridTileBase bubbleToPop = bubbleTiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetTilePosition());
                return tileGridPosition == checkCoord;
            });

            bubbleToPop?.StartPop();
            // if (bubbleToPop != null)
            // {
            //     bubbleToPop.SetState(BubbleState.POPPING);
            //     // bubbleTiles.Remove(bubbleToPop);
            //     // RemoveGrid(bubbleToPop);
            // }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
        CheckGameState();
    }

    public void PowerUpHorizontalLine(GridTileBase tile)
    {

        Vector3 tilePosition = tile.GetTilePosition();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int x = 0; x < width; x++)
        {
            Vector3Int checkCoord = new Vector3Int(x, tileCoord.y, 0);

            GridTileBase bubbleToPop = bubbleTiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetTilePosition());
                return tileGridPosition == checkCoord;
            });

            bubbleToPop?.StartPop();
            // if (tileToRemove != null)
            // {
            //     bubbleTiles.Remove(tileToRemove);
            //     RemoveGrid(tileToRemove);
            // }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
        CheckGameState();
    }

    public void PowerUpTripleClick(GridTileBase bubbleToPop)
    {
        if (_tripleRemain > 0)
        {
            _tripleRemain--;
            bubbleToPop.StartPop();
            // bubbleTiles.Remove(tile);
            // RemoveGrid(tile);
            // OnUpdateGrid?.Invoke();
        }

        OnUpdateGrid?.Invoke();

        if (_tripleRemain <= 0)
        {
            OnTurnEnd?.Invoke();
            CheckGameState();
            _tripleRemain = 3;
        };
    }

    public void PowerUpCross(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetTilePosition();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int y = 0; y < height; y++)
        {
            Vector3Int checkCoord = new Vector3Int(tileCoord.x, y, 0);

            GridTileBase bubbleToPop = bubbleTiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetTilePosition());
                return tileGridPosition == checkCoord;
            });

            bubbleToPop?.StartPop();
            // if (bubbleToPop != null)
            // {
            //     bubbleTiles.Remove(bubbleToPop);
            //     RemoveGrid(bubbleToPop);
            // }
        }

        for (int x = 0; x < width; x++)
        {
            Vector3Int checkCoord = new Vector3Int(x, tileCoord.y, 0);

            GridTileBase bubbleToPop = bubbleTiles.Find(t =>
            {
                Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetTilePosition());
                return tileGridPosition == checkCoord;
            });

            bubbleToPop?.StartPop();

            // if (tileToRemove != null)
            // {
            //     bubbleTiles.Remove(tileToRemove);
            //     RemoveGrid(tileToRemove);
            // }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
        CheckGameState();
    }

    public void PowerUpSurrounding(GridTileBase tile)
    {
        Vector3 tilePosition = tile.GetTilePosition();
        Vector3Int tileCoord = Vector3Int.RoundToInt(tilePosition);

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                Vector3Int checkCoord = new Vector3Int(tileCoord.x + colOffset, tileCoord.y + rowOffset, 0);

                if (checkCoord.x >= 0 && checkCoord.x < width && checkCoord.y >= 0 && checkCoord.y < height)
                {
                    GridTileBase bubbleToPop = bubbleTiles.Find(t =>
                    {
                        Vector3Int tileGridPosition = Vector3Int.RoundToInt(t.GetTilePosition());
                        return tileGridPosition == checkCoord;
                    });

                    bubbleToPop?.StartPop();
                    // if (tileToRemove != null)
                    // {
                    //     bubbleTiles.Remove(tileToRemove);
                    //     RemoveGrid(tileToRemove);
                    // }
                }
            }
        }

        OnUpdateGrid?.Invoke();
        OnTurnEnd?.Invoke();
        CheckGameState();
    }
}