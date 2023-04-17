using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("인벤토리")]
    public InventoryManager inventory;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            //Debug.Log(pos.x + ", " + pos.y);
            //RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                HitCheckObject(hit);
            }
            //Physics2D.Raycast(pos, Vector2.zero);
            /*
            if (hit.collider != null)
            {
                Debug.Log("Item Click1");
                HitCheckObject(hit);
            }*/
        }

        // weaponSuit();
    }


    void HitCheckObject(RaycastHit hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            //inventory.AddItem(item);
            //Destroy(hit.transform.gameObject);
        }
    }

    /*
    public void weaponSuit()
    {
        if(Input.GetKey(KeyCode.Keypad1))
        {
            weaponChange.weaponUI(inventory.items[0]);
        }

        else if (Input.GetKey(KeyCode.Keypad2))
        {
            weaponChange.weaponUI(inventory.items[1]);
        }

        else if (Input.GetKey(KeyCode.Keypad3))
        {
            weaponChange.weaponUI(inventory.items[2]);
        }
    }
    */
}
