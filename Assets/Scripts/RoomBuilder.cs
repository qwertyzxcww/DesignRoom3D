using TMPro;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _platformPrefab;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _projectNameText;

    private int _widthTiles;
    private int _heightTiles;
    private Transform _wallsRoot;
    private Transform _floorRoot;

    private void Start()
    {
        InitRoomData();
        if (_widthTiles < 1 || _heightTiles < 1) return;

        GenerateFloor();
        GenerateWalls();
        GeneratePlatform();
    }

    private void InitRoomData()
    {
        var settings = RoomSettings.Instance;

        if (_projectNameText != null)
            _projectNameText.text = settings.ProjectName;

        _widthTiles = Mathf.Max(1, settings.RoomWidth);
        _heightTiles = Mathf.Max(1, settings.RoomHeight);

        _floorRoot = new GameObject("FloorRoot").transform;
        _floorRoot.SetParent(transform, false);

        _wallsRoot = new GameObject("WallsRoot").transform;
        _wallsRoot.SetParent(transform, false);
    }

    private void GenerateFloor()
    {
        for (int x = 0; x < _widthTiles; x++)
        {
            for (int z = 0; z < _heightTiles; z++)
            {
                var pos = new Vector3(x, 0f, z);
                var tile = Instantiate(_floorPrefab, pos, Quaternion.identity, _floorRoot);
                tile.tag = "Floor";
            }
        }
    }

    private void GenerateWalls()
    {
        // Северная стена (вперёд)
        for (int x = 0; x < _widthTiles; x++)
        {
            var wall = Instantiate(_wallPrefab, new Vector3(x, 1.5f, -1f), Quaternion.identity, _wallsRoot);
            wall.tag = "Wall";
        }

        // Западная стена (левая) — поворачиваем на 90 градусов
        for (int z = 0; z < _heightTiles; z++)
        {
            var wall = Instantiate(_wallPrefab, new Vector3(-1f, 1.5f, z), Quaternion.Euler(0, 90, 0), _wallsRoot);
            wall.tag = "Wall";
        }
    }


    private void GeneratePlatform()
    {
        var centerX = (_widthTiles - 1) * 0.5f;
        var centerZ = (_heightTiles - 1) * 0.5f;
        var platform = Instantiate(_platformPrefab, new Vector3(centerX, -1f, centerZ), Quaternion.identity, transform);
        platform.transform.localScale = new Vector3(Mathf.Max(12f, _widthTiles + 3f), 1f, Mathf.Max(12f, _heightTiles + 3f));
    }
}
