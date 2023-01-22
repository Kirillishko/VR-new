using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialChanger : MonoBehaviour
{
    public List<Material> Materials;
    public bool ToDelete;

    private const string _colorName = "_Color";
    private const string _albedoMapName = "_MainTex";
    private const string _metallicMapName = "_MetallicGlossMap";
    private const string _roughnessMapName = "_SpecGlossMap";
    private const string _normalMapName = "_BumpMap";
    private const string _heightMapName = "_ParallaxMap";
    private const string _occlusionMapName = "_OcclusionMap";

    [HideInInspector] public int CurrentMaterialIndex = 0;
    private MaterialPropertyBlock _materialPropertyBlock;
    private MeshRenderer _meshRenderer;
    [SerializeField] private float _xScaleMult = 1;
    [SerializeField] private float _yScaleMult = 1;
    [SerializeField] private AxisExpand _axisExpand;
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        // var textureScale = new Vector2();
        // textureScale.x = Mathf.Max(transform.localScale.x, transform.localScale.z) * _xScaleMult;
        // textureScale.y = transform.localScale.y * _yScaleMult;
        // _meshRenderer.material.mainTextureScale = textureScale;
        
        SetMaterial(Materials[CurrentMaterialIndex]);
        
        var textureScale = Vector2.one;

        if (_axisExpand == AxisExpand.Horizontal)
        {
            textureScale.x = transform.localScale.x * _xScaleMult;
            textureScale.y = transform.localScale.z * _yScaleMult;
        }
        else if (_axisExpand == AxisExpand.Vertical)
        {
            //textureScale.x = transform.localScale.x * _xScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
        }

        _meshRenderer.material.mainTextureScale = textureScale;
        
        if (ToDelete)
            Destroy(this);
    }

    private void Update()
    {
        var textureScale = Vector2.one;

        if (_axisExpand == AxisExpand.Horizontal)
        {
            textureScale.x = transform.localScale.x * _xScaleMult;
            textureScale.y = transform.localScale.z * _yScaleMult;
        }
        else if (_axisExpand == AxisExpand.Vertical)
        {
            //textureScale.x = transform.localScale.x * _xScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
            //textureScale.y = transform.localScale.y * _yScaleMult;
        }

        _meshRenderer.material.mainTextureScale = textureScale;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        
        _materialPropertyBlock.Clear();
        _materialPropertyBlock.SetColor(_colorName, material.GetColor(_colorName));
        TrySetTexture(_albedoMapName, material);
        TrySetTexture(_metallicMapName, material);
        TrySetTexture(_roughnessMapName, material);
        TrySetTexture(_normalMapName, material);
        TrySetTexture(_heightMapName, material);
        TrySetTexture(_occlusionMapName, material);

        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void TrySetTexture(string textureName, Material material)
    {
        var texture = material.GetTexture(textureName);

        if (texture != null)
            _materialPropertyBlock.SetTexture(textureName, texture);
    }
}

public enum AxisExpand
{
    Horizontal,
    Vertical
}