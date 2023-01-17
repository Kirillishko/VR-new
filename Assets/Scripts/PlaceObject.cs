using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public bool IsBusy = false;
    public bool IsPlacing = false;
    
    [SerializeField] private List<ObjectToPlace> _objectTemplates;
    [SerializeField] MeshRenderer _leftImage;
    [SerializeField] MeshRenderer _centerImage;
    [SerializeField] MeshRenderer _rightImage;
    [SerializeField] MeshRenderer _helpImage;
    [SerializeField] List<MeshRenderer> _images;
    [SerializeField, Range(0f, 10f)] private float _moveStep = 3f;
    [SerializeField, Range(0f, 10f)] private float _alphaStep = 3f;

    private const string _colorName = "_Color";
    private int _currentObjectIndex = 0;
    
    public ObjectToPlace CurrentObject { get; private set; }

    private void Start()
    {
        SetMaterial(_leftImage, -1);
        SetMaterial(_centerImage, 0);
        SetMaterial(_rightImage, 1);
        
        SetMaterialsAlpha(0);
    }

    private void SetMaterial(MeshRenderer meshRenderer, Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }

    private void SetMaterial(MeshRenderer meshRenderer, int offset)
    {
        int index = _currentObjectIndex;

        if (offset >= 0)
        {
            if (index + offset >= _objectTemplates.Count)
                index = index + offset - _objectTemplates.Count;
            else
                index += offset;
        }
        else
        {
            if (index + offset < 0)
                index = index + offset + _objectTemplates.Count;
            else 
                index += offset;
        }

        SetMaterial(meshRenderer, _objectTemplates[index].Material);
    }

    private void SetMaterialsAlpha(float alpha)
    {
        foreach (var image in _images)
        {
            SetMaterialAlpha(image, alpha);
        }
    }

    private void SetMaterialAlpha(MeshRenderer meshRenderer, float alpha)
    {
        var color = meshRenderer.material.color;
        color.a = alpha;
        meshRenderer.material.SetColor(_colorName, color);
    }
    
    public IEnumerator MoveToLeft()
    {
        IsBusy = true;

        var startPositions = new List<Vector3>();
        var endPositions = new List<Vector3>();

        SetMaterial(_helpImage, 2);
        _helpImage.transform.localPosition = new Vector3(0.9f, 0, 0);
        _helpImage.gameObject.SetActive(true);
        var position = _helpImage.transform.localPosition;

        startPositions.Add(position);
        position.x -= 0.3f;
        endPositions.Add(position);

        foreach (var image in _images)
        {
            position = image.transform.localPosition;

            startPositions.Add(position);
            position.x -= 0.3f;
            endPositions.Add(position);
        }

        SetMaterialAlpha(_helpImage, 0);

        for (float i = 0; i < 1; i += _moveStep * Time.deltaTime)
        {
            SetMaterialAlpha(_leftImage, 1 - i);
            SetMaterialAlpha(_helpImage, i);

            _helpImage.transform.localPosition = Vector3.Slerp(startPositions[0], endPositions[0], i);
            
            for (int j = 0; j < _images.Count; j++)
                _images[j].transform.localPosition = Vector3.Slerp(startPositions[j + 1], endPositions[j + 1], i);

            yield return null;
        }

        var helpImage = _helpImage;

        _helpImage = _leftImage;
        _leftImage = _centerImage;
        _centerImage = _rightImage;
        _rightImage = helpImage;

        SetXPosition(_leftImage.transform, 0f);
        SetXPosition(_centerImage.transform, 0.3f);
        SetXPosition(_rightImage.transform, 0.6f);
        
        SetMaterialAlpha(_leftImage, 1);
        SetMaterialAlpha(_centerImage, 1);
        SetMaterialAlpha(_rightImage, 1);
        _helpImage.gameObject.SetActive(false);

        if (++_currentObjectIndex == _objectTemplates.Count)
            _currentObjectIndex = 0;

        _images.Clear();
        _images.Add(_leftImage);
        _images.Add(_centerImage);
        _images.Add(_rightImage);
        
        Destroy(CurrentObject.gameObject);
        CurrentObject = Instantiate(_objectTemplates[_currentObjectIndex]);
        CurrentObject.SetCollider(false);

        IsBusy = false;
    }
    
    public IEnumerator MoveToRight()
    {
        IsBusy = true;

        var startPositions = new List<Vector3>();
        var endPositions = new List<Vector3>();

        SetMaterial(_helpImage, -2);
        _helpImage.transform.localPosition = new Vector3(-0.3f, 0, 0);
        _helpImage.gameObject.SetActive(true);
        var position = _helpImage.transform.localPosition;

        startPositions.Add(position);
        position.x += 0.3f;
        endPositions.Add(position);

        foreach (var image in _images)
        {
            position = image.transform.localPosition;

            startPositions.Add(position);
            position.x += 0.3f;
            endPositions.Add(position);
        }

        SetMaterialAlpha(_helpImage, 0);

        for (float i = 0; i < 1; i += _moveStep * Time.deltaTime)
        {
            SetMaterialAlpha(_rightImage, 1 - i);
            SetMaterialAlpha(_helpImage, i);

            _helpImage.transform.localPosition = Vector3.Lerp(startPositions[0], endPositions[0], i);
            
            for (int j = 0; j < _images.Count; j++)
                _images[j].transform.localPosition = Vector3.Lerp(startPositions[j + 1], endPositions[j + 1], i);

            yield return null;
        }

        var rightImage = _rightImage;
        _rightImage = _centerImage;
        _centerImage = _leftImage;
        _leftImage = _helpImage;
        _helpImage = rightImage;

        SetXPosition(_leftImage.transform, 0f);
        SetXPosition(_centerImage.transform, 0.3f);
        SetXPosition(_rightImage.transform, 0.6f);
        
        SetMaterialAlpha(_leftImage, 1);
        SetMaterialAlpha(_centerImage, 1);
        _helpImage.gameObject.SetActive(false);
        SetMaterialAlpha(_rightImage, 1);

        if (--_currentObjectIndex == -1)
            _currentObjectIndex = _objectTemplates.Count - 1;

        _images.Clear();

        _images.Add(_leftImage);
        _images.Add(_centerImage);
        _images.Add(_rightImage);

        Destroy(CurrentObject.gameObject);
        CurrentObject = Instantiate(_objectTemplates[_currentObjectIndex]);
        CurrentObject.SetCollider(false);
        
        IsBusy = false;
    }
    
    private void SetXPosition(Transform objectTransform, float xPosition)
    {
        var position = objectTransform.localPosition;
        position.x = xPosition;
        objectTransform.localPosition = position;
    }
    
    public IEnumerator ShowObjectChoice()
    {
        IsBusy = true;
        SetMaterialsAlpha(0);
        
        for (float i = 0; i < 1; i += _alphaStep * Time.deltaTime)
        {
            SetMaterialsAlpha(i);
            yield return null;
        }

        SetMaterialsAlpha(1);
        CurrentObject = Instantiate(_objectTemplates[_currentObjectIndex]);
        CurrentObject.SetCollider(false);
        IsBusy = false;
    }

    public IEnumerator HideObjectChoice()
    {
        IsBusy = true;
        SetMaterialsAlpha(1);
        
        for (float i = 1; i >= 0; i -= _alphaStep * Time.deltaTime)
        {
            SetMaterialsAlpha(i);
            yield return null;
        }

        SetMaterialsAlpha(0);

        Destroy(CurrentObject.gameObject);
        CurrentObject = null;
        IsBusy = false;
    }

    public void SetObjectPosition(Vector3 position)
    {
        CurrentObject.transform.position = position;
    }

    public void Place()
    {
        CurrentObject.SetPreviousPosition(CurrentObject.transform.position);
        CurrentObject.SetCollider(true);
        CurrentObject = Instantiate(_objectTemplates[_currentObjectIndex]);
        CurrentObject.SetCollider(false);
    }

    public IEnumerator ShowSideImages(bool toPlace)
    {
        IsBusy = true;
        
        if (toPlace)
            Place();
        
        SetMaterialAlpha(_leftImage, 0);
        SetMaterialAlpha(_rightImage, 0);
        
        for (float i = 0; i < 1; i += _alphaStep * Time.deltaTime)
        {
            SetMaterialAlpha(_leftImage, i);
            SetMaterialAlpha(_rightImage, i);
            yield return null;
        }

        SetMaterialAlpha(_leftImage, 1);
        SetMaterialAlpha(_rightImage, 1);
        IsBusy = false;
    }
    
    public IEnumerator HideSideImages()
    {
        IsBusy = true;
        SetMaterialAlpha(_leftImage, 1);
        SetMaterialAlpha(_rightImage, 1);
        
        for (float i = 1; i >= 0; i -= _alphaStep * Time.deltaTime)
        {
            SetMaterialAlpha(_leftImage, i);
            SetMaterialAlpha(_rightImage, i);
            yield return null;
        }

        SetMaterialAlpha(_leftImage, 0);
        SetMaterialAlpha(_rightImage, 0);
        IsBusy = false;
    }
}
