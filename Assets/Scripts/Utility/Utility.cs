#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
#endregion


public class Utility
{
    /// <summary>
    /// Returns true if the enum is the same as the enum being checked.
    /// </summary>
    /// <returns></returns>
    public static bool IsEqual<T>(T ItemOne, T ItemTwo)
    {
        if (ItemOne.Equals(ItemTwo))
        {
            return true;
        }
        else
        {
            Debug.LogWarning("Item is not equal to the other, Item One is" + ItemOne.GetType().Name + "Item Two is:" + ItemTwo.GetType().Name);
            return false;
        }

    }

}



	public class UI
	{ 
    
         public static void SetupButton(Button ChangeButton, UnityAction ButtonFunction, string ButtonText)
		{
         
            ChangeButton.onClick.RemoveAllListeners();
			ChangeButton.onClick.AddListener(ButtonFunction);
            if (ChangeButton.gameObject.GetComponentInChildren<Text>())
                ChangeButton.gameObject.GetComponentInChildren<Text>().text = ButtonText;
            else if (ChangeButton.gameObject.GetComponentInChildren<TMP_Text>())
                ChangeButton.gameObject.GetComponentInChildren<TMP_Text>().text = ButtonText;
		}
    }

    public class UnityIntegerEvent : UnityEvent<int>
    { 
    
    }