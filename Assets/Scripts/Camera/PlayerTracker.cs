using UnityEngine;
using System.Collections;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private PlayerTower _playerTower;
    [SerializeField] private float _movSpeed;
    [SerializeField] private Vector3 _offsetPosition;
    [SerializeField] private Vector3 _offsetRotation;

    private Vector3 _firstOffsetPosition;
    private Vector3 _targetPosition;
    private Coroutine _zoom;

    private void Awake()
    {
        transform.position = _playerTower.transform.position;
        transform.position += _offsetPosition;

        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.rotation = Quaternion.Euler(_offsetRotation);

        _firstOffsetPosition = _offsetPosition;
    }

    private void OnEnable()
    {
        _playerTower.HumanAdded += OnHumanAdded;
    }

    private void Update()
    {
        _targetPosition = GetTargetPosition();
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _movSpeed);
    }

    private void OnDisable()
    {
        _playerTower.HumanAdded -= OnHumanAdded;        
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 target = _playerTower.Humans.Count <= 4 ? _playerTower.transform.position : _playerTower.Humans[3].transform.position;
        return target + _offsetPosition;
    }

    private void OnHumanAdded(int count)
    {
        if (count <= 4)
        {
            if (_zoom != null) 
                StopCoroutine(_zoom);

            _zoom = StartCoroutine(Zoom(_firstOffsetPosition + ((Vector3.left + Vector3.back) / 2) * count));
        }
    }

    IEnumerator Zoom(Vector3 nextPosition)
    {
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            _offsetPosition = Vector3.Lerp(_offsetPosition, nextPosition, i);
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(_zoom);
        _zoom = null;
    }
}
