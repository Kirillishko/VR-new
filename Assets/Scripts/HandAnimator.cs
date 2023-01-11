using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    public InputActionReference GripReference;
    public InputActionReference TriggerReference;
    public InputActionReference StickReference;
    public InputActionReference PrimaryButtonReference;     // 	[X/A]

    [Range(0f,0.1f)] public float PrimaryButtonStep;
    private float _currentStep;
    private float _currentState = 0;

    private Animator _anim;

    private readonly string _grip = "Grip";
    private readonly string _trigger = "Trigger";
    private readonly string _yStick = "YStick";
    private readonly string _primaryButton = "PrimaryButton";

    Coroutine _coroutine;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        if (PrimaryButtonReference != null)
        {
            PrimaryButtonReference.action.performed += PrimaryButtonPerformed;
            PrimaryButtonReference.action.canceled += PrimaryButtonCanceled;
        }
    }

    private void PrimaryButtonPerformed(InputAction.CallbackContext obj)
    {
        _currentStep = PrimaryButtonStep;
        if (_coroutine == null) _coroutine = StartCoroutine(PrimaryButtonAnim());
    }

    private void PrimaryButtonCanceled(InputAction.CallbackContext obj)
    {
        _currentStep = -PrimaryButtonStep;
        if (_coroutine == null) _coroutine = StartCoroutine(PrimaryButtonAnim());
    }

    IEnumerator PrimaryButtonAnim()
    {
        for (; _currentState >= 0 && _currentState <= 1; _currentState += _currentStep)
        {
            _anim.SetFloat(_primaryButton, _currentState);
            yield return null;
        }

        if (_currentState > 1) _currentState = 1;
        if (_currentState < 0) _currentState = 0;

        _coroutine = null;  
    }

    private void Update()
    {
        SetFloatAnim(_grip, GripReference);
        SetFloatAnim(_trigger, TriggerReference);
        SetVector2Anim(_yStick, StickReference);
    }

    private void SetFloatAnim(string animParameter, InputActionReference actionReference)
    {
        float value = actionReference.action.ReadValue<float>();
        if (value < 0) return;
        _anim.SetFloat(animParameter, value);
    }

    private void SetVector2Anim(string animParameter, InputActionReference actionReference)
    {
        Vector2 value = actionReference.action.ReadValue<Vector2>();
        _anim.SetFloat(animParameter, value.y);
    }
}
