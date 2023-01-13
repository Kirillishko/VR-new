using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialChanger : MonoBehaviour
{
    public List<Material> Materials;

    private const string _colorName = "_Color";
    private const string _albedoMapName = "_MainTex";
    private const string _roughnessMapName = "_SpecGlossMap";
    private const string _normalMapName = "_BumpMap";

    private int _currentMaterialIndex = 0;
    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        SetMaterial(Materials[_currentMaterialIndex]);
    }

    public void SetNextMaterial()
    {
        if (++_currentMaterialIndex == Materials.Count)
            _currentMaterialIndex = 0;

        SetMaterial(Materials[_currentMaterialIndex]);
    }

    public void SetPreviousMaterial()
    {
        if (--_currentMaterialIndex == -1)
            _currentMaterialIndex = Materials.Count - 1;

        SetMaterial(Materials[_currentMaterialIndex]);
    }

    private void SetMaterial(Material material)
    {
        //_meshRenderer.GetPropertyBlock(_materialPropertyBlock);

        //_materialPropertyBlock.

        _meshRenderer.material.SetColor(_colorName, material.GetColor(_colorName));
        _meshRenderer.material.SetTexture(_albedoMapName, material.GetTexture(_albedoMapName));
        _meshRenderer.material.SetTexture(_roughnessMapName, material.GetTexture(_roughnessMapName));
        _meshRenderer.material.SetTexture(_normalMapName, material.GetTexture(_normalMapName));
    }
}
