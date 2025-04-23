using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ruby : StickyObject
{
    [SerializeField] private List<string> _poems;
    [SerializeField] private float _unitDuration;
    [SerializeField] private List<GameObject> _glow;
    [SerializeField] private List<MorseCode> _codes;

    private bool _isMorseActive;
    private string _poem;
    private List<MorseBit> _poemMorse;
    private int _currentBitIndex;
    private float _nextBitTime;

    protected override void OnStuck()
    {
        base.OnStuck();

        _isMorseActive = true;
        _poem = _poems[Random.Range(0, _poems.Count)];
        _poem = _poem.Replace("\n", " ");
        _poem = _poem.Replace(".", "");
        _poem = _poem.Replace(",", "");
        _poem = _poem.Replace("-", "");
        _poem = _poem.Replace("!", "");
        _poem = _poem.Replace("?", "");

        _poemMorse = new List<MorseBit>();
        _poemMorse.Add(new MorseBit(false, 20));

        for (var i = 0; i < _poem.Length; i++)
        {
            if (_poem[i] == ' ')
            {
                _poemMorse.Add(new MorseBit(false, 7));
                continue;
            }

            var currentChar = _poem[i].ToString().ToLower().ToCharArray().First();
            var code = _codes.FirstOrDefault(item => item.Char == currentChar);

            if (code == null)
            {
                continue;
            }

            for (var j = 0; j < code.Code.Length; j++)
            {
                _poemMorse.Add(new MorseBit(true, code.Code[j] == '1' ? 1 : 3));

                if (j < code.Code.Length - 1)
                {
                    _poemMorse.Add(new MorseBit(false, 1));
                }
            }

            if (i < _poem.Length - 1 && _poem[i + 1] != ' ')
            {
                _poemMorse.Add(new MorseBit(false, 3));
            }
        }
    }

    private void Update()
    {
        if (!_isMorseActive || Time.time < _nextBitTime)
        {
            return;
        }

        var currentBit = _poemMorse[_currentBitIndex];

        _nextBitTime = Time.time + currentBit.DurationUnits * _unitDuration;

        foreach (var glowObject in _glow)
        {
            glowObject.SetActive(currentBit.IsActive);
        }

        _currentBitIndex++;

        if (_currentBitIndex == _poemMorse.Count)
        {
            _currentBitIndex = 0;
        }
    }
}

public struct MorseBit
{
    public bool IsActive;
    public int DurationUnits;

    public MorseBit(bool isActive, int durationUnits)
    {
        IsActive = isActive;
        DurationUnits = durationUnits;
    }
}

[Serializable]
public class MorseCode
{
    public char Char;
    public string Code;
}