﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerRecipeBookController : MonoBehaviour {

	public GameObject recipeBookPanelPrefab;

	public GameObject recipeButtonPrefab;

    public GameObject recipeSlotPrefab;

    public GameObject ingredientSlotPrefab;

	public GameObject recipeBookPanel;

    public InventoryController inventoryController;

    public Inventory inventory;

    PlayerSkillController skillController;

    public RecipeBook recipeBook;

    public Recipe selectedRecipe;

	bool showRecipeBook = false;

	float cameraHeight;
	float cameraWidth;

    protected const string recipesScrollViewName = "RecipesScrollView";
    protected const string recipesViewPortName = "Viewport";
    protected const string recipesContainerName = "RecipesContainer";
    protected const string ingredientsContainerName = "IngredientsContainer";
    protected const string productImageName = "RecipeProductImage";
    protected const string craftButtonName = "CraftButton";
    protected const string ingredientImageName = "IngredientImage";
    protected const string ingredientTextName = "IngredientCount";

	// Use this for initialization
	void Start () {
		
		cameraHeight = Camera.main.pixelHeight;
		cameraWidth = Camera.main.pixelWidth;

        recipeBook = new RecipeBook();

        /*APIManager apiManager = GameObject.FindGameObjectWithTag("APIManager").GetComponent<APIManager>();
        StartCoroutine(apiManager.GetCharacterSkills((recipes) => {
            recipeBook.recipes = recipes;
        }));

        if (recipeBook.recipes.Count >= 1)
            selectedRecipe = recipeBook.recipes[0];
        else
            selectedRecipe = null;*/

		GameObject mainCanvas = GameObject.FindWithTag ("MainCanvas");
		recipeBookPanel = Instantiate (recipeBookPanelPrefab, Vector3.zero, Quaternion.identity, mainCanvas.transform);
        recipeBookPanel.GetComponent<RectTransform>().localPosition = new Vector3(Screen.width + 1000f, 0f, 0f);

        skillController = GameManager.instance.LocalPlayer().GetComponent<PlayerSkillController>();

        GameObject craftButtonObj = recipeBookPanel.transform.Find(craftButtonName).gameObject;
        Button craftButton = craftButtonObj.GetComponent<Button>();

        craftButton.onClick.AddListener(delegate { CraftItem(); });
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C))
			ToggleRecipeBook ();
	}

    public void FindInventory() {
        inventoryController = GameManager.instance.LocalPlayer().GetComponent<InventoryController>();
        inventory = inventoryController.inventory;

        return;
    }

	public void LayoutRecipeBook() {
        
        GameObject recipesScrollView = recipeBookPanel.transform.Find(recipesScrollViewName).gameObject;
        GameObject recipesViewPort = recipesScrollView.transform.Find(recipesViewPortName).gameObject;
        GameObject recipesContainer = recipesViewPort.transform.Find(recipesContainerName).gameObject;

        foreach (Transform recipeSlotTransform in recipesContainer.transform)
        {
            Destroy(recipeSlotTransform.gameObject);
        }

        recipesContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(125f, Mathf.Max(230f, recipeBook.recipes.Count * 50f));

		foreach (Recipe recipe in recipeBook.recipes) {
            GameObject slot = Instantiate(
                recipeSlotPrefab,
                Vector3.zero,
                Quaternion.identity,
                recipesContainer.transform
            );

            slot.GetComponent<RecipeSlot>().recipe = recipe;
            
            Image slotImage = slot.GetComponent<Image>();
            slotImage.sprite = recipe.product.sprite;
            slotImage.color = Color.white;
        }

        DisplayRecipe();
	}

    public void SelectRecipe(Recipe recipe)
    {
        selectedRecipe = recipe;
        DisplayRecipe();
    }

    public void DisplayRecipe() {

        if (selectedRecipe == null)
            return;

        Image productImage = recipeBookPanel.transform.Find(productImageName).gameObject.GetComponent<Image>();
        productImage.sprite = selectedRecipe.product.sprite;
        DisplayIngredients();

        return;
    }

    public void DisplayIngredients() {

        GameObject ingredientsContainer = recipeBookPanel.transform.Find(ingredientsContainerName).gameObject;

        foreach (Transform slotTransform in ingredientsContainer.transform) {
            Destroy(slotTransform.gameObject);
        }

        foreach (RecipeRequirement ingredient in selectedRecipe.ingredients) {
            GameObject slot = Instantiate(
                ingredientSlotPrefab,
                Vector3.zero,
                Quaternion.identity,
                ingredientsContainer.transform
            );

            Image slotImage = slot.transform.Find(ingredientImageName).GetComponent<Image>();
            slotImage.sprite = ingredient.ingredient.sprite;

            Text slotText = slot.transform.Find(ingredientTextName).GetComponent<Text>();
            int currentCount = inventory.CheckQuantity(ingredient.ingredient.Id);
            int requiredCount = ingredient.quantity;
            slotText.text = currentCount.ToString() + "/" + requiredCount.ToString();

            if (currentCount >= requiredCount)
                slotText.color = Color.green;
            else
                slotText.color = Color.red;
        }
    }

	public void CraftItem () {
		
		bool hasRequiredIngredients = true;

        foreach (RecipeRequirement ingredient in selectedRecipe.ingredients) {
			hasRequiredIngredients = hasRequiredIngredients && (inventory.CheckQuantity (ingredient.ingredient.Id) >= ingredient.quantity);
		}

		if (hasRequiredIngredients) {
			foreach (RecipeRequirement ingredient in selectedRecipe.ingredients) {
				inventory.RemoveQtyOfItems (ingredient.ingredient.Id, ingredient.quantity);
			}

			inventory.AddItem (selectedRecipe.product.Id, inventoryController);

            skillController.AddXP(selectedRecipe.expReward);
            skillController.LayoutXP();

            inventoryController.UpdateInventoryPanelUI ();
		}

        DisplayIngredients();
	}

	void ToggleRecipeBook() {
        
		showRecipeBook = !showRecipeBook;

        if (showRecipeBook)
			recipeBookPanel.transform.localPosition = new Vector3 (-cameraWidth / 3, 0f, 0f);
		else
			recipeBookPanel.transform.localPosition = new Vector3 (cameraWidth * 2, 0f, 0f);
	}

    public bool SearchRecipeBook(int searchRecipeId) {
        foreach (Recipe recipe in recipeBook.recipes)
        {
            if (recipe.id == searchRecipeId)
                return true;
        }

        return false;
    }

    public void AddRecipe(Recipe recipe)
    {
        recipeBook.recipes.Add(recipe);
        LayoutRecipeBook();
    }
}