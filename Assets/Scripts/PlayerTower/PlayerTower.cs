using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;
    [SerializeField] private Transform _distanceChecker;
    [SerializeField] private float _fixationMaxDistance;
    [SerializeField] private BoxCollider _checkCollider;

    private readonly float displaceScale = 1.5f;
    private List<Human> _humans;
    
    public event UnityAction<int> HumanAdded;
    public event UnityAction PlayerDead;

    public List<Human> Humans => _humans;

    private void Start()
    {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        _humans[0].Run();

        HumanAdded?.Invoke(_humans.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Human human))
        {
            HumanAdd(human);
        }
    }

    private void HumanAdd(Human human)
    {
        Tower collisionTower = human.transform.GetComponentInParent<Tower>();

        if (collisionTower == null)
            return;

        List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChecker, _fixationMaxDistance);

        if (collectedHumans != null)
        {
            _humans[0].StopRun();
            _humans[_humans.Count - 1].StopWaving();

            for (int i = collectedHumans.Count - 1; i >= 0; i--)
            {
                Human insertHuman = collectedHumans[i];
                InSertHuman(insertHuman);
                DisplaceCheckers(insertHuman);
            }

            EquateHumansPosition(_humans);
            HumanAdded?.Invoke(_humans.Count);

            _humans[_humans.Count - 1].Waving();
            _humans[0].Run();
        }

        collisionTower.Break();
    }

    public void HumanDelete(Human human)
    {
        if (_humans.Count == 1)
        {
            _humans[0].StopRun();
            _humans[0].Dead();
            PlayerDead?.Invoke();

            return;
        }

        if(human != null)
        {
            _humans[0].StopRun();
            _humans[_humans.Count - 1].StopWaving();

            PullOutHuman(human);

            if(_humans.Count != 1)
                _humans[_humans.Count - 1].Waving();

            _humans[0].Run();

            AttractCheckers(human);
            HumanAdded?.Invoke(_humans.Count);
        }
    }

    private void InSertHuman(Human collectedHuman)
    {
        _humans.Insert(0, collectedHuman);
        SetHumanPosition(collectedHuman);
    }

    private void PullOutHuman(Human collectedHuman)
    {
        _humans.Remove(collectedHuman);

        collectedHuman.Dead();
        collectedHuman.transform.parent = null;

        float delayDead = 2f;
        Destroy(collectedHuman, delayDead);
    }

    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);
        human.transform.localRotation = Quaternion.identity;
    }

    private void EquateHumansPosition(List<Human> humans)
    {
        for (int i = 1; i < humans.Count; i++)
        {
            humans[i].transform.position = humans[i - 1].FixationPoint.position;
            humans[i].transform.localPosition = new Vector3(0, humans[i].transform.localPosition.y, 0);
        }
    }

    private void DisplaceCheckers(Human human)
    {
        Vector3 distanceCheckerNewPosition = _distanceChecker.position;
        distanceCheckerNewPosition.y -= human.transform.localScale.y * displaceScale;
        _distanceChecker.position = distanceCheckerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;
    }    
    
    private void AttractCheckers(Human human)
    {
        Vector3 distanceCheckerNewPosition = _distanceChecker.position;
        distanceCheckerNewPosition.y += human.transform.localScale.y * displaceScale;
        _distanceChecker.position = distanceCheckerNewPosition;
        _checkCollider.center = _distanceChecker.localPosition;
    }
}
