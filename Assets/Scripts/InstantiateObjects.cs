using System;
using System.IO;
using UnityEngine;

public class InstantiateObjects : MonoBehaviour
{
    public GameObject CubeForDoor;
    public GameObject CubeForWall;
    public GameObject CubeForDoorOpening;
    public GameObject CubeForFrame;
    public GameObject CubeForFloor;
    public GameObject CubeForOpening;
    public GameObject CubeForGlass;
    public GameObject CubeForCeiling;

    private string[] _map;
    private string _path;
    private string _fileName = "Layer1";

    private const string Door = "Door";
    private const string Window = "Window";
    private const string PanoramicWindow = "Panoramic window";
    private const string Partition = "Partition";
    private const string Floor = "Floor";
    private const string Opening = "Opening";
    private const string Wall = "Wall";

    void Start()
    {
        _path = Transfer.Path;
        _map = File.ReadAllLines(_path);
        Debug.Log(_path);

        //int heightWallEndIndex = _map[0].IndexOf(" ");
        //int widthEndIndex = _map[0].IndexOf(" ", heightWallEndIndex + 1);

        //var heightWall = Convert.ToSingle(_map[0].Substring(0, heightWallEndIndex));
        //var width = Convert.ToSingle(_map[0].Substring(heightWallEndIndex + 1, widthEndIndex - heightWallEndIndex - 1));
        //var widthFloor = Convert.ToSingle(_map[0].Substring(widthEndIndex + 1, _map[0].Length - (widthEndIndex + 1)));

        //int itagEndIndex;
        //int typeEndIndex;
        //int xEndIndex;
        //int zEndIndex;
        //int angleEndIndex;
        //int lengthEndIndex;

        float heightWall;
        float heightCeiling;
        float heightFloor;
        float heightDoor;
        float lengthFrame;
        float widthWall;
        float widthDoor;

        string type;

        //wall
        float xWall;
        float zWall;
        int angleWall;
        float lengthWall;

        //door
        float xDoor;
        float zDoor;
        int angleDoor;
        float lengthDoor;

        //opening
        float xOpening;
        float zOpening;
        int angleOpening;
        float lengthOpening;

        //window
        float xWindow;
        float zWindow;
        int angleWindow;
        float lengthWindow;

        //panoramic window
        float xPanoramicWindow;
        float zPanoramicWindow;
        int anglePanoramicWindow;
        float lengthPanoramicWindow;

        //floor
        float xFloor;
        float zFloor;
        float lengthFloor;
        float widthFloor;

        ////ceiling
        //float xFloor;
        //float zFloor;
        //float lengthFloor;
        //float widthFloor;

        int heightWallIndex = _map[0].IndexOf(" ");
        int heightCeilingIndex = _map[0].IndexOf(" ", heightWallIndex + 1);
        int heightFloorIndex = _map[0].IndexOf(" ", heightCeilingIndex + 1);
        int heightDoorIndex = _map[0].IndexOf(" ", heightFloorIndex + 1);
        int lengthFrameIndex = _map[0].IndexOf(" ", heightDoorIndex + 1);
        int widthWallIndex = _map[0].IndexOf(" ", lengthFrameIndex + 1);
        int widthDoorIndex = _map[0].Length;

        heightWall = Convert.ToSingle(_map[0].Substring(0, heightWallIndex));
        heightCeiling = Convert.ToSingle(_map[0].Substring(heightWallIndex + 1, heightCeilingIndex - heightWallIndex - 1));
        heightFloor = Convert.ToSingle(_map[0].Substring(heightCeilingIndex + 1, heightFloorIndex - heightCeilingIndex - 1));
        heightDoor = Convert.ToSingle(_map[0].Substring(heightFloorIndex + 1, heightDoorIndex - heightFloorIndex - 1));
        lengthFrame = Convert.ToSingle(_map[0].Substring(heightDoorIndex + 1, lengthFrameIndex - heightDoorIndex - 1));
        widthWall = Convert.ToSingle(_map[0].Substring(lengthFrameIndex + 1, widthWallIndex - lengthFrameIndex - 1));
        widthDoor = Convert.ToSingle(_map[0].Substring(widthWallIndex + 1, widthDoorIndex - widthWallIndex - 1));

        for (int i = 1; i < _map.Length; i++)
        {
            int typeIndex = _map[i].IndexOf(" ");
            type = _map[i].Substring(0, typeIndex);

            if (type == "wall")
            {
                int xWallIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zWallIndex = _map[i].IndexOf(" ", xWallIndex + 1);
                int angleWallIndex = _map[i].IndexOf(" ", zWallIndex + 1);
                int lengthWallIndex = _map[i].Length;

                xWall = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xWallIndex - typeIndex - 1));
                zWall = Convert.ToSingle(_map[i].Substring(xWallIndex + 1, zWallIndex - xWallIndex - 1));
                angleWall = Convert.ToInt32(_map[i].Substring(zWallIndex + 1, angleWallIndex - zWallIndex - 1));
                lengthWall = Convert.ToSingle(_map[i].Substring(angleWallIndex + 1, lengthWallIndex - angleWallIndex - 1));

                CreateWall(widthWall, heightWall, heightFloor, xWall, zWall, angleWall, lengthWall);
            }
            if (type == "door")
            {
                int xDoorIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zDoorIndex = _map[i].IndexOf(" ", xDoorIndex + 1);
                int angleDoorIndex = _map[i].IndexOf(" ", zDoorIndex + 1);
                int lengthDoorIndex = _map[i].Length;

                xDoor = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xDoorIndex - typeIndex - 1));
                zDoor = Convert.ToSingle(_map[i].Substring(xDoorIndex + 1, zDoorIndex - xDoorIndex - 1));
                angleDoor = Convert.ToInt32(_map[i].Substring(zDoorIndex + 1, angleDoorIndex - zDoorIndex - 1));
                lengthDoor = Convert.ToSingle(_map[i].Substring(angleDoorIndex + 1, lengthDoorIndex - angleDoorIndex - 1));

                CreateDoor(widthWall, heightWall, heightFloor, heightDoor, xDoor, zDoor, angleDoor, lengthDoor, widthDoor);
            }
            if (type == "opening")
            {
                int xOpeningIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zOpeningIndex = _map[i].IndexOf(" ", xOpeningIndex + 1);
                int angleOpeningIndex = _map[i].IndexOf(" ", zOpeningIndex + 1);
                int lengthOpeningIndex = _map[i].Length;

                xOpening = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xOpeningIndex - typeIndex - 1));
                zOpening = Convert.ToSingle(_map[i].Substring(xOpeningIndex + 1, zOpeningIndex - xOpeningIndex - 1));
                angleOpening = Convert.ToInt32(_map[i].Substring(zOpeningIndex + 1, angleOpeningIndex - zOpeningIndex - 1));
                lengthOpening = Convert.ToSingle(_map[i].Substring(angleOpeningIndex + 1, lengthOpeningIndex - angleOpeningIndex - 1));

                CreateOpening(widthWall, heightWall, heightFloor, heightDoor, xOpening, zOpening, angleOpening, lengthOpening);
            }
            if (type == "window")
            {
                int xWindowIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zWindowIndex = _map[i].IndexOf(" ", xWindowIndex + 1);
                int angleWindowIndex = _map[i].IndexOf(" ", zWindowIndex + 1);
                int lengthWindowIndex = _map[i].Length;

                xWindow = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xWindowIndex - typeIndex - 1));
                zWindow = Convert.ToSingle(_map[i].Substring(xWindowIndex + 1, zWindowIndex - xWindowIndex - 1));
                angleWindow = Convert.ToInt32(_map[i].Substring(zWindowIndex + 1, angleWindowIndex - zWindowIndex - 1));
                lengthWindow = Convert.ToSingle(_map[i].Substring(angleWindowIndex + 1, lengthWindowIndex - angleWindowIndex - 1));

                CreateWindow(widthWall, heightWall, heightFloor, lengthFrame, xWindow, zWindow, angleWindow, lengthWindow);
            }
            if (type == "panoramicWindow")
            {
                int xPanoramicWindowIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zPanoramicWindowIndex = _map[i].IndexOf(" ", xPanoramicWindowIndex + 1);
                int anglePanoramicWindowIndex = _map[i].IndexOf(" ", zPanoramicWindowIndex + 1);
                int lengthPanoramicWindowIndex = _map[i].Length;

                xPanoramicWindow = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xPanoramicWindowIndex - typeIndex - 1));
                zPanoramicWindow = Convert.ToSingle(_map[i].Substring(xPanoramicWindowIndex + 1, zPanoramicWindowIndex - xPanoramicWindowIndex - 1));
                anglePanoramicWindow = Convert.ToInt32(_map[i].Substring(zPanoramicWindowIndex + 1, anglePanoramicWindowIndex - zPanoramicWindowIndex - 1));
                lengthPanoramicWindow = Convert.ToSingle(_map[i].Substring(anglePanoramicWindowIndex + 1, lengthPanoramicWindowIndex - anglePanoramicWindowIndex - 1));

                CreatePanoramicWindow(heightWall, heightFloor, xPanoramicWindow, zPanoramicWindow, anglePanoramicWindow, lengthPanoramicWindow);
            }
            if (type == "floor")
            {
                int xFloorIndex = _map[i].IndexOf(" ", typeIndex + 1);
                int zFloorIndex = _map[i].IndexOf(" ", xFloorIndex + 1);
                int lengthFloorIndex = _map[i].IndexOf(" ", zFloorIndex + 1);
                int widthFloorIndex = _map[i].Length;

                xFloor = Convert.ToSingle(_map[i].Substring(typeIndex + 1, xFloorIndex - typeIndex - 1));
                zFloor = Convert.ToSingle(_map[i].Substring(xFloorIndex + 1, zFloorIndex - xFloorIndex - 1));
                lengthFloor = Convert.ToSingle(_map[i].Substring(zFloorIndex + 1, lengthFloorIndex - zFloorIndex - 1));
                widthFloor = Convert.ToSingle(_map[i].Substring(lengthFloorIndex + 1, widthFloorIndex - lengthFloorIndex - 1));

                CreateFloor(heightFloor, xFloor, zFloor, lengthFloor, widthFloor);
            }

        }
    }

    private void CreateDoor(float widthWall, float heightWall, float heightFloor, float heightDoor, float xDoor, float zDoor, int angleDoor, float lengthDoor, float widthDoor)
    {
        var DoorOpeningUp = Instantiate(CubeForDoorOpening, transform).transform;

        var newPosition = new Vector3(xDoor, (heightFloor + heightWall) - ((heightWall - heightDoor) / 2), zDoor);

        var newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightWall - heightDoor;
        newScale.z *= lengthDoor;

        DoorOpeningUp.localScale = newScale;
        DoorOpeningUp.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleDoor, 0)));
        
        var DoorOpeningDown = Instantiate(CubeForDoorOpening, transform).transform;
        
        newPosition = new Vector3(xDoor, heightFloor / 2, zDoor);

        newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightFloor;
        newScale.z *= lengthDoor;

        DoorOpeningDown.localScale = newScale;
        DoorOpeningDown.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleDoor, 0)));

        var Door = Instantiate(CubeForDoor, transform).transform;

        newPosition = new Vector3(xDoor, heightFloor + heightDoor / 2, zDoor);

        newScale = Vector3.one;
        newScale.x *= widthDoor;
        newScale.y *= heightDoor;
        newScale.z *= lengthDoor;

        Door.localScale = newScale;
        Door.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleDoor, 0)));
    }

    private void CreateWall(float widthWall, float heightWall, float heightFloor, float xWall, float zWall, int angleWall, float lengthWall)
    {
        var cube = Instantiate(CubeForWall, transform).transform;

        var newPosition = new Vector3(xWall, (heightFloor + heightWall) - (heightWall / 2), zWall);

        var newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightWall;
        newScale.z *= lengthWall;

        cube.localScale = newScale;
        cube.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleWall, 0)));
    }

    private void CreateOpening(float widthWall, float heightWall, float heightFloor, float heightDoor, float xOpening, float zOpening, int angleOpening, float lengthOpening)
    {
        var OpeningUp = Instantiate(CubeForOpening, transform).transform;

        var newPosition = new Vector3(xOpening, (heightFloor + heightWall) - ((heightWall - heightDoor) / 2), zOpening);

        var newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightWall - heightDoor;
        newScale.z *= lengthOpening;

        OpeningUp.localScale = newScale;
        OpeningUp.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleOpening, 0)));
        
        var OpeningDown = Instantiate(CubeForFloor, transform).transform;

        newPosition = new Vector3(xOpening, heightFloor / 2, zOpening);

        newScale = Vector3.one;
        newScale.x *= lengthOpening;
        newScale.y *= heightFloor;
        newScale.z *= widthWall;

        OpeningDown.localScale = newScale;
        OpeningDown.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleOpening - 90, 0)));
    }

    private void CreateWindow(float widthWall, float heightWall, float heightFloor, float lengthFrame, float xWindow, float zWindow, int angleWindow, float lengthWindow)
    {
        float heightOtWindowToFloor = 0.5f;
        float heightWindow = 1.4f;

        var WindowOpeningUp = Instantiate(CubeForOpening, transform).transform;

        var newPosition = new Vector3(xWindow, heightFloor + (heightWall - (heightWall - (heightWindow + heightOtWindowToFloor)) / 2), zWindow);

        var newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightWall - (heightWindow + heightOtWindowToFloor);
        newScale.z *= lengthWindow;

        WindowOpeningUp.localScale = newScale;
        WindowOpeningUp.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleWindow, 0)));

        var WindowOpeningDown = Instantiate(CubeForOpening, transform).transform;

        newPosition = new Vector3(xWindow, heightFloor + heightFloor / 2 + (heightOtWindowToFloor / 2), zWindow);

        newScale = Vector3.one;
        newScale.x *= widthWall;
        newScale.y *= heightWall - (heightWindow + heightOtWindowToFloor);
        newScale.z *= lengthWindow;

        WindowOpeningDown.localScale = newScale;
        WindowOpeningDown.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleWindow, 0)));

        var WindowGlass = Instantiate(CubeForGlass, transform).transform;

        newPosition = new Vector3(xWindow, heightFloor + (heightWall / 2), zWindow);

        newScale = Vector3.one;
        newScale.x *= 0.025f;
        newScale.y *= heightWindow - (lengthFrame * 4);
        newScale.z *= lengthWindow - (lengthFrame * 2);

        WindowGlass.localScale = newScale;
        WindowGlass.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, angleWindow, 0)));
    }

    private void CreatePanoramicWindow(float heightWall, float heightFloor, float xPanoramicWindow, float zPanoramicWindow, int anglePanoramicWindow, float lengthPanoramicWindow)
    {
        var PanoramicWindow = Instantiate(CubeForGlass, transform).transform;

        var newPosition = new Vector3(xPanoramicWindow, (heightFloor + heightWall) - ((heightWall) / 2), zPanoramicWindow);

        var newScale = Vector3.one;
        newScale.x *= 0.05f;
        newScale.y *= heightWall;
        newScale.z *= lengthPanoramicWindow;

        PanoramicWindow.localScale = newScale;
        PanoramicWindow.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, anglePanoramicWindow, 0)));

    }

    private void CreateFloor(float heightFloor, float xFloor, float zFloor, float lengthFloor, float widthFloor)
    {
        var floor = Instantiate(CubeForFloor, transform).transform;

        var newPosition = new Vector3(xFloor, heightFloor / 2, zFloor);

        var newScale = Vector3.one;
        newScale.x = widthFloor;
        newScale.y *= heightFloor;
        newScale.z = lengthFloor;

        floor.localScale = newScale;
        floor.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 0, 0)));

        var ceiling = Instantiate(CubeForCeiling, transform).transform;
        
        newPosition = new Vector3(xFloor, 2.55f, zFloor);
         
        newScale.x = widthFloor * 1.3f;
        newScale.y *= heightFloor;
        newScale.z = lengthFloor * 1.3f;

        ceiling.localScale = newScale;
        ceiling.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
    }

    private void CreateCeiling(float heightWall, float heightCeiling, float heightFloor, float xFloor, float zFloor, float lengthFloor, float widthFloor)
    {
        var ceiling = Instantiate(CubeForCeiling, transform).transform;

        var newPosition = new Vector3(xFloor, heightFloor + heightWall + heightCeiling / 2, zFloor);

        var newScale = Vector3.one;
        newScale.x = widthFloor;
        newScale.y *= heightCeiling;
        newScale.z = lengthFloor;

        ceiling.localScale = newScale;
        ceiling.SetPositionAndRotation(newPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
    }
}