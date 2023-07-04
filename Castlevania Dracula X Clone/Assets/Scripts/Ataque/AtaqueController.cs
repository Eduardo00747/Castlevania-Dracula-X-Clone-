using UnityEngine;

public class AtaqueController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetBool("Ataque", true);
    }

    public void EndAttack()
    {
        animator.SetBool("Ataque", false);
    }
}