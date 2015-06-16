using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MerchantController : MonoBehaviour {

    // player reference
    private PlayerController player;

    // items for sale 
    public Dictionary<string,int> items;

    //number of items for sale
    public int numItemsForSale;

    private int purchaseBalance, 
                playerBalance, 
                remainingBalance;

    // text to display player balance
    public Text playerBalanceText;

    // text to show total amount being purchased
    public Text purchaseBalanceText;

    // text to show remaining balance after purchase
    public Text remainingBalanceText;

    // buttons for increase/decrease quantity

    // text to display amount of each item

    // purchase button
    public Button purchaseButton;
    // cancel button
    public Button cancelButton;
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
    /// <summary>
    /// Calculate Shop balances here to be updated
    /// </summary>
    void CalculateShop()
    {
        // get palyer balance
        this.playerBalance = this.player.dollarBalance;
        
        // get item balance
        //this.purchaseBalance = 

        // calculate player's remaining balance
        this.remainingBalance = this.playerBalance - this.purchaseBalance;
    }

    void UpdateText()
    {
        this.playerBalanceText.text = this.playerBalance.ToString();
        this.remainingBalanceText.text = this.remainingBalance.ToString();
        this.purchaseBalanceText.text = this.purchaseBalance.ToString();
    }

	void Update () {
        CalculateShop();
        UpdateText();
	}
}
