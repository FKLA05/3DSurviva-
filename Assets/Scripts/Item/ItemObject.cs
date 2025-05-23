using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInterect();
}
public class ItemObject : MonoBehaviour , IInteractable
{
    public ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n {data.description}";
        return str;
    }

    public void OnInterect()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
        
    }
}
