using UnityEngine;
using PathCreation;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;

    [Header("Tower")]
    [SerializeField] private Tower _towerTemplate;
    [SerializeField] private int _humanTowerCount;

    [Header("ExtraJumpPoint")]
    [SerializeField] private ExtraJumpPoint _extraJumpPoint;
    [SerializeField] private float _offsetFromTorwer;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        float roadLenght = _pathCreator.path.length;
        float distanceBetweenTower = roadLenght / _humanTowerCount;

        float distancTrabelled = 0;
        Vector3 spawnTowerPoint;
        Vector3 spawnExtraJumpPoint;

        for (int i = 0; i < _humanTowerCount; i++)
        {
            distancTrabelled += distanceBetweenTower;

            spawnTowerPoint = _pathCreator.path.GetPointAtDistance(distancTrabelled, EndOfPathInstruction.Stop);
            Tower tower = Instantiate(_towerTemplate, spawnTowerPoint, Quaternion.identity);

            spawnExtraJumpPoint = _pathCreator.path.GetPointAtDistance(distancTrabelled - _offsetFromTorwer, EndOfPathInstruction.Stop);
            ExtraJumpPoint extraJumpPoint = Instantiate(_extraJumpPoint, spawnExtraJumpPoint, Quaternion.identity);
            
            extraJumpPoint.CountBuster = tower.TowerSize;
        }
    }
}
