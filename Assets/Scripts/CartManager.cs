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
    private Dictionary<string, string> keyValuePairs = new Dictionary<string, string>() {
        {"1", "Apple Watch Series 7"},
        {"2", "Apple Watch Series 8"},
        {"3", "Digital Watch"},
        {"4", "Hand Watch High Poly"},
        {"5", "Seiko Coutura Watch"},
        {"6", "Smart Watch KW 19"},
        {"7", "Rolex Daytona"},
        {"8", "Watch Test"},
        {"9", "Wrist Watch"},
    };
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
                string idx = getProductIdx(productName);
                ClearCart(idx);
            }
        }
        // if(cartLists != null) {
        //     ClearCart(itemIdx);
        // }
    }

    private string getProductIdx(string productName) {
        string key = null;
        foreach (var pair in keyValuePairs) {
            if (pair.Value == productName) {
                key = pair.Key;
                break;
            }
        }
        if (key != null) {
            return key;
        } else {
            return productName;
        }
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
                childObject.Find("Product Name").GetComponent<TextMeshProUGUI>().text = keyValuePairs[key];
                childObject.Find("Price").GetComponent<TextMeshProUGUI>().text = "$ 255";
            }
        }
    }
}