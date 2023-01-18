using TMPro;
using UnityEngine;

namespace Screens.Leaderboard
{
    public class LeaderboardSingleItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text rank;
        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text score;


        public void Init(string rankValue, string nameValue, string scoreValue)
        {
            rank.text = rankValue;
            userName.text = nameValue;
            score.text = scoreValue;
        }
    }
}