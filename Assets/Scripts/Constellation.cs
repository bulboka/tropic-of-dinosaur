using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    [SerializeField] private Transform _starLocatorsContainer;
    [SerializeField] private Transform _mainStarLocatorsContainer;

    private int _nextStarLocatorIndex;
    private int _nextMainStarLocatorIndex;
    private List<MainStarLocator> _mainStarLocators = new();

    private void Start()
    {
        _mainStarLocators = _mainStarLocatorsContainer.GetComponentsInChildren<MainStarLocator>().ToList();

        foreach (var starLocator in _mainStarLocators)
        {
            foreach (var connectedLocator in starLocator.ConnectedLocators)
            {
                if (!connectedLocator.ConnectedLocators.Contains(starLocator))
                {
                    connectedLocator.ConnectedLocators.Add(starLocator);
                }
            }
        }
    }

    public Transform GetNextStarLocator()
    {
        if (_nextStarLocatorIndex >= _starLocatorsContainer.childCount)
        {
            return null;
        }

        return _starLocatorsContainer.GetChild(_nextStarLocatorIndex++);
    }

    public MainStarLocator GetNextMainStarLocator()
    {
        if (_nextMainStarLocatorIndex >= _mainStarLocators.Count)
        {
            return null;
        }

        return _mainStarLocators[_nextMainStarLocatorIndex++];
    }
}