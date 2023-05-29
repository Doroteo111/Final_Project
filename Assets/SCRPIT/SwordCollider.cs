using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    //pues la idea la ten�is nada, en vuestro caso que la vida del player la tenga el player y
    //la del enemigo el enemigo lo que pod�is hacer es una funci�n p�blica que sea TakeDamage
    //el player tendr� la suya y gestionar� su vida, pero la funci�n ser� llamada desde el enemigo(cuando ataque el enemigo al player)
    //y viceversa para el enemigo

    //en el caso del enemigo que tendr�s acceso a la componente collider, basta usar el OnCollisionEnter
    //(ya que tanto enemigo como player tienen colliders no trigger)

    //en el caso del player, si lo tiene la espada, entonces os recomiendo que la espada tenga un
    //script que se dedique a detectar la colisi�n en el caso de la espada s� que podr�a ser trigger

    [SerializeField] private BoxCollider swordCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") && swordCollider)
        {
            Debug.Log($"enemy detected");
        }

    }


}
