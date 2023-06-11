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
    private DataSnapshot cartLists;
    public Mode mode;

    // private List<Item> cartItems;

    void Start() {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        if (mode == Mode.Both) {
            loadItems();
        }
    }

    public async void AddToCart(string itemIdx) {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        cartLists = await database.getUserInfos("carts", user.UserId);
        if(cartLists != null) {
            UpdateCartInfo(itemIdx);
        }
    }

    public async void Purchase() {
        foreach (Transform child in cartItemsParent) {
            Transform childObject = child.transform.Find("CheckBox");
            bool toggle = childObject.gameObject.GetComponent<Toggle>().isOn;
            if (toggle) {
                string productName = child.transform.Find("Product Info").Find("Product Name").GetComponent<TextMeshProUGUI>().text;
                print(productName);
                ClearCart(productName);
            }
        }
        // if(cartLists != null) {
        //     ClearCart(itemIdx);
        // }
    }

    private async void loadItems() {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        cartLists = await database.getUserInfos("carts", user.UserId);
        if(cartLists != null) {
            UpdateCartUI();
        }
    }

    private void UpdateCartInfo(string itemIdx) {
        foreach (DataSnapshot itemName in cartLists.Children)
        {
            if(itemIdx == itemName.Key) {
                var amount = int.Parse(itemName.Value.ToString());
                amount += 1;
                itemName.Reference.SetValueAsync(amount);
                database.updateInfo(user.UserId, JsonUtility.ToJson(cartLists));
                return;
            }
        }
    }

    private async void ClearCart(string itemIdx) {
        cartLists = await database.getUserInfos("carts", user.UserId);
        foreach (DataSnapshot itemName in cartLists.Children)
        {
            if(itemIdx == itemName.Key) {
                var amount = int.Parse(itemName.Value.ToString());
                amount = 0;
                itemName.Reference.SetValueAsync(amount);
                database.updateInfo(user.UserId, JsonUtility.ToJson(cartLists));
                return;
            }
        }
    }

    private void UpdateCartUI()
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