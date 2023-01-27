using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.WebGL;
using UnityEngine.UI;

public class PathIndicatorScript : MonoBehaviour
{
    public static PathIndicatorScript Instance;
    public float borderDistance = 0.3f;
    public GameObject image;

    private Vector3 _currentTarget;
    private float _width, _height;

    private bool _enabled;

    void Start()
    {
        Instance = this;
        _width = GetComponentInParent<CanvasScaler>().referenceResolution.x;
        _height = GetComponentInParent<CanvasScaler>().referenceResolution.y;
        image.SetActive(_enabled);
    }

    public void UpdatePosition(Vector3 playerPosition)
    {
        if (!_enabled) return;

        Transform tr = transform;
        
        if (Vector3.Distance(_currentTarget, playerPosition) < borderDistance)
        {
            image.SetActive(false);
        }
        else
        {
            image.SetActive(true);
            
            Vector3 diff = playerPosition - _currentTarget;
            Vector3 angle = new Vector3(0, 0, 0);
            Vector3 position = new Vector3(0, 0, 0);

            if (Math.Abs(Math.Abs(diff.x) - Math.Abs(diff.y)) <= 0.3)
            {
                if (diff.y <= 0 && diff.x <= 0) angle.z = 225;
                else if (diff.y <= 0 && diff.x > 0) angle.z = 315;
                else if (diff.y > 0 && diff.x <= 0) angle.z = 135;
                else if (diff.y > 0 && diff.x > 0) angle.z = 45;
                
                position.y = diff.y <= 0 ? (_height-20) / 2 : -(_height-20) / 2;
                position.x = diff.x <= 0 ? (_width-20) / 2 : -(_width-20) / 2;
            }
            else if (Math.Abs(diff.y) > Math.Abs(diff.x))
            {
                angle.z = diff.y <= 0 ? 270 : 90;
                position.x = 0;
                position.y = diff.y <= 0 ? (_height-20) / 2 : -(_height-20) / 2;
            }
            else
            {
                angle.z = diff.x <= 0 ? 180 : 0;
                position.y = 0;
                position.x = diff.x <= 0 ? (_width-20) / 2 : -(_width-20) / 2;
            }

            tr.eulerAngles = angle;
            tr.localPosition = position;
        }
    }

    public void Enable(Vector3 target)
    {
        _currentTarget = target;
        image.SetActive(true);
        _enabled = true;
    }

    public void Disable()
    {
        image.SetActive(false);
        _enabled = false;
    }
}
