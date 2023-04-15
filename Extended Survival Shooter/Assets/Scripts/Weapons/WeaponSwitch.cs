using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    Transform[] weapons;
    KeyCode[] keys;
    float switchTime = 1f;
    int selectedWeapon = 0;
    float time;
    bool playerDead = false;
    public Slider chargeSlider;
    Bow bow;

    void Start()
    {
        SetWeapons();
        Select(selectedWeapon);

        time = 0f;
        bow = weapons[3].gameObject.GetComponent<Bow>();
    }

    void Update()
    {
        time += Time.deltaTime;
        
        int prevSelected = selectedWeapon;

        if (prevSelected != 2 || (prevSelected == 2 && bow.switchWeaponAble)) {
            for (int i = 0; i < keys.Length; i++) {
                if (Input.GetKeyDown(keys[i]) && time >= switchTime) {
                    selectedWeapon = i;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                if (selectedWeapon >= weapons.Length - 1) {
                    selectedWeapon = 0;
                }
                else {
                    selectedWeapon++;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                if (selectedWeapon <= 0) {
                    selectedWeapon = weapons.Length - 1;
                }
                else {
                    selectedWeapon--;
                }
            }

            if (prevSelected != selectedWeapon) {
                Select(selectedWeapon);
            }
        }
    }

    void SetWeapons() {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {
            weapons[i] = transform.GetChild(i);
        }

        keys = new KeyCode[weapons.Length];
        keys[0] = KeyCode.Alpha1;
        keys[1] = KeyCode.Alpha2;
        keys[2] = KeyCode.Alpha3;
        keys[3] = KeyCode.Alpha4;
    }

    void Select(int weaponIndex) {
        Bow bow = weapons[3].gameObject.GetComponent<Bow>();

        // bow UI
        if (weaponIndex == 2) {
            chargeSlider.gameObject.SetActive(true);
        } else {
            chargeSlider.gameObject.SetActive(false);
        }

        // clean gunline
        bow.CleanGunLine();

        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        time = 0f;
    }

    public void Disable() {
        playerDead = true;
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].gameObject.SetActive(false);
        }
    }
}
