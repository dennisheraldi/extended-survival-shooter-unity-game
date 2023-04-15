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

    public GameObject HealerPetOriginal;
    public GameObject AttackerPetOriginal;
    public GameObject AuraBuffPetOriginal;

    public GameObject HealerPetClone;
    public GameObject AttackerPetClone;
    public GameObject AuraBuffPetClone;

    public Button HealerPetBuyButton;
    public Button AttackerPetBuyButton;
    public Button AuraBuffPetBuyButton;

    public Button ShotgunBuyButton;
    public Button SwordBuyButton;
    public Button BowBuyButton;

    // Timer
    float infoTimer = 0;
    float infoDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Clone the pet if it is not null
        if (MainManager.Instance.currentPet != "")
        {
            PetHealthUI.SetActive(true);

            if (MainManager.Instance.currentPet == "Healer")
            {
                // Instansiate the pet next to the player

                HealerPetClone = Instantiate(HealerPetOriginal, Player.transform.position, Quaternion.identity);
                HealerPetClone.SetActive(true);
            }
            else if (MainManager.Instance.currentPet == "Attacker")
            {
                AttackerPetClone = Instantiate(AttackerPetOriginal, Player.transform.position, Quaternion.identity);
                AttackerPetClone.SetActive(true);
            }
            else if (MainManager.Instance.currentPet == "AuraBuff")
            {
                AuraBuffPetClone = Instantiate(AuraBuffPetOriginal, Player.transform.position, Quaternion.identity);
                AuraBuffPetClone.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Player.transform.position, Shop.transform.position) < 5)
        {
            if(MainManager.Instance.isQuestOnGoing == true)
            {
                information.text = "You are within the range of the shop area, but you cannot open the shop while the quest is ongoing";
                information.gameObject.SetActive(true);
            } else {
                information.text = "You are within the range of the shop area.  Press Button 'B' to open shop";
            information.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                if(MainManager.Instance.isQuestOnGoing == false)
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
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                infoTimer = 0;
                information.text = "You are too far away from the shopkeeper";
                information.gameObject.SetActive(true);
            }

            infoTimer += Time.deltaTime;
            if (infoTimer >= infoDelay)
            {
                information.gameObject.SetActive(false);
            }

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
            PetHealthUI.SetActive(true);
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
                MainManager.Instance.currentPetHealth = 100;
                HealerPetClone = Instantiate(HealerPetOriginal, Player.transform.position, Quaternion.identity); 
                HealerPetClone.gameObject.SetActive(true);
            } else if (petName == "Attacker")
            {
                MainManager.Instance.currentMoney -= 300;
                MainManager.Instance.currentPet = "Attacker";
                MainManager.Instance.currentPetHealth = 100;
                AttackerPetClone = Instantiate(AttackerPetOriginal, Player.transform.position, Quaternion.identity);
                AttackerPetClone.gameObject.SetActive(true);
            } else if (petName == "AuraBuff")
            {
                MainManager.Instance.currentMoney -= 400;
                MainManager.Instance.currentPet = "AuraBuff";
                MainManager.Instance.currentPetHealth = 100;
                AuraBuffPetClone = Instantiate(AuraBuffPetOriginal, Player.transform.position, Quaternion.identity);
                AuraBuffPetClone.gameObject.SetActive(true);
            }
            PetHealthUI.SetActive(true);
        }
    }

}
