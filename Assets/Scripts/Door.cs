using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _step;
    
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Quaternion _startRotation;
    private Quaternion _endRotation;

    private bool _isClosed;
    private bool _isBusy;

    private Vector3 anchor;
    private Vector3 axis;
    
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition;
    
        _endPosition.x += transform.localScale.z / 2 - transform.localScale.x / 2;
        _endPosition.z += transform.localScale.x / 2 - transform.localScale.z / 2;
        
        _startRotation = transform.rotation;
        _endRotation = Quaternion.Euler(
            _startRotation.eulerAngles.x,
            _startRotation.eulerAngles.y - 90f,
            _startRotation.eulerAngles.z);
    }

    public IEnumerator Interact()
    {
        if (_isBusy)
            yield break;
        
        _isBusy = true;

        var position = Vector3.zero;
        var rotation = Quaternion.identity;

        if (_isClosed == false)
        {
            for (float i = 0; i <= 1; i += _step * Time.deltaTime)
            {
                position = Vector3.Slerp(_startPosition, _endPosition, i);
                rotation = Quaternion.Slerp(_startRotation, _endRotation, i);
                
                transform.position = position;
                transform.rotation = rotation;
                
                yield return null;
            }

            position = _endPosition;
            rotation = _endRotation;
            transform.SetPositionAndRotation(position, rotation);
        }
        else
        {
            for (float i = 1; i >= 0; i -= _step * Time.deltaTime)
            {
                position = Vector3.Slerp(_startPosition, _endPosition, i);
                rotation = Quaternion.Slerp(_startRotation, _endRotation, i);
                
                transform.position = position;
                transform.rotation = rotation;
            
                yield return null;
            }
            
            position = _startPosition;
            rotation = _startRotation;
            transform.SetPositionAndRotation(position, rotation);
        }

        _isClosed = !_isClosed;
        _isBusy = false;
    }
}
