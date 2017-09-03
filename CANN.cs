using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroLib
{
    [Serializable]
    public class CANN
    {
        public double[,] _inputMinMax { get; }
        public double[,] _outputMinMax { get; }
        public CNeuron[][] _Net { get; }


        public CANN(int[] Arh, double[,] mmInput, double[,] mmOutput)
        {
            
            if (Arh != null&&mmInput.GetLength(0)==Arh.First()&&mmOutput.GetLength(0)==Arh.Last())
            {
                _inputMinMax = new double[mmInput.GetLength(0), 2];
                _outputMinMax= new double[mmOutput.GetLength(0), 2];

                for (int i = 0; i < mmInput.GetLength(0); i++)
                    for (int j = 0; j < 2; j++)
                        _inputMinMax[i, j] = mmInput[i, j];

                for (int i = 0; i < mmOutput.GetLength(0); i++)
                    for (int j = 0; j < 2; j++)
                        _outputMinMax[i, j] = mmOutput[i, j];

                _Net = new CNeuron[Arh.Count()][];
                _Net[0] = new CNeuron[Arh[0]];
                for (int j = 0; j < Arh[0]; j++)
                    _Net[0][j] = new CNeuron();

                for (int i = 1; i < Arh.Count(); i++)
                {
                    _Net[i] = new CNeuron[Arh[i]];
                    for (int j = 0; j < Arh[i]; j++)
                    {
                        _Net[i][j] = new CNeuron(_Net[i - 1]);
                    }
                }
            }

        }

        private void ActivateNet()
        {
            for (int i = 1; i < _Net.Length; i++)
            {
                for (int j = 0; j < _Net[i].Length; j++)
                {
                    _Net[i][j].Activate();
                }
            }
        }

        public double[] GetNetOutput(double [] Input)
        {
            double[] result = new double[_Net[_Net.Length - 1].Length];

            if (Input.Length == _Net[0].Length)
            {
                for (int j = 0; j < _Net[0].Length; j++)
                {
                    _Net[0][j].neuronOut = Normalization(Input[j], _inputMinMax[j,0], _inputMinMax[j, 1]);
                }
                ActivateNet();
                for (int j = 0; j < _Net[_Net.Length - 1].Length; j++)
                {
                    result[j] =DeNormalization(
                        Normalization(_Net[_Net.Length - 1][j].neuronOut, 
                               _Net[_Net.Length - 1][j]._actFunc.GetMinOut(),
                               _Net[_Net.Length - 1][j]._actFunc.GetMaxOut()),
                        _outputMinMax[j,0],
                        _outputMinMax[j, 1]);
                }
            }
            return result;
        }

        private double Normalization(double Value, double Min, double Max)
        {
            if (Value <= Min)
            {
                return 0;
            }
            if (Value >= Max)
            {
                return 1;
            }

            return (Value - Min) / (Max - Min);
        }

        private double DeNormalization(double Code, double Min, double Max)
        {
            if (Code <= 0)
            {
                return Min;
            }
            if (Code >= 1)
            {
                return Max;
            }

            return Min + (Max - Min) * Code;
        }
    }
}
