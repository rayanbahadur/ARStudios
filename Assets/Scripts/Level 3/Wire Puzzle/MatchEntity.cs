using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchEntity : MonoBehaviour
{
    public MatchFeedback _feedback;
    public MovablePair _movablePair;
    public Renderer _fixedPairRenderer;
    public MatchSystemManager _systemManager;

    private bool _isMatched;

    public Vector3 GetMovablePairPosition()
    {
        return _movablePair.GetPosition();
    }

    public void SetMovablePairPosition(Vector3 NewMovablePairPosition)
    {
        _movablePair.SetInitialPosition(NewMovablePairPosition);
    }

    public void SetMaterialToPairs(Material PairMaterial)
    {
        _movablePair.GetComponent<Renderer>().material = PairMaterial;
        _fixedPairRenderer.material = PairMaterial;
    }

    public void PairObjectInteraction(bool IsEnter, MovablePair movable)
    {
        if (IsEnter && !_isMatched)
        {
            _isMatched = (movable == _movablePair);
            if (_isMatched)
            {
                _systemManager.NewMatchRecord(_isMatched);
                _feedback.ChangeMaterialWithMatch(_isMatched);
            }
        }
        else if (!IsEnter && _isMatched)
        {
            _isMatched = !(movable == _movablePair);
            if (!_isMatched)
            {
                _systemManager.NewMatchRecord(_isMatched);
                _feedback.ChangeMaterialWithMatch(_isMatched);
            }
        }
    }
}
