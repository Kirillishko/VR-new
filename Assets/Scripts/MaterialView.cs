using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialView : MonoBehaviour
{
    [SerializeField] Image _leftImage;
    [SerializeField] Image _centerImage;
    [SerializeField] Image _rightImage;
    [SerializeField] Image _helpImage;
    [SerializeField] List<Image> _images;

    private const string _colorName = "_Color";
    private const string _albedoMapName = "_MainTex";
    private const string _roughnessMapName = "_SpecGlossMap";
    private const string _normalMapName = "_BumpMap";

    private List<Material> _materials;

    private int _currentMaterialIndex = 0;

    public void SetMaterials(List<Material> materials)
    {
        _materials = materials;

        SetMaterial(_leftImage.material, _materials[0]);
        SetMaterial(_centerImage.material,_materials[1]);
        SetMaterial(_rightImage.material,_materials[2]);
    }

    private void SetMaterial(Material oldMaterial, Material newMaterial)
    {
        oldMaterial.SetColor(_colorName, newMaterial.GetColor(_colorName));
        oldMaterial.SetTexture(_albedoMapName, newMaterial.GetTexture(_albedoMapName));
        oldMaterial.SetTexture(_roughnessMapName, newMaterial.GetTexture(_roughnessMapName));
        oldMaterial.SetTexture(_normalMapName, newMaterial.GetTexture(_normalMapName));
    }

    public void SetMaterialsAlpha(float alpha)
    {
        foreach (var image in _images)
        {
            SetMaterialAlpha(image, alpha);
        }
    }

    private void SetMaterialAlpha(Image image, float alpha)
    {
        var color = image.material.color;
        color.a = alpha;
        image.material.SetColor(_colorName, color);
    }

    // public IEnumerator MoveToLeft()
    // {
    //
    // }

    public IEnumerator MoveToRight()
    {
        const float step = 0.01f;
        var wait = new WaitForSecondsRealtime(step);

        var startPositions = new List<Vector3>();
        var endPositions = new List<Vector3>();

        _helpImage.rectTransform.localPosition = new Vector3(-140, 0, 0);
        _helpImage.gameObject.SetActive(true);
        var position = _helpImage.rectTransform.localPosition;

        startPositions.Add(position);
        position.x += 70;
        endPositions.Add(position);

        foreach (var image in _images)
        {
            position = image.rectTransform.localPosition;

            startPositions.Add(position);
            position.x += 70;
            endPositions.Add(position);
        }

        SetMaterialAlpha(_helpImage, 0);

        for (float i = 0; i < 1; i += step)
        {
            SetMaterialAlpha(_rightImage, 1 - i);
            SetMaterialAlpha(_helpImage, i);

            _helpImage.rectTransform.localPosition = Vector3.Lerp(startPositions[0], endPositions[0], i);
            
            for (int j = 0; j < _images.Count; j++)
                _images[j].rectTransform.localPosition = Vector3.Lerp(startPositions[j + 1], endPositions[j + 1], i);

            yield return wait;
        }
        
        // Правая становится помогающей
        // Центральная становится правой
        // Левая становится центром
        // Помошающая становится левой

        var helpImage = _helpImage;

        SetMaterial(_helpImage.material,_leftImage.material);
        _helpImage = _leftImage;
        SetMaterial(_leftImage.material,_centerImage.material);
        _leftImage = _centerImage;
        SetMaterial(_centerImage.material,_rightImage.material);
        _centerImage = _rightImage;
        _rightImage = helpImage;

        AddXPosition(_leftImage.rectTransform, -70);
        AddXPosition(_centerImage.rectTransform, -70);
        AddXPosition(_rightImage.rectTransform, -70);
        AddXPosition(_helpImage.rectTransform, -70);
        
        _helpImage.gameObject.SetActive(false);
        SetMaterialAlpha(_rightImage, 1);
    }

    private void AddXPosition(Transform rectTransform, int xPosition)
    {
        var position = rectTransform.localPosition;
        position.x += xPosition;
        rectTransform.localPosition = position;
    }
}
