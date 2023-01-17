using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialView : MonoBehaviour
{
    public bool IsBusy = false;

    [SerializeField] MeshRenderer _leftImage;
    [SerializeField] MeshRenderer _centerImage;
    [SerializeField] MeshRenderer _rightImage;
    [SerializeField] MeshRenderer _helpImage;
    [SerializeField] List<MeshRenderer> _images;
    [SerializeField, Range(0f, 10f)] private float _moveStep = 3f;
    [SerializeField, Range(0f, 10f)] private float _alphaStep = 3f;
    
    private const string _colorName = "_Color";
    private const string _albedoMapName = "_MainTex";
    private const string _roughnessMapName = "_SpecGlossMap";
    private const string _normalMapName = "_BumpMap";

    private MaterialChanger _currentMaterialChanger;
    private List<Material> _materials;
    private int _currentMaterialIndex = 1;

    private void Start()
    {
        SetMaterialsAlpha(0);
    }

    public void SetMaterials(MaterialChanger materialChanger)
    {
        _currentMaterialChanger = materialChanger;
        _materials = _currentMaterialChanger.Materials;
        _currentMaterialIndex = _currentMaterialChanger.CurrentMaterialIndex;

        SetMaterial(_leftImage, -1);
        SetMaterial(_centerImage, 0);
        SetMaterial(_rightImage, 1);
    }

    private void SetMaterial(MeshRenderer meshRenderer, Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }

    private void SetMaterial(MeshRenderer meshRenderer, int offset)
    {
        int index = _currentMaterialIndex;

        if (offset >= 0)
        {
            if (index + offset >= _materials.Count)
                index = index + offset - _materials.Count;
            else
                index += offset;
        }
        else
        {
            if (index + offset < 0)
                index = index + offset + _materials.Count;
            else 
                index += offset;
        }

        SetMaterial(meshRenderer, _materials[index]);
    }

    public void SetMaterialsAlpha(float alpha)
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

        if (++_currentMaterialIndex == _materials.Count)
            _currentMaterialIndex = 0;

        _images.Clear();
        _images.Add(_leftImage);
        _images.Add(_centerImage);
        _images.Add(_rightImage);
        
        _currentMaterialChanger.CurrentMaterialIndex = _currentMaterialIndex;
        _currentMaterialChanger.SetMaterial(_materials[_currentMaterialIndex]);

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

        if (--_currentMaterialIndex == -1)
            _currentMaterialIndex = _materials.Count - 1;

        _images.Clear();

        _images.Add(_leftImage);
        _images.Add(_centerImage);
        _images.Add(_rightImage);
        
        _currentMaterialChanger.CurrentMaterialIndex = _currentMaterialIndex;
        _currentMaterialChanger.SetMaterial(_materials[_currentMaterialIndex]);

        IsBusy = false;
    }
    
    public IEnumerator ShowMaterialView()
    {
        IsBusy = true;
        SetMaterialsAlpha(0);
        
        for (float i = 0; i < 1; i += _alphaStep * Time.deltaTime)
        {
            SetMaterialsAlpha(i);
            yield return null;
        }

        SetMaterialsAlpha(1);
        IsBusy = false;
    }

    public IEnumerator HideMaterialView()
    {
        IsBusy = true;
        SetMaterialsAlpha(1);
        
        for (float i = 1; i >= 0; i -= _alphaStep * Time.deltaTime)
        {
            SetMaterialsAlpha(i);
            yield return null;
        }

        SetMaterialsAlpha(0);
        IsBusy = false;
    }

    public IEnumerator HideAndShow(MaterialChanger materialChanger)
    {
        yield return StartCoroutine(HideMaterialView());
        SetMaterials(materialChanger);
        StartCoroutine(ShowMaterialView());
    }

    private void SetXPosition(Transform objectTransform, float xPosition)
    {
        var position = objectTransform.localPosition;
        position.x = xPosition;
        objectTransform.localPosition = position;
    }
}
