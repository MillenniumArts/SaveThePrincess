using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MerchantController : MonoBehaviour {

    // player reference
    private PlayerController player;

    // items for sale - need Key/value pairs here!
    public Dictionary<string,int> items;

    //number of items for sale
    public int numItemsForSale;

    // text to display player balance
    public Text playerBalance;

    // text to show total amount being purchased
    public Text cartBalance;

    // buttons for increase/decrease quantity

    // text to display amount of each item

    // exit button
    public Button exitButton;

	void Start () {
        // get player
        this.player = FindObjectOfType<PlayerController>();

        // set up items list
        this.items = new Dictionary<string, int>();

        // add Items to Dictionary 
        this.items.Add("Apple", 0);
        this.items.Add("Bread", 0);
        this.items.Add("Cheese", 0);
        this.items.Add("Large Health Potion", 0);
        this.items.Add("Small Health Potion", 0);
        this.items.Add("Large Energy Potion", 0);
        this.items.Add("Small Energy Potion", 0);
	}

    void UpdateText()
    {
        this.playerBalance.text = this.player.dollarBalance.ToString();
    }

	void Update () {
        UpdateText();
	}
}
