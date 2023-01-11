using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialView : MonoBehaviour
{
    [SerializeField] List<Image> _images;

    private static string _colorName = "_Color";

    private List<Material> _materials;
    private MaterialPropertyBlock _mpb;

    private void Start()
    {
        _mpb = new MaterialPropertyBlock();
    }

    public void SetMaterials(List<Material> materials)
    {
        _materials = materials;
    }

    public void SetMaterialsAlpha(float alpha)
    {
        Color color;

        foreach (var image in _images)
        {
            //color = image.material.color;
            //color.a = alpha;
            //image.material.SetColor(_colorName, color);
            image.CrossFadeAlpha(alpha, 0, false);
        }
    }
}
