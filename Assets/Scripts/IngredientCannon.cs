using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCannon : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private int maxLaunchInterval = 3;
    [SerializeField] private GameObject[] ingredientPrefabs;

    private IEnumerator StartLaunch()
    {
        // initial launch is consistent
        yield return new WaitForSeconds(.5f);

        // exit early if no longer in play
        if (!CursorController.Instance.isInPlay || !enabled)
            yield break;

        GameObject ingredient = Instantiate(ingredientPrefabs[0], transform.position, Quaternion.identity);

        // apply force to the ingredient
        Rigidbody2D ingredientRigidbody = ingredient.GetComponent<Rigidbody2D>();
        float launchAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        ingredientRigidbody.AddForce(new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchForce,
            ForceMode2D.Impulse);

        // launch another ingredient
        StartCoroutine(LaunchIngredient());
    }

    private IEnumerator LaunchIngredient()
    {
        // wait before launching another ingredient
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, maxLaunchInterval + 1));

        // exit early if no longer in play
        if (!CursorController.Instance.isInPlay || !enabled)
            yield break;

        // launch a random ingredient
        GameObject ingredient = Instantiate(ingredientPrefabs[UnityEngine.Random.Range(0, ingredientPrefabs.Length)], 
            transform.position, Quaternion.identity);

        // apply force to the ingredient
        Rigidbody2D ingredientRigidbody = ingredient.GetComponent<Rigidbody2D>();
        float launchAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        ingredientRigidbody.AddForce(new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchForce, 
            ForceMode2D.Impulse);

        // launch another ingredient
        StartCoroutine(LaunchIngredient());
    }
    
    private void Start()
    {
        StartCoroutine(StartLaunch());
    }
}
