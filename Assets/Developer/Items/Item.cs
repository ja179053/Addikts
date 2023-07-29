using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Items will ahve to be serializable
//A real targeting system has to be implemented
public class Item : MonoBehaviour {
    //Damage can be made negative for healing
    public int damage, cost = 1, quantity = 1;
    [HideInInspector]
    public TextMeshProUGUI nameText, costText, quantityText;
    public string Name
    {
        get
        {
            int i = name.Contains("(") ? name.IndexOf("(") : name.Length;
            return name.Substring(0, i);
        }
        set { }
    }
    //By default, all items will be used to damage/heal a target
    public virtual void Use(Character user, Character target)
    {
        //The code goes to here
        if (target != null)
        {
            Character finalTarget = (damage > 0) ? target : user;
            finalTarget.Damage(damage, user, AttackType.Normal);
            quantity--;
            if(quantity < 1)
            {
                ClearTexts();
                Drop(user);
            } else
            {
                quantityText.text = quantity.ToString();
            }
          //  Debug.Log(string.Format("Used, {0} charges remaining", quantity));
        }
    }
    protected virtual void Drop(Character user)
    {
        user.DropItem(this);
    }
    public void ClearTexts(bool destroy = true)
    {
        if (destroy)
        {
            if (nameText != null)
            {
                Destroy(nameText.gameObject);
            }
            if (costText != null)
            {
                Destroy(costText.gameObject);
            }
            if (quantityText != null)
            {
                Destroy(quantityText.gameObject);
            }
        } else
        {
            if (nameText != null)
            {
                nameText.color = new Vector4(nameText.color.r, nameText.color.g, nameText.color.b, 0.0f);
            }
            if (costText != null)
            {
                costText.color = new Vector4(costText.color.r, costText.color.g, costText.color.b, 0.0f);
            }
            if (quantityText != null)
            {
                quantityText.color = new Vector4(quantityText.color.r, quantityText.color.g, quantityText.color.b, 0.0f);
            }
        }
    }
}
