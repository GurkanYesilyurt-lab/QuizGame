using TMPro;
using UnityEngine;

namespace Views
{
    public class LeaderboardSingleItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text rank;
        [SerializeField] private TMP_Text userName;
        [SerializeField] private TMP_Text score;


        public void Init(string rank, string name, string score)
        {
            this.rank.text = rank;
            userName.text = name;
            this.score.text = score;
        }
    }
}