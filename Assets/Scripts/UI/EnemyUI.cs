using System.Collections;
using TDC;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviourSingleton<EnemyUI>
{
    [SerializeField] private Image enemyHealthBarCurrent;

    [SerializeField] private Image enemyHealthBarEmpty;

    [HideInInspector] public Health health;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void HealthBarValueChange(int currentHealth)
    {
        enemyHealthBarCurrent.fillAmount = (float) currentHealth / 100;
    }

    public void SetHealthBarStatus(bool brightVisibility)
    {
        if (enemyHealthBarCurrent != null && enemyHealthBarEmpty != null)
        {
            if (brightVisibility)
            {
                enemyHealthBarCurrent.color = Colors.enemyEngagedHP;
                enemyHealthBarEmpty.color = Colors.enemyEngagedHP;
            }
            else
            {
                enemyHealthBarCurrent.color = Colors.enemyCalmHP;
                enemyHealthBarEmpty.color = Colors.enemyCalmHP;
            }
        }
    }

    public void DestroyEnemyUI(float duration)
    {
        //StartCoroutine(DestroyEnemyUIRoutine(duration));
        Destroy(enemyHealthBarCurrent.gameObject);
        Destroy(enemyHealthBarEmpty.gameObject);
    }

    private IEnumerator DestroyEnemyUIRoutine(float duration)
    {
        enemyHealthBarEmpty.CrossFadeAlpha(0f, duration, true);
        yield return new WaitForSeconds(duration);
        Destroy(enemyHealthBarCurrent.gameObject);
        Destroy(enemyHealthBarEmpty.gameObject);
    }
}