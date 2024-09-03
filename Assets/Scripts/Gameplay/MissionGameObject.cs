using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionGameObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI missionDescText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI rewardText;

    public MissionData missionData;
    
    void Update() {
        if (missionData.status == "Complete") {
            // Update UI
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.missionCompleteColor;
        } else if (missionData.status == "Incomplete") {
            // Update UI
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.missionIncompleteColor;
        } else if (missionData.status == "Submitted") {
            // Update UI
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Constants.missionSubmittedColor;
        }

        // Sets text values
        categoryText.text = string.Join(", ", missionData.categories);
        missionDescText.text = missionData.missionDesc;
        progressText.text = missionData.progressValue + " / " + missionData.goalValue;
        rewardText.text = missionData.reward.ToString();
    }
}
