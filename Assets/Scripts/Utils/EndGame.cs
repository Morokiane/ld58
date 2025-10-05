using UnityEngine;
using TMPro;
using Controllers;

namespace Utils {
    public class EndGame : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI amountCollected;
        [SerializeField] private TextMeshProUGUI amountTrashed;
        [SerializeField] private TextMeshProUGUI scoreNum;

        private int amountLeft;
        private int score;

        private void Start() {
            amountLeft = (int)GameController.instance.amountCollected - ((int)GameController.instance.amountTrashed + (int)GameController.instance.amountDestroyed);
            score = amountLeft * 2;

            amountCollected.text = GameController.instance.amountCollected.ToString();
            amountTrashed.text = GameController.instance.amountTrashed.ToString();
            scoreNum.text = score.ToString();
        }
    }
}