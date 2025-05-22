using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public string GetInteractPrompt();
    public void OnInterect();
}
public class ItemObject : MonoBehaviour , Interactable
{
    public ItemData itemData;
    
    public string GetInteractPrompt()
    {
        string str = $"{itemData.displayName}\n {itemData.description}";
        return str;
    }

    public void OnInterect()
    {
        CharacterManager.Instance.Player.itemData = ScriptableObject.CreateInstance<ItemData>();
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
        
    }
}
