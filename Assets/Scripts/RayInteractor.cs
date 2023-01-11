using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteractor : MonoBehaviour
{
    [SerializeField] private MaterialView _materialView;
    [SerializeField] private LineRenderer _lineRenderer;    //  Удалить

    private MaterialView _currentMaterialView;
    private bool _materialViewEnabled = false;


    private void Update()
    {
        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.TryGetComponent(out MaterialChanger materialChanger))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (_materialViewEnabled == false)
                    {
                        _materialViewEnabled = true;
                        _materialView.SetMaterials(materialChanger.Materials);
                        StartCoroutine(ShowMaterialView());
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _materialViewEnabled = false;
                    StartCoroutine(HideMaterialView());
                }


                _lineRenderer.SetPosition(1, hit.point);
                _lineRenderer.enabled = true;
            }

            Debug.Log(hit.transform.name);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    private IEnumerator ShowMaterialView()
    {
        float step = 0.05f;
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.05f);

        for (float i = 0; i < 1; i += step)
        {
            _materialView.SetMaterialsAlpha(i);
            yield return wait;
        }
    }

    private IEnumerator HideMaterialView()
    {
        float step = 0.05f;
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.05f);

        for (float i = 1; i >= 0; i -= step)
        {
            _materialView.SetMaterialsAlpha(i);
            yield return wait;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
