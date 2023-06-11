using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

[System.Serializable]
public class Item
{
    public string name;
}
public class CartManager : MonoBehaviour
{
    public enum Mode {
        Read,
        Write,
        Both
    };
    public GameObject cartItemPrefab;
    public Transform cartItemsParent;
    public DatabaseManagement database;
    private FirebaseAuth auth;
    private FirebaseUser user;
    public Mode mode;

    // private List<Item> cartItems;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        if (mode == Mode.Both) {
            loadItems();
        }
    }
    // public void AddToCart(string itemName)
    // {
    //     cartItems.Add(itemName);
    //     UpdateCartUI();
    // }

    private async void loadItems() {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        var userCartLists = await database.getUserInfos("carts", user.UserId);
        if(userCartLists != null) {
            UpdateCartUI(userCartLists);
        }
    }

    private void UpdateCartUI(DataSnapshot cartLists)
    {
        // Clear existing cart items
        foreach (Transform child in cartItemsParent)
        {
            Destroy(child.gameObject);
        }
        foreach (DataSnapshot itemName in cartLists.Children)
        {
            var amount = int.Parse(itemName.Value.ToString());
            if(amount > 0) {
                var key = itemName.Key;
                GameObject cartItem = Instantiate(cartItemPrefab, cartItemsParent);
                Transform childObject = cartItem.transform.Find("Product Info");
                childObject.Find("Product Name").GetComponent<TextMeshProUGUI>().text = key;
                childObject.Find("Price").GetComponent<TextMeshProUGUI>().text = "$ 255";
            }
        }
    }
}