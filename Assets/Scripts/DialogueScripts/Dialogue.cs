using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 [System.Serializable]
public class Dialogue
{
        public string[] names;

        [TextArea(3,10)]
        public string[] sentences;

}
