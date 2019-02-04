using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class settingsManager : MonoBehaviour {

    public PhysicsMaterial2D wallBounce;
    public Slider wallBounceSlider;
    public TextMeshProUGUI wallBounceValue;

    private void Update()
    {
        wallBounce.bounciness = wallBounceSlider.value;
        wallBounceValue.text = wallBounceSlider.value.ToString();
    }

}
