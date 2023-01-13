using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialView : MonoBehaviour
{

    [SerializeField] MeshRenderer _leftImage;
    [SerializeField] MeshRenderer _centerImage;
    [SerializeField] MeshRenderer _rightImage;
    [SerializeField] MeshRenderer _helpImage;
    [SerializeField] List<MeshRenderer> _images;

    private MaterialChanger _currentMaterialChanger;

    private const string _colorName = "_Color";
    private const string _albedoMapName = "_MainTex";
    private const string _roughnessMapName = "_SpecGlossMap";
    private const string _normalMapName = "_BumpMap";

    private List<Material> _materials;

    private int _currentMaterialIndex = 1;

    //public void SetNextMaterial(MeshRenderer meshRenderer)
    //{
    //    if (++_currentMaterialIndex == _materials.Count)
    //        _currentMaterialIndex = 0;

    //    SetMaterial(meshRenderer, _materials[_currentMaterialIndex]);
    //}

    //public void SetPreviousMaterial()
    //{
    //    if (--_currentMaterialIndex == -1)
    //        _currentMaterialIndex = Materials.Count - 1;

    //    SetMaterial(Materials[_currentMaterialIndex]);
    //}

    public void SetMaterials(MaterialChanger materialChanger)
    {
        _currentMaterialChanger = materialChanger;
        _materials = _currentMaterialChanger.Materials;
        _currentMaterialIndex = _currentMaterialChanger.CurrentMaterialIndex;

        SetMaterial(_leftImage, _currentMaterialIndex - 1);
        SetMaterial(_centerImage, _currentMaterialIndex);
        SetMaterial(_rightImage, _currentMaterialIndex + 1);
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
                index -= _materials.Count;
        }
        else
        {
            if (index + offset < 0)
                index = index + offset + _materials.Count;
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

    // public IEnumerator MoveToLeft()
    // {
    //
    // }

    public IEnumerator MoveToRight()
    {
        const float step = 0.04f;
        var wait = new WaitForSecondsRealtime(step);

        var startPositions = new List<Vector3>();
        var endPositions = new List<Vector3>();

        SetMaterial(_helpImage, -1);
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

        for (float i = 0; i < 1; i += step)
        {
            SetMaterialAlpha(_rightImage, 1 - i);
            SetMaterialAlpha(_helpImage, i);

            _helpImage.transform.localPosition = Vector3.Lerp(startPositions[0], endPositions[0], i);
            
            for (int j = 0; j < _images.Count; j++)
                _images[j].transform.localPosition = Vector3.Lerp(startPositions[j + 1], endPositions[j + 1], i);

            yield return wait;
        }
        
        // Правая становится помогающей
        // Центральная становится правой
        // Левая становится центром
        // Помошающая становится левой

        var rightImage = _rightImage;

        //SetMaterial(_helpImage,_leftImage.material);
        //_helpImage = _leftImage;
        //SetMaterial(_leftImage,_centerImage.material);
        //_leftImage = _centerImage;
        //SetMaterial(_centerImage,_rightImage.material);
        //_centerImage = _rightImage;
        //_rightImage = helpImage;
        _rightImage = _centerImage;
        _centerImage = _leftImage;
        _leftImage = _helpImage;
        _helpImage = rightImage;

        //SetMaterial(_rightImage, _centerImage.material);
        //SetMaterial(_centerImage, _leftImage.material);
        //SetMaterial(_leftImage, _helpImage.material);
        //SetMaterial(_leftImage, _centerImage.material);
        //SetMaterial(_centerImage, _rightImage.material);

        SetXPosition(_leftImage.transform, 0f);
        SetXPosition(_centerImage.transform, 0.3f);
        SetXPosition(_rightImage.transform, 0.6f);
        //SetXPosition(_helpImage.transform, -0.3f);
        
        SetMaterialAlpha(_leftImage, 1);
        SetMaterialAlpha(_centerImage, 1);
        _helpImage.gameObject.SetActive(false);
        SetMaterialAlpha(_rightImage, 1);

        if (--_currentMaterialIndex == -1)
        {
            _currentMaterialIndex = _materials.Count - 1;
            _currentMaterialChanger.CurrentMaterialIndex = _currentMaterialIndex;
        }

        _images.Clear();

        _images.Add(_leftImage);
        _images.Add(_centerImage);
        _images.Add(_rightImage);
    }

    private void AddXPosition(Transform objectTransform, float xPosition)
    {
        var position = objectTransform.localPosition;
        position.x += xPosition;
        objectTransform.localPosition = position;
    }

    private void SetXPosition(Transform objectTransform, float xPosition)
    {
        var position = objectTransform.localPosition;
        position.x = xPosition;
        objectTransform.localPosition = position;
    }
}
