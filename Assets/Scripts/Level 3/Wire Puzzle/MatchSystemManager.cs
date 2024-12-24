using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchSystemManager : MonoBehaviour
{
    public List<Material> _materials; // List of all the colours for wires
    private List<MatchEntity> _matchEntities; // List of match entities
    private int _tMatchCount; // Target match count
    private int _cMatchCount = 0; // Current match count

    void Start()
    {
        _matchEntities = transform.GetComponentsInChildren<MatchEntity>().ToList();
        _tMatchCount = _matchEntities.Count;
        SetEntityColours();
        RandomisePairPlacement();
    }

    void SetEntityColours()
    {
        Shuffle(_materials);

        for (int i = 0; i < _matchEntities.Count; i++)
        {
            _matchEntities[i].SetMaterialToPairs(_materials[i]);
        }
    }

    void RandomisePairPlacement()
    {
        List<Vector3> movablePairPositions = new List<Vector3>();

        for (int i = 0; i < _matchEntities.Count; i++)
        {
            movablePairPositions.Add(_matchEntities[i].GetMovablePairPosition());
        }

        Shuffle(movablePairPositions);

        for (int i = 0; i < _matchEntities.Count; i++)
        {
            _matchEntities[i].SetMovablePairPosition(movablePairPositions[i]);
        }
    }

    public void NewMatchRecord(bool MatchConnected)
    {
        if (MatchConnected)
        {
            _cMatchCount++;
        }
        else
        {
            _cMatchCount--;
        }
        Debug.Log($"Currently, there are {_cMatchCount} matches");

        if (_cMatchCount == _tMatchCount)
        {
            Debug.Log("WELL DONE! ALL PAIRED");
        }
    }


    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T t = list[k];
            list[k] = list[n];
            list[n] = t;
        }
    }
}
