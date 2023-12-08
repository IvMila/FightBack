using UnityEngine;
using TMPro;

namespace TestTask.UI
{
    public class VictoryUI :UIScreen
    {
        [SerializeField] private TextMeshProUGUI _coinText;

        public void SetCoin(int coin)
        {
            _coinText.text = coin.ToString();
        }
    }
}
