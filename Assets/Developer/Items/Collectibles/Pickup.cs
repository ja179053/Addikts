using UnityEngine;

//Will have an item referenc instead of inheriting from item
//MonoBeahviours cant be serialized
//We need MonoBehaviour to make OnTriggerStay work
public class Pickup : Item
{
    public bool destroyOnUse = false;
    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Hero")
        {
            Ally2 ally2 = c.GetComponent<Ally2>();
            if (ally2 == null)
            {
                Ally ally = c.GetComponent<Ally>();
                ally.GetLoot(this);
            }
            else
            {
                ally2.GetLoot(this);
            }
            //Destroying means we cant access the gameobject
            ///OPTION A: Make inventory slots
            ///OPTION B: Dont destroy
            ///im lazy so im just gonna turn off the object.
            this.gameObject.SetActive(false);
         //   Destroy(this.gameObject);
        }
    }
    protected override void Drop(Character user)
    {
        if(!destroyOnUse)
        {
            quantity = 1;
            Rigidbody r = GameObject.Instantiate(this, user.transform.position + (Vector3.up * 1.5f), Quaternion.identity).GetComponent<Rigidbody>();
            r.gameObject.SetActive(true);
            r.AddForce(user.transform.forward * 500);
        }
        base.Drop(user);
    }
}
