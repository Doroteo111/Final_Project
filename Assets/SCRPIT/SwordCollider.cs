using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    //pues la idea la tenéis nada, en vuestro caso que la vida del player la tenga el player y
    //la del enemigo el enemigo lo que podéis hacer es una función pública que sea TakeDamage
    //el player tendrá la suya y gestionará su vida, pero la función será llamada desde el enemigo(cuando ataque el enemigo al player)
    //y viceversa para el enemigo

    //en el caso del enemigo que tendrás acceso a la componente collider, basta usar el OnCollisionEnter
    //(ya que tanto enemigo como player tienen colliders no trigger)

    //en el caso del player, si lo tiene la espada, entonces os recomiendo que la espada tenga un
    //script que se dedique a detectar la colisión en el caso de la espada sí que podría ser trigger

    [SerializeField] private BoxCollider swordCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") && swordCollider)
        {
            Debug.Log($"enemy detected");
        }

    }


}
