﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Recipe
{

    public int id;
    public int productID;
    public int productQuantity;
    public Sprite productSprite;
    public List<RecipeRequirement> ingredients;
    public bool canCraft = false;

    protected Database database;

    public Recipe(int id, int productID, int productQuantity, List<RecipeRequirement> ingredients, Sprite productSprite)
    {
        this.id = id;
        this.productID = productID;
        this.productQuantity = productQuantity;
        this.ingredients = ingredients;
        this.productSprite = productSprite;
    }

    public Recipe(JsonData recipeJson, JsonData ingredientsJson)
    {

        database = new Database();

        id = (int)recipeJson["id"];
        productID = (int)recipeJson["productId"];
        productQuantity = (int)recipeJson["productQuantity"];

        InventoryItem productItem = database.GetItem(productID);
        productSprite = productItem.sprite;

        ingredients = new List<RecipeRequirement>();

        foreach (JsonData ingredient in ingredientsJson)
        {

            InventoryItem ingredientItem = database.GetItem((int)ingredient["ingredientId"]);
            ingredients.Add(new RecipeRequirement((int)ingredient["ingredientId"], (int)ingredient["ingredientQuantity"], ingredientItem.sprite));
        }
    }

    //This one will probably end up being obsolete by the time I'm done creating a constructor that takes JsonData arguments.
    public Recipe(string recipeJSON, string ingredientsJSON)
    {

        database = new Database();

        this.ingredients = new List<RecipeRequirement>();

        JsonData recipe = JsonMapper.ToObject(File.ReadAllText(recipeJSON));
        //Pull down recipe requirements
        JsonData recipeRequirements = JsonMapper.ToObject(File.ReadAllText(ingredientsJSON));
        foreach (JsonData requirement in recipeRequirements)
        {

            InventoryItem ingredientItem = database.GetItem((int)requirement["ingredientId"]);
            ingredients.Add(new RecipeRequirement((int)requirement["ingredientId"], (int)requirement["ingredientQuantity"], ingredientItem.sprite));
            //int ing = (int)requirement ["ingredientId"];
            //int qty = (int)requirement ["ingredientQuantity"];
            //RecipeRequirement recReq = new RecipeRequirement (ing, qty);
            //ingredients.Add (recReq);

        }

        this.id = (int)recipe[0]["id"];
        this.productID = (int)recipe[0]["productId"];
        this.productQuantity = (int)recipe[0]["productQuantity"];

        InventoryItem productItem = database.GetItem(this.productID);
        this.productSprite = productItem.sprite;
    }
}