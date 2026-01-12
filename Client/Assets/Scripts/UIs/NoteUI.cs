using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UIs
{
    public class NoteUI : MonoBehaviour
    {
        public TMP_Text Note0;
        public TMP_Text Note1;
        public TMP_Text Note2;

        public void Set(List<int> Note)
        {
            Note0.text = $"Note[0]:{Note[0]}";
            Note1.text = $"Note[1]:{Note[1]}";
            Note2.text = $"Note[2]:{Note[2]}";
        }
    }
}
