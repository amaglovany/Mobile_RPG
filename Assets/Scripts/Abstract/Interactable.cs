using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected float radius = 3f;
    protected bool canInteract = true;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnMouseDown()
    {
        var distanceBetweenPlayerAndObject = Vector3.Distance(gameObject.transform.position,
            SquadData.Instance.listCharacter[SquadData.Instance.currentIndex].locomotion.transform.position);

        //UIGamePlay.Instance.PickUpVisibility(distanceBetweenPlayerAndObject <= radius);

        if (distanceBetweenPlayerAndObject <= radius)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        //This meant to be overridden
    }
    
    protected void SwitchInteractableState()
    {
        canInteract = !canInteract;
    }
}