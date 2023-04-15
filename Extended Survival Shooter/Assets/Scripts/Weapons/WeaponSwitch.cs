using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    Transform[] weapons;
    KeyCode[] keys;
    public List<string> ownedWeapons;
    List<string> possibleWeapons;
    float switchTime = 1f;
    int selectedWeapon = 0;
    int scrollIndex = 0;
    float time;
    public Slider chargeSlider;
    Bow bow;

    void Start()
    {
        possibleWeapons = new List<string>() {
            "NormalGun",
            "Shotgun",
            "Sword",
            "Bow"
        };
        ownedWeapons = new List<string>() {"NormalGun"};
        SetWeapons();
        Select(selectedWeapon);

        time = 0f;
        bow = weapons[3].gameObject.GetComponent<Bow>();
    }

    void Update()
    {
        time += Time.deltaTime;
        
        int prevSelected = selectedWeapon;
        int prevScrollIndex = scrollIndex;

        if ((prevSelected != 3 || (prevSelected == 3 && bow.switchWeaponAble)) && (time >= switchTime)) {
            for (int i = 0; i < keys.Length; i++) {
                if (Input.GetKeyDown(keys[i])) {
                    selectedWeapon = i;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) {
                if (scrollIndex <= 0) {
                    scrollIndex = ownedWeapons.Count - 1;
                }
                else {
                    scrollIndex--;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
                if (scrollIndex >= ownedWeapons.Count - 1) {
                    scrollIndex = 0;
                }
                else {
                    scrollIndex++;
                }
            }

            if (prevScrollIndex != scrollIndex) {
                selectedWeapon = possibleWeapons.IndexOf(ownedWeapons[scrollIndex]);
            }

            if (prevSelected != selectedWeapon && ownedWeapons.Contains(possibleWeapons[selectedWeapon])) {
                Select(selectedWeapon);
            } else {
                selectedWeapon = prevSelected;
                scrollIndex = prevScrollIndex;
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
