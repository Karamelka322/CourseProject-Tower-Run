using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    [SerializeField] private PlayerTower _playerTower;
    [SerializeField] private Jumper _jumperTower;
    [SerializeField] private PathFollower _pathFollowerTower;

    private void OnEnable()
    {
        _playerTower.PlayerDead += OnPlayerDead;
    }

    private void OnDisable()
    {
        _playerTower.PlayerDead -= OnPlayerDead;        
    }

    private void OnPlayerDead()
    {
        _playerTower.enabled = false;
        _jumperTower.enabled = false;
        _pathFollowerTower.enabled = false;
    }
}
