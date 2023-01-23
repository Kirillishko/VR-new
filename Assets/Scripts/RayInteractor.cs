using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

// Grip - перетаскивать поставленный объект
// B/M1 - отмена и закрытие менюшек
// A/Space - принять, открыть меню объектов
// RT/M0 - выбор лучом :
//      · если в Material Changer :
//          · если прошлого нету, то открывается меню текующего
//          · если прошлый есть, то его меню закрывается и открывается текущий
//      · если в поставленный объект, то он выбирается

public class RayInteractor : MonoBehaviour
{
    [Header("Actions")]
    //[SerializeField] private InputActionReference _triggerReference;
    //[SerializeField] private InputActionReference _stickReference;
    //[SerializeField] private InputActionReference _primaryButtonReference;
    //[SerializeField] private InputActionReference _secondaryButtonReference;

    private InputDevice _targetDevice;

    [Header("Dependencies")]
    [SerializeField] private MaterialView _materialView;
    [SerializeField] private PlaceObject _placeObject;
    //[SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _rotateSpeed;

    private MaterialChanger _currentMaterialChanger;
    private ObjectToPlace _currentObjectToPlace;
    private bool _materialViewVisible = false;
    private bool _placeObjectVisible = false;
    private bool _isChanging = false;

    private bool _primaryPressed = false;
    private bool _secondaryPressed = false;

