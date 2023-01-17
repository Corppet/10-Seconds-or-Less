using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance { get; private set; }

    [Range(0f, 10f)]
    [SerializeField] private float gameDuration = 10f;
    [Tooltip("The number of ingredients the sandwich recipe will have " +
        "(does not include top and bottom buns).")]

    [SerializeField] private KeyCode mainMenuKey = KeyCode.Escape;

    [SerializeField] private Image[] ingredientImages;
    [SerializeField] private Sprite[] ingredientSprites;
    [SerializeField] private Transform foodPlate;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string menuScene = "Main Menu";

    private List<IngredientType> sandwichRecipe;
    private int score;

    public static int LCS(List<IngredientType> SequenceX, List<IngredientType> SequenceY, 
        int x, int y)
    {
        if (x == 0 || y == 0)
        {
            return 0;
        }
        else if (SequenceX[x - 1] == SequenceY[y - 1])
        {
            return 1 + LCS(SequenceX, SequenceY, x - 1, y - 1);
        }
        else
        {
            return Math.Max(LCS(SequenceX, SequenceY, x, y - 1), 
                LCS(SequenceX, SequenceY, x - 1, y));
        }
    }

    public static void ReturnToMenu()
    {
        SceneManager.LoadScene(Instance.menuScene);
    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void GameOver()
    {
        gameDuration = 0f;
        CursorController.Instance.isInPlay = false;
        CursorController.Instance.selectedRigidbody = null;

        // get the list of ingredients on the plate
        RaycastHit2D[] hits = Physics2D.RaycastAll(foodPlate.position, Vector2.up, 50f);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        List<IngredientType> plateIngredients = new List<IngredientType>();
        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag("Food"))
                continue;

            Ingredient ingredient = hit.collider.GetComponent<Ingredient>();
            if (ingredient.isFlat)
                plateIngredients.Add(ingredient.ingredientType);
        }

        // calculate the score based on the sandwich recipe
        // by finding the "longest common subsequence"
        score = LCS(sandwichRecipe, plateIngredients, sandwichRecipe.Count, plateIngredients.Count);
        scoreText.text = "Score: " + score + " / " + ingredientImages.Length;
        gameOverPanel.SetActive(true);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int length = ingredientImages.Length;

        // create a random sandwich recipe
        sandwichRecipe = new List<IngredientType>
        {
            IngredientType.Bread
        };
        ingredientImages[0].sprite = ingredientSprites[0];
        Array ingredientTypes = Enum.GetValues(typeof(IngredientType));
        for (int i = 1; i < length - 1; i++)
        {
            IngredientType ingredient = (IngredientType)ingredientTypes.GetValue(
                UnityEngine.Random.Range(1, ingredientTypes.Length));
            sandwichRecipe.Add(ingredient);
            ingredientImages[i].sprite = ingredientSprites[ingredient.GetHashCode()];
        }
        sandwichRecipe.Add(IngredientType.Bread);
        ingredientImages[length - 1].sprite = ingredientSprites[0];

        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        // main menu
        if (Input.GetKeyDown(mainMenuKey))
        {
            ReturnToMenu();
        }

        if (!CursorController.Instance.isInPlay)
            return;

        // countdown timer
        gameDuration -= Time.deltaTime;
        if (gameDuration <= 0f)
        {
            GameOver();
        }
        timerText.text = gameDuration.ToString("F2");
    }
}
