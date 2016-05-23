using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class RecognitionKinect : IRecognition
    {

        private string command;
        private int index;
        private double confidence;

        public RecognitionKinect(string Command, int Index, double Confidence) 
        {
            command = Command;
            index = Index;
            confidence = Confidence;
        }

        public string GetCommand()
        {
            return command;
        }

        public int GetIndex()
        {
            return index;
        }

        public double GetConfidence()
        {
            return confidence;
        }
    }
}
