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

        if (prevSelected != 3 || (prevSelected == 3 && bow.switchWeaponAble)) {
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
        Shotgun shotgun = weapons[1].gameObject.GetComponent<Shotgun>();

        // bow UI
        if (weaponIndex == 3) {
            chargeSlider.gameObject.SetActive(true);
        } else {
            chargeSlider.gameObject.SetActive(false);
        }

        // clean gunline
        shotgun.CleanGunLine();
        bow.CleanGunLine();

        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        time = 0f;
    }

    public void Disable() {
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].gameObject.SetActive(false);
        }
    }
}
