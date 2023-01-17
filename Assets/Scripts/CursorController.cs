using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [HideInInspector] public static CursorController Instance { get; private set; }

    [SerializeField] private float maxForce = 10f;
    public Rigidbody2D selectedRigidbody;
    public bool isInPlay;

    private Vector3 cursorPosition;
    private Vector2 cursorForce;
    private Vector3 lastPosition;
    private float originalRotation;
    private Vector3 offset;

    private void Awake()
    {
        selectedRigidbody = null;
        isInPlay = true;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isInPlay)
            return;
        
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // select and "pick up" an object with the cursor
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(cursorPosition);
            if (targetObject)
            {
                targetObject.GetComponent<Ingredient>().SelectIngredient();
                originalRotation = selectedRigidbody.rotation;
                offset = selectedRigidbody.transform.position - cursorPosition;
            }
        }
        // move the selected object with the cursor
        else if (selectedRigidbody)
        {
            cursorForce = (cursorPosition - lastPosition) / Time.deltaTime;
            cursorForce = Vector2.ClampMagnitude(cursorForce, maxForce);
            lastPosition = cursorPosition;

            selectedRigidbody.rotation = originalRotation - cursorForce.x * 2f;

            // release the selected object
            if (Input.GetMouseButtonUp(0))
            {
                selectedRigidbody.velocity = Vector2.zero;
                selectedRigidbody.AddForce(cursorForce, ForceMode2D.Impulse);
                selectedRigidbody = null;
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            selectedRigidbody.MovePosition(cursorPosition + offset);
        }
    }
}
