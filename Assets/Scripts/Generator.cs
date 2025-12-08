using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] float maxEnergy = 5f;
    [SerializeField] float rechargeRate = 1f;
    [SerializeField] float drainRate = 1f;

    float currentEnergy;

    public float Energy01 => currentEnergy / maxEnergy;
    public bool HasEnergy => currentEnergy > .09f;


    Projector projector;
    bool focusLocked;

    void Awake()
    {
        currentEnergy = maxEnergy;
        //TODO: DI
        projector = FindAnyObjectByType<Projector>();
    }

    void Update()
    {
        if (projector == null) return;

        if (!focusLocked)
        {
            if (projector.IsFocusActive && currentEnergy > 0f)
                DrainEnergy();
            else
                Recharge();
        }
        else
        {
            Recharge();
            if (currentEnergy >= maxEnergy)
                focusLocked = false;
        }

        if (currentEnergy <= 0f)
            focusLocked = true;
    }

    void DrainEnergy()
    {
        currentEnergy -= drainRate * Time.deltaTime;

        if (currentEnergy < 0.0001f)
            currentEnergy = 0f;

        Debug.Log("Energy: " + currentEnergy + " Has:" + HasEnergy);
    }

    void Recharge()
    {
        currentEnergy += rechargeRate * Time.deltaTime;
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Debug.Log("Energy: " + currentEnergy + " Has:" + HasEnergy);
    }
}
