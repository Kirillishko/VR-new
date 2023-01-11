using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialChanger : MonoBehaviour
{
    public List<Material> Materials;

    private static string _colorName = "_Color";
    private static string _albedoName = "_MainTex";

    private int _currentMaterialIndex = 0;
    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        SetMaterial(Materials[_currentMaterialIndex]);
    }

    private void SetMaterial(Material material)
    {
        //_meshRenderer.GetPropertyBlock(_materialPropertyBlock);

        //_materialPropertyBlock.

        _meshRenderer.material.SetColor(_colorName, material.GetColor(_colorName));
        _meshRenderer.material.SetTexture(_albedoName, material.GetTexture(_albedoName));
    }
}
