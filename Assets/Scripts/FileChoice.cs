using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileChoice : MonoBehaviour
{
    private string _path;
    private void Start()
    {
        _path = Application.dataPath + "/StreamingAssets";
    }
    
    private void Update()
    {
        
    }
}
