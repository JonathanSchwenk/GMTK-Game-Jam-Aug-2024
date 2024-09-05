using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dorkbots.ServiceLocatorTools;

public class MissionGameObject : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI categoryText;
    [SerializeField] private TextMeshProUGUI missionDescText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI rewardText;

    public MissionData missionData;

    private IMissionManager missionManager;

    // Start is called before the first frame update
    void Start() {
        if (missionManager == null) {
            missionManager = ServiceLocator.Resolve<IMissionManager>();
        }
    }

    void Update() {
        if (missionData.status == "Complete") {
            // Update UI
            this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Constants.missionCompleteColor;
        } else if (missionData.status == "Incomplete") {
            // Update UI
            this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Constants.missionIncompleteColor;
        } else if (missionData.status == "Submitted") {
            // Update UI
            this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Constants.missionSubmittedColor;
        }

        // Sets text values
        categoryText.text = string.Join(", ", missionData.categories);
        missionDescText.text = missionData.missionDesc;
        progressText.text = missionData.progressValue + " / " + missionData.goalValue;
        rewardText.text = missionData.reward.ToString();

        if (missionData.status == "Submitted") {
            gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    public void SubmitMission() {
        missionManager.SubmitMission(gameObject);
    }
}
