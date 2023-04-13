using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopkeeper : MonoBehaviour
{
    public Animator anim;
    public GameObject Player;
    public GameObject Shop;
    public Text information;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, Shop.transform.position) < 5)
        {
            information.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (anim.GetBool("isShopOpen"))
                {
                    anim.SetBool("isShopOpen", false);
                    information.gameObject.SetActive(true);
                    Time.timeScale = 1;
                }
                else
                {
                    anim.SetBool("isShopOpen", true);
                    information.gameObject.SetActive(false);
                    Time.timeScale = 0;
                }
            }
        } else
        {
            information.gameObject.SetActive(false);
            anim.SetBool("isShopOpen", false);
        }
    }
}
