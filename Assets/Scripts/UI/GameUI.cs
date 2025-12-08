using UnityEngine;
using UnityEngine.UI;
using Markers;

public class GameUI : MonoBehaviour
{
    [SerializeField] Image generatorBar;
    [SerializeField] Image clinicBar;

    Generator generator;
    ClinicTarget clinic;

    void Awake()
    {
        generator = FindAnyObjectByType<Generator>();
        clinic = FindAnyObjectByType<ClinicTarget>();
    }

    void Update()
    {
        if (generator != null)
            generatorBar.fillAmount = generator.Energy01;

        if (clinic != null)
            clinicBar.fillAmount = clinic.Health01;
    }
}
