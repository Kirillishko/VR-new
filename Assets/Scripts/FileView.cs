using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileView : MonoBehaviour
{
    [SerializeField] private Image _image;

    private LayoutFile _layoutFile;
    private FileChoice _fileChoice;
    
    public void Init(LayoutFile layoutFile, FileChoice fileChoice)
    {
        _layoutFile = layoutFile;
        _fileChoice = fileChoice;

        _image.sprite = layoutFile.Sprite;
    }

    public void Press()
    {
        _fileChoice.SetPath(_layoutFile.Path);
    }
}
