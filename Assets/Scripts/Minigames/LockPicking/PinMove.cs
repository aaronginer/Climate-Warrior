using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinMove : MonoBehaviour
{
    public float speed = 50f;
    public float targetPosition = 0.0f;
    public float errorMargin = 0.025f;

    public bool positioned = false;

    private GameObject selectedObject;
    private Color rightColor = Color.green;
    private Color selectedColor = Color.yellow;
    private Color unselected = Color.gray;
    private Vector3 offset;

    private float pinSize;
    private float topBorder;
    private float bottomBorder;
    

    // Start is called before the first frame update
    void Start()
    {
        pinSize = GetComponent<SpriteRenderer>().bounds.size.y / 2;
        topBorder = Camera.main.ScreenToWorldPoint(Vector3.zero).y*(-1);
        bottomBorder = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
    }


    // Update is called once per frame
    void Update()
    {
        if (PauseScript.instance.gamePaused)
            return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D target = Physics2D.OverlapPoint(mousePosition);
            if (target)
            {
                selectedObject = target.transform.gameObject;
                offset = selectedObject.transform.position - mousePosition;
            }
        }
        if (selectedObject)
        {
            float newPositionX = selectedObject.transform.position.x;
            float newPositionY = mousePosition.y + offset.y;
            float newPositionZ = mousePosition.z + offset.z;

            selectedObject.GetComponent<SpriteRenderer>().color = CheckWin() ? rightColor : selectedColor;
            selectedObject.transform.position = Vector3.MoveTowards(selectedObject.transform.position, new Vector3(newPositionX, newPositionY, newPositionZ), Time.deltaTime * speed);
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject.GetComponent<PinMove>().positioned = CheckWin();
            selectedObject.GetComponent<SpriteRenderer>().color = selectedObject.GetComponent<PinMove>().positioned ? rightColor : unselected;
            selectedObject = null;
        }
    }

    public void movePin(Vector3 position)
    {
        if (position.y < bottomBorder + pinSize|| position.y > (topBorder - pinSize))
            return;
        this.transform.position = position;
    }


    private bool CheckWin()
    {
        return (selectedObject.transform.position.y > targetPosition - errorMargin) && (selectedObject.transform.position.y < targetPosition + errorMargin);
    }
}
