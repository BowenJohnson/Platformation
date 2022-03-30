using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this object will move an object transform between 2 or more given transform positions
public class HoveringObject : MonoBehaviour
{
    // the object to hover and set speed
    [Header("Hovering Object Control")]
    [SerializeField] private Transform _object;
    [SerializeField] private float _hoverSpeed;

    // if random speed is desired at instantiation
    [Header("Random Speed Control")]
    [SerializeField] bool _setRandomSpeed;
    [SerializeField] private float _randSpeedMin;
    [SerializeField] private float _randSpeedMax;

    // min and max points that it will hover between
    [Header("Hover Min & Max Points")]
    [SerializeField] private Transform _hoverMax;
    [SerializeField] private Transform _hoverMin;

    [Header("Array For Series Point Movement")]
    [SerializeField] private bool _seriesMovement;
    [SerializeField] private bool _randomMovement;
    [SerializeField] private Transform[] _pointSeries;
    private int _seriesSize;
    private int _seriesCount;
    private int seriesNext;

    // vars for randomization
    private int _seriesCurr;
    private int _seriesTemp;
    private int _randNum;

    private Vector3 _nextPosition;

    private void Start()
    {
        // if random speed bool activaed set random speed instead
        if (_setRandomSpeed == true)
        {
            SetRandomSpeed();
        }

        // if series movement bool is activated set next position to first in array
        if (_seriesMovement)
        {
            if (!_randomMovement)
            {
                _nextPosition = _pointSeries[0].position;
                _seriesCount = 0;
            }
            else
            {
                SetRandomStart();
            }
        }
        else
        {
            _nextPosition = _hoverMax.position;
        }

        SetSeriesSize();
    }

    // Update is called once per frame
    private void Update()
    {
        Hover();
    }

    private void Hover()
    {
        _object.position = Vector3.MoveTowards(_object.position, _nextPosition, _hoverSpeed * Time.deltaTime);

        // if distance to next position is 0.1 or less change direction
        if (Vector3.Distance(_object.position, _nextPosition) <= 0.1)
        {
            // if series movement is on go to next in series
            if (_seriesMovement)
            {
                // if random is on get random next position
                if (_randomMovement)
                {
                    RandomNextPosition();
                }
                // else go to next in sequence
                else
                {
                    SeriesChangeDirection();
                }
                
            }
            // else go to next min or max preset
            else
            {
                ChangeDirection();
            }
        }
    }

    // change direction based on what the next position is currently
    private void ChangeDirection()
    {
        // if next position does not equal current position then change to other position
        _nextPosition = _nextPosition != _hoverMax.position ? _hoverMax.position : _hoverMin.position;
    }

    private void SetRandomSpeed()
    {
        _hoverSpeed = Random.Range(_randSpeedMin, _randSpeedMax);
    }

    // if there is anything in the array set _seriesSize to its
    // length and set _series count to length for future mod rotation;
    private void SetSeriesSize()
    {
        if (_pointSeries != null)
        {
            _seriesSize = _pointSeries.Length;
        }
    }

    private void SeriesChangeDirection()
    {
        // increase series count
        _seriesCount++;

        // reset counter if count is > series size
        if (_seriesCount > _seriesSize)
        {
            _seriesCount = 0;
        }

        // mod to get next position
        seriesNext = _seriesCount % _seriesSize;

        // set next position to array count
        _nextPosition = _pointSeries[seriesNext].position;
    }

    // get a random number within array bounds and set next position to that
    // set current and temp to the same random number for future calculations
    private void SetRandomStart()
    {
        _randNum = Random.Range(0, _pointSeries.Length);

        _nextPosition = _pointSeries[_randNum].position;
        _seriesCurr = _randNum;
        _seriesTemp = _randNum;
    }

    // get a random number for the next array position
    private void RandomNextPosition()
    {
        // get random number within array bounds
        _randNum = Random.Range(0, _pointSeries.Length);

        // set temp to number
        _seriesTemp = _randNum;

        // if temp number is same as current then get random until
        // a different number pops up
        if (_seriesCurr == _seriesTemp)
        {
            while (_seriesCurr == _seriesTemp)
            {
                _randNum = Random.Range(0, _pointSeries.Length);

                _seriesTemp = _randNum;
            }
        }

        // set current to now different temp number and set next position
        _seriesCurr = _seriesTemp;
        _nextPosition = _pointSeries[_seriesCurr].position;
    }
}
