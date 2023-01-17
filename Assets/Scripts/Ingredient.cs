using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum IngredientType
{
    Bread,
    Cheese,
    Lettuce,
    Tomato,
    Meat,
    Onion,
    Egg
}

[RequireComponent(typeof(Rigidbody2D))]
public class Ingredient : MonoBehaviour
{
    public IngredientType ingredientType;

    [Space(10)]

    [Tooltip("True if the object has been selected before and is in its flat orientation. False otherwise.")]
    public bool isFlat;
    [SerializeField] private GameObject SelectPrefab;

    public void SelectIngredient()
    {
        if (!isFlat)
        {
            GameObject ingredient = Instantiate(SelectPrefab, transform.position, transform.rotation);
            CursorController.Instance.selectedRigidbody = ingredient.GetComponent<Rigidbody2D>();
            Destroy(gameObject);
        }
        else
        {
            CursorController.Instance.selectedRigidbody = GetComponent<Rigidbody2D>();
        }
    }
}
