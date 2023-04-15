using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage = 40;
    public GameObject knife;

    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(_knifeSwing());
        }
    }
    
    private IEnumerator _knifeSwing()
    {
        knife.GetComponent<Animator>().Play("KnifeSwing");
        _swing();
        yield return new WaitForSeconds(0.5f);
        knife.GetComponent<Animator>().Play("Idle");
    }

    private void _swing()
    {
        var playerPosition = _player.transform.position;
        var swordReach = Physics.OverlapSphere(playerPosition, 1.5f);
        foreach (var entity in swordReach)
        {
            var enemyHealth = entity.GetComponent<EnemyHealth>();
            var colliderVector = entity.transform.position - playerPosition;
            if (Vector3.Dot(_player.transform.forward, colliderVector) >= 0 && enemyHealth is not null)
            {
                enemyHealth.TakeDamage(damage, entity.transform.position);
            }
        }
    }
}
