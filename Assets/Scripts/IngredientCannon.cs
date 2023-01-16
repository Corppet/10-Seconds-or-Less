using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCannon : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private int maxLaunchInterval = 3;
    [SerializeField] private GameObject[] ingredientPrefabs;

    private IEnumerator LaunchIngredient()
    {
        // wait before launching another ingredient
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, maxLaunchInterval + 1));

        // launch a random ingredient
        GameObject ingredient = Instantiate(ingredientPrefabs[UnityEngine.Random.Range(0, ingredientPrefabs.Length)], 
            transform.position, Quaternion.identity);

        // apply force to the ingredient
        Rigidbody2D ingredientRigidbody = ingredient.GetComponent<Rigidbody2D>();
        float launchAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        ingredientRigidbody.AddForce(new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchForce, 
            ForceMode2D.Impulse);

        // launch another ingredient if still in play
        if (CursorController.Instance.isInPlay && enabled)
        {
            StartCoroutine(LaunchIngredient());
        }
    }

    private void Start()
    {
        StartCoroutine(LaunchIngredient());
    }
}
