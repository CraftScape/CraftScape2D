using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

[System.Serializable]
public class Character : ScriptableObject {

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Currency { get; private set; }
    public double WalkSpeed { get; private set; }
    public List<Inventory> Inventories { get; private set; }
    public List<string> inventoryUrls { get; set; }

    void Init(int id, int userId, string name, int health, int maxHealth, int currency, double walkSpeed, List<string> inventoryUrls)
    {
        this.Id = id;
        this.UserId = userId;
        this.Name = name;
        this.Health = health;
        this.MaxHealth = maxHealth;
        this.Currency = currency;
        this.WalkSpeed = walkSpeed;
        this.inventoryUrls = inventoryUrls;
    }

    void Init(int id, int userId, string name, int health, int maxHealth, int currency, double walkSpeed, List<Inventory> inventories) 
    {
        Init(id, userId, name, health, maxHealth, currency, walkSpeed, new List<string>());
        Inventories = inventories;
    }

    public static Character Parse(JsonData data)
    {
        int id = (int) data["id"];
        int userId = (int) data["user"];
        string name = data["name"].ToString();
        int health = (int) (double) data["health"];
        int maxHealth = (int) (double) data["max_health"];
        int currency = (int) data["currency"];
        double walkSpeed = (double) data["walk_speed"];
        List<string> urls = new List<string>();
        foreach(JsonData url in data["inventories"]) { urls.Add(url.ToString()); }
        Character character = Character.CreateInstance("Character") as Character;
        character.Init(id, userId, name, health, maxHealth, currency, walkSpeed, urls);
        return character;
    }

}