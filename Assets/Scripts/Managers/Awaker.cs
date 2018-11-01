using UnityEngine;

public class Awaker : MonoBehaviour
{
    [SerializeField] private GameObject[] awakeAndDisableObjects;

    private void Start()
    {
        #region Initialize_All_Awakes_And_Disable_Objects

        foreach (GameObject obj in awakeAndDisableObjects)
        {
            obj.SetActive(true);
            obj.SetActive(false);
        }

        #endregion
    }
}