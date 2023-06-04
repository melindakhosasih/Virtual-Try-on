using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CartManager : MonoBehaviour
{
    public GameObject cartItemPrefab;
    public Transform cartItemsParent;

    private List<string> cartItems = new List<string>();

    public void AddToCart(string itemName)
    {
        cartItems.Add(itemName);
        UpdateCartUI();
    }

    private void UpdateCartUI()
    {
        // Clear existing cart items
        foreach (Transform child in cartItemsParent)
        {
            Destroy(child.gameObject);
        }

        // Create UI elements for each item in the cart
        foreach (string itemName in cartItems)
        {
            GameObject cartItem = Instantiate(cartItemPrefab, cartItemsParent);
            cartItem.GetComponentInChildren<Text>().text = itemName;
        }
    }
}