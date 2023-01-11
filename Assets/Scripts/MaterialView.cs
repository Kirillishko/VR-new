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

    public IEnumerator MoveToLeft()
    {

    }

    public IEnumerator MoveToRight()
    {
        float step = 0.01f;
        var wait = new WaitForSecondsRealtime(step);

        var startPositions = new List<Vector3>();
        var endPositions = new List<Vector3>();

        var position = _helpImage.transform.position;

        startPositions.Add(position);
        position.x += 70;
        endPositions.Add(position);

        foreach (var image in _images)
        {
            var position = image.transform.position;

            startPositions.Add(position);
            position.x += 70;
            endPositions.Add(position);
        }

        _helpImage.transform.localPosition = new Vector3(-140, 0, 0);
        _helpImage.gameObject.SetActive(true);

        for (float i = 0; i < 1; i += step)
        {
            SetMaterialAlpha(_rightImage, 1 - i);
            SetMaterialAlpha(_helpImage, i);

            _leftImage.transform.position = Vector3.Lerp()

            yield return wait;
        }
    }
}
