using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileChoice : MonoBehaviour
{
    [SerializeField] private List<LayoutFile> _layoutFiles;
    [SerializeField] private FileView _fileViewTemplate;
    [SerializeField] private ScrollRect _scrollView;
    
    private string _path;
    private bool _fileSetted;

    private const string _extension = ".txt";
    private void Start()
    {
        _path = Application.dataPath + "/StreamingAssets/";

        var content = _scrollView.content;

        foreach (var layoutFile in _layoutFiles)
        {
            var fileView = Instantiate(_fileViewTemplate, content, false);
            fileView.Init(layoutFile, this);
        }
    }

    public void SetPath(string selectedPath)
    {
        Transfer.Path = _path + selectedPath + _extension;
        _fileSetted = true;
        //Debug.Log(Transfer.Path);
    }

    public void NestScene()
    {
        if (_fileSetted == false)
            return;
        
        SceneManager.LoadScene(1);
    }
}


[Serializable]
public struct LayoutFile
{
    [SerializeField] private string _path;
    [SerializeField] private Sprite _sprite;

    public string Path => _path;
    public Sprite Sprite => _sprite;
}
