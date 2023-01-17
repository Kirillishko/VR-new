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

    public int CurrentMaterialIndex = 0;
    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        SetMaterial(Materials[CurrentMaterialIndex]);
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        
        _materialPropertyBlock.Clear();
        _materialPropertyBlock.SetColor(_colorName, material.GetColor(_colorName));
        TrySetTexture(_albedoMapName, material);
        TrySetTexture(_roughnessMapName, material);
        TrySetTexture(_normalMapName, material);

        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void TrySetTexture(string textureName, Material material)
    {
        var texture = material.GetTexture(textureName);

        if (texture != null)
            _materialPropertyBlock.SetTexture(textureName, texture);
    }
}
