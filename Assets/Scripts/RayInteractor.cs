using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private MaterialView _materialView;
    [SerializeField] private PlaceObject _placeObject;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _rotateSpeed;

    private MaterialChanger _currentMaterialChanger;
    private ObjectToPlace _currentObjectToPlace;
    private bool _materialViewVisible = false;
    private bool _placeObjectVisible = false;
    private bool _isChanging = false;

    private void Update()
    {
        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out var hit))
        {
            if (Input.GetMouseButtonDown(0))
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

            if (Input.GetMouseButtonDown(1))
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
            
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, hit.point);
            _lineRenderer.enabled = true;
            
            Debug.Log(hit.transform.name);
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
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
                else if (_placeObject.CurrentObject.transform.position != Vector3.zero)
                {
                    _placeObject.IsPlacing = false;
                    StartCoroutine(_placeObject.ShowSideImages(true));
                }
            }
        }

        if (_currentObjectToPlace != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _currentObjectToPlace.Rotate(-_rotateSpeed);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                _currentObjectToPlace.Rotate(_rotateSpeed);
            }
        }
        if (_materialViewVisible && _currentMaterialChanger != null && _materialView.IsBusy == false && _isChanging == false)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(_materialView.MoveToLeft());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(_materialView.MoveToRight());
            }
        }
        else if (_placeObjectVisible && _placeObject.IsBusy == false && _isChanging == false)
        {
            if (_placeObject.IsPlacing == false)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StartCoroutine(_placeObject.MoveToLeft());
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(_placeObject.MoveToRight());
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    _placeObject.CurrentObject.Rotate(-_rotateSpeed);
                }
                else if (Input.GetKey(KeyCode.E))
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
}
