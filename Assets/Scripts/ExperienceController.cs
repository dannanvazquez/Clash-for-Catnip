using UnityEngine;

public class ExperienceController : MonoBehaviour {
    private int currentLevel = 1;
    private float currentEXP = 0;
    private float levelUpEXPRequirement = 5;

    private void LevelUp() {
        currentLevel++;
        currentEXP -= levelUpEXPRequirement;
        levelUpEXPRequirement += 10;

        HUDController.Instance.UpdateLevelUI(currentLevel);
        HUDController.Instance.UpdateExperienceBarUI(Mathf.Min(currentEXP, levelUpEXPRequirement) / levelUpEXPRequirement);

        if (currentEXP >= levelUpEXPRequirement) {
            LevelUp();
        }
    }

    public void GainEXP(float expAmount) {
        currentEXP += expAmount;

        HUDController.Instance.UpdateExperienceBarUI(Mathf.Min(currentEXP, levelUpEXPRequirement) / levelUpEXPRequirement);

        if (currentEXP >= levelUpEXPRequirement) {
            LevelUp();
        }
    }
}
