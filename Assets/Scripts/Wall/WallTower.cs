using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WallTower : MonoBehaviour
{
    [SerializeField] private int _lenghtTower = 1;
    [Space]
    [SerializeField] private Wall _firstWall;
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private float _offsetY;

    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();

        if (_lenghtTower > 1)
        {
            SpawnWalls(_lenghtTower - 1);
            UpdatedSizeCollider(_lenghtTower);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Human human))
        {
            human.GetComponentInParent<PlayerTower>().HumanDelete(human);
        }
    }

    private void SpawnWalls(int lenght)
    {
        Vector3 spawnposition = new Vector3(_firstWall.transform.localPosition.x, -_firstWall.transform.localPosition.y, _firstWall.transform.localPosition.z);

        for (int i = 0; i < lenght; i++)
        {
            Wall wall = Instantiate(_wallTemplate, transform);
            wall.transform.localPosition = spawnposition;

            spawnposition = new Vector3(spawnposition.x, spawnposition.y + _offsetY, spawnposition.z);
        }
    }

    private void UpdatedSizeCollider(int lenght)
    {
        _collider.center = new Vector3(0, (_offsetY / 2) * (lenght - 1), 0);
        _collider.size = new Vector3(_collider.size.x, _offsetY * lenght, _collider.size.z);
    }
}
