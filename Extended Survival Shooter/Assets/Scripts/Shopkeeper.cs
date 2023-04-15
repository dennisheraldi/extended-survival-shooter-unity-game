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

    public GameObject PetHealthUI;

    public GameObject HealerPet;
    public GameObject AttackerPet;
    public GameObject AuraBuffPet;

    public Button HealerPetBuyButton;
    public Button AttackerPetBuyButton;
    public Button AuraBuffPetBuyButton;

    public Button ShotgunBuyButton;
    public Button SwordBuyButton;
    public Button BowBuyButton;

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

        // Deactivate all button
        HealerPetBuyButton.interactable = false;
        AttackerPetBuyButton.interactable = false;
        AuraBuffPetBuyButton.interactable = false;

        ShotgunBuyButton.interactable = false;
        SwordBuyButton.interactable = false;
        BowBuyButton.interactable = false;


        // activate button if enough money
        if (MainManager.Instance.currentMoney >= 200)
        {
            HealerPetBuyButton.interactable = true;
            ShotgunBuyButton.interactable = true;
        }

        if (MainManager.Instance.currentMoney >= 300)
        {
            AttackerPetBuyButton.interactable = true;
            SwordBuyButton.interactable = true;
        }

        if (MainManager.Instance.currentMoney >= 400)
        {
            AuraBuffPetBuyButton.interactable = true;
            BowBuyButton.interactable = true;
        }

        // If player already have pet, deactivate all pet button
        if (MainManager.Instance.currentPet != "")
        {
            HealerPetBuyButton.interactable = false;
            AttackerPetBuyButton.interactable = false;
            AuraBuffPetBuyButton.interactable = false;
        }

        if (MainManager.Instance.currentPet == ""){
            PetHealthUI.SetActive(false);
        }
    }

    public void PurchasePet(string petName)
    {
        // Buy pet will instantiate pet and cannot buy another pet
        if (MainManager.Instance.currentPet == ""){
            if (petName == "Healer"){
                MainManager.Instance.currentMoney -= 200;
                MainManager.Instance.currentPet = "Healer";
                Instantiate(HealerPet, Player.transform.position, Quaternion.identity);
            } else if (petName == "Attacker")
            {
                MainManager.Instance.currentMoney -= 300;
                MainManager.Instance.currentPet = "Attacker";
                Instantiate(AttackerPet, Player.transform.position, Quaternion.identity);
            } else if (petName == "AuraBuff")
            {
                MainManager.Instance.currentMoney -= 400;
                MainManager.Instance.currentPet = "AuraBuff";
                Instantiate(AuraBuffPet, Player.transform.position, Quaternion.identity);
            }
            PetHealthUI.SetActive(true);
        }
    }

}