    private void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        if (devices.Count > 0)
            _targetDevice = devices[0];

    }

    private void Update()
    {
        if (IsGripPressed() && IsTriggerPressed() && IsPrimaryPressed())
            SceneManager.LoadScene(0);

        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out var hit))
        {
            if (IsTriggerPressed())
            {
                if (hit.transform.TryGetComponent(out MaterialChanger materialChanger))
                {
                    if (_materialView.IsBusy == false && _currentMaterialChanger != materialChanger && _isChanging == false)
                    {
                        _currentMaterialChanger = materialChanger;

                        if (_materialViewVisible == false)
                        {
                            _materialView.SetMaterials(materialChanger);
                            _materialViewVisible = true;

                            if (_placeObjectVisible)
                                StartCoroutine(ObjectToMaterial());
                            else
                                StartCoroutine(_materialView.ShowMaterialView());
                        }
                        else
                        {
                            StartCoroutine(_materialView.HideAndShow(materialChanger));
                        }
                    }
                }

                if (hit.transform.TryGetComponent(out ObjectToPlace objectToPlace))
                {
                    _currentObjectToPlace = objectToPlace;
                    _currentObjectToPlace.SetCollider(false);
                }
            }

            if (hit.transform.TryGetComponent(out Floor _))
            {
                if (_placeObject.CurrentObject != null && _isChanging == false)
                    _placeObject.SetObjectPosition(hit.point);
                else if (_currentObjectToPlace != null)
                    _currentObjectToPlace.transform.position = hit.point;
            }

            if (hit.transform.TryGetComponent(out Door door))
            {
                if (IsGripPressed())
                    StartCoroutine(door.Interact());
            }

            if (IsSecondaryPressed())
            {
                if (_currentObjectToPlace != null)
                {
                    _currentObjectToPlace.ResetPosition();
                    _currentObjectToPlace.SetCollider(true);
                    _currentObjectToPlace = null;
                }
                else if (_materialViewVisible && _materialView.IsBusy == false && _isChanging == false)
                {
                    StartCoroutine(_materialView.HideMaterialView());
                    _materialViewVisible = false;
                    _currentMaterialChanger = null;
                }
                else if (_placeObjectVisible && _placeObject.IsBusy == false && _isChanging == false)
                {
                    if (_placeObject.IsPlacing == false)
                    {
                        StartCoroutine(_placeObject.HideObjectChoice());
                        _placeObjectVisible = false;
                    }
                    else
                    {
                        _placeObject.IsPlacing = false;
                        StartCoroutine(_placeObject.ShowSideImages(false));
                    }
                }
            }
            
            var start = transform.position;
            start.y -= 0.5f;

            //_lineRenderer.SetPosition(0, start);
            //_lineRenderer.SetPosition(1, hit.point);
            //_lineRenderer.enabled = true;

            _targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            _targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
            _targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            _targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 stickValue);
            //Debug.Log($"Primary : {primaryButtonValue}");
            //Debug.Log($"Secondary : {secondaryButtonValue}");
            //Debug.Log($"Trigger : {triggerValue}");
            //Debug.Log($"Stick : {stickValue}");

            //Debug.Log(hit.transform.name);
        }
        else
        {
            //_lineRenderer.enabled = false;
        }

        if (IsPrimaryPressed())
        {
            if (_currentObjectToPlace != null)
            {
                _currentObjectToPlace.SetPreviousPosition(_currentObjectToPlace.transform.position);
                _currentObjectToPlace.SetCollider(true);
                _currentObjectToPlace = null;
            }
            else if (_placeObject.IsBusy == false && _isChanging == false && _placeObjectVisible == false)
            {
                // if (_placeObjectVisible)
                //     StartCoroutine(_placeObject.HideObjectChoice());
                // else
                // {
                if (_materialViewVisible)
                    StartCoroutine(MaterialToObject());
                else
                    StartCoroutine(_placeObject.ShowObjectChoice());

                _currentMaterialChanger = null;
                //}

                _placeObjectVisible = !_placeObjectVisible;
            }
            else
            {
                if (_placeObject.IsPlacing == false)
                {
                    _placeObject.IsPlacing = true;
                    StartCoroutine(_placeObject.HideSideImages());
                }
                else if (_placeObject.CurrentObject != null && _placeObject.CurrentObject.transform.position != Vector3.zero)
                {
                    _placeObject.IsPlacing = false;
                    StartCoroutine(_placeObject.ShowSideImages(true));
                }
            }
        }

        if (_currentObjectToPlace != null)
        {
            if (IsStickToLeft())
            {
                _currentObjectToPlace.Rotate(-_rotateSpeed);
            }
            else if (IsStickToRight())
            {
                _currentObjectToPlace.Rotate(_rotateSpeed);
            }
        }
        else if (_materialViewVisible && _currentMaterialChanger != null && _materialView.IsBusy == false && _isChanging == false)
        {
            if (IsStickToLeft())
            {
                StartCoroutine(_materialView.MoveToLeft());
            }
            else if (IsStickToRight())
            {
                StartCoroutine(_materialView.MoveToRight());
            }
        }
        else if (_placeObjectVisible && _placeObject.IsBusy == false && _isChanging == false)
        {
            if (_placeObject.IsPlacing == false)
            {
                if (IsStickToLeft())
                {
                    StartCoroutine(_placeObject.MoveToLeft());
                }
                else if (IsStickToRight())
                {
                    StartCoroutine(_placeObject.MoveToRight());
                }
            }
            else
            {
                if (IsStickToLeft())
                {
                    _placeObject.CurrentObject.Rotate(-_rotateSpeed);
                }
                else if (IsStickToRight())
                {
                    _placeObject.CurrentObject.Rotate(_rotateSpeed);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, transform.forward);
    }

    private IEnumerator MaterialToObject()
    {
        _isChanging = true;
        
        yield return StartCoroutine(_materialView.HideMaterialView());
        _materialViewVisible = false;
        yield return StartCoroutine(_placeObject.ShowObjectChoice());
        _placeObjectVisible = true;

        _isChanging = false;
    }
    
    private IEnumerator ObjectToMaterial()
    {
        _isChanging = true;
        
        yield return StartCoroutine(_placeObject.HideObjectChoice());
        _placeObjectVisible = false;
        yield return StartCoroutine(_materialView.ShowMaterialView());
        _materialViewVisible = true;

        _isChanging = false;
    }

    private bool IsTriggerPressed()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float value);

        return value >= 0.8f || Input.GetMouseButtonDown(0);
    }

    private bool IsPrimaryPressed()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool value);

        if (value && _primaryPressed)
            value = false;
        else if (value && _primaryPressed == false)
            _primaryPressed = true;
        else if (value == false && _primaryPressed)
            _primaryPressed = false;

        return value || Input.GetKeyDown(KeyCode.Space);
    }

    private bool IsSecondaryPressed()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool value);

        if (value && _secondaryPressed)
            value = false;
        else if (value && _secondaryPressed == false)
            _secondaryPressed = true;
        else if (value == false && _secondaryPressed)
            _secondaryPressed = false;

        return value || Input.GetMouseButtonDown(1);
    }

    private bool IsStickToLeft()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value);

        return value.x >= 0.8f || Input.GetKey(KeyCode.Q);
    }

    private bool IsStickToRight()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 value);

        return value.x <= -0.8f || Input.GetKey(KeyCode.E);
    }

    private bool IsGripPressed()
    {
        _targetDevice.TryGetFeatureValue(CommonUsages.grip, out float value);

        return value >= 0.8f || Input.GetKeyDown(KeyCode.LeftShift);
    }
}
