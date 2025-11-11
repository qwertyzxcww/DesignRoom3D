using UnityEngine;

public enum EditMode { None, Rotate, Delete, PaintWall, SelectObject }

public class RoomEditController : MonoBehaviour
{
    [Header("Raycast & Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance = 200f;

    [Header("Spawn")]
    [SerializeField] private float _spawnYOffset = 0.01f;

    private EditMode _mode = EditMode.None;
    private Material _pendingWallMaterial;
    private GameObject _pendingPrefab;

    private void Awake()
    {
        if (_camera == null) _camera = Camera.main;
    }

    public void SetMode(EditMode mode)
    {
        _mode = mode;
        if (mode != EditMode.PaintWall) _pendingWallMaterial = null;
        if (mode != EditMode.SelectObject) _pendingPrefab = null;
    }

    public void SetPaintWall(Material mat)
    {
        _pendingWallMaterial = mat;
        _mode = EditMode.PaintWall;
    }

    public void SetPendingPrefab(GameObject prefab)
    {
        _pendingPrefab = prefab;
        _mode = EditMode.SelectObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Raycast();
            if (hit.transform == null) return;

            switch (_mode)
            {
                case EditMode.Delete: TryDelete(hit.transform); break;
                case EditMode.PaintWall: TryPaintWall(hit.transform); break;
                case EditMode.SelectObject: TrySpawnOnFloorCell(hit); break;
                case EditMode.Rotate: TryRotate(hit.transform); break;
            }
        }
    }

    private RaycastHit Raycast()
    {
        if (_camera == null) return default;
        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, _rayDistance, ~0, QueryTriggerInteraction.Collide);
        return hit;
    }

    private Transform GetObjectRootIfValid(Transform t)
    {
        if (t == null) return null;
        if (t.CompareTag("Object")) return t;
        var p = t.GetComponentInParent<Transform>();
        return (p != null && p.CompareTag("Object")) ? p : null;
    }

    private void TryDelete(Transform t)
    {
        var obj = GetObjectRootIfValid(t);
        if (obj != null) Destroy(obj.gameObject);
    }

    private void TryPaintWall(Transform t)
    {
        if (_pendingWallMaterial == null) return;
        var target = t.CompareTag("Wall") ? t : t.GetComponentInParent<Transform>();
        if (target == null || !target.CompareTag("Wall")) return;
        if (target.TryGetComponent(out Renderer r)) r.material = _pendingWallMaterial;
    }

    private void TrySpawnOnFloorCell(RaycastHit hit)
    {
        if (_pendingPrefab == null) return;
        if (!hit.transform.CompareTag("Floor")) return;
        if (!hit.collider.TryGetComponent(out BoxCollider col)) return;

        var b = col.bounds;
        var pos = new Vector3(b.center.x, b.max.y + _spawnYOffset, b.center.z);
        var go = Instantiate(_pendingPrefab, pos, Quaternion.identity);
        go.tag = "Object";
    }

    private void TryRotate(Transform t)
    {
        var obj = GetObjectRootIfValid(t);
        if (obj != null) obj.Rotate(0f, 90f, 0f, Space.World);
    }
}
