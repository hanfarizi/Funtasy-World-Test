using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonuses;


    private void Start()
    {
        GameEvents.ShowBonusImage += OnShowBonusImage;
    }


    private void OnDisable()
    {
        GameEvents.ShowBonusImage -= OnShowBonusImage;
    }

    void OnShowBonusImage(Config.SquareColor color)
    {
        GameObject go = null;
        foreach (var bonus in bonuses)
        {
            var bonusComp = bonus.GetComponent<Bonus>();

            if (bonusComp != null && bonusComp.bonusColor == color)
            {
                go = bonus;
                bonus.SetActive(true);
                break;
            }

        }

        StartCoroutine(DeactivateBonus(go));
    }


    IEnumerator DeactivateBonus(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        go.SetActive(false);
    }
}
