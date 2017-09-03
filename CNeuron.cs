using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeuroLib
{
     public interface NAF
     {
        double Function(double Input);
        double Derivative(double Input);
        double GetMinOut();
        double GetMaxOut();
     }

    
    [Serializable]
    public class AFTh : NAF
     {
        public double GetMinOut()
        {
            return -1.0;
        }
        public double GetMaxOut()
        {
            return 1.0;
        }

        public double Function(double Input)//-1..1
        {
            return (2/(1 + Math.Exp(-2 * Input)))-1;
        }
        public double Derivative(double Input)//0..1
        {
            return 4*Math.Exp(-2*Input) / Math.Pow((1 + Math.Exp(-2*Input)), 2);
        }

     }

    [Serializable]
    public class CNeuron
    {
        const double START_RANGE = 0.5;

        public NAF _actFunc;
        public double neuronOut=0;
        public double [] synapsWeights=null;
        private CNeuron [] _Pointer=null;
        private int _N = 0;



        public CNeuron(CNeuron[] prevLayer)
        {
            Random rnd = RandomProvider.GetThreadRandom();
            _actFunc = new AFTh();
            if (prevLayer != null)
            {
                _N = prevLayer.Count();
                _Pointer = prevLayer;
                synapsWeights = new double[_N+1];
                for (int i = 0; i < synapsWeights.Length; i++)
                {
                    synapsWeights[i] = -START_RANGE+rnd.NextDouble()* 2*START_RANGE;
                }
            }
        }

        
        public CNeuron()
        {
           
        }

        public void Activate()
        {
            if (_Pointer != null)
            {
                neuronOut = 0;

                for (int i = 0; i < _N; i++)
                {
                    neuronOut += _Pointer[i].neuronOut * synapsWeights[i];                 
                }

                neuronOut += synapsWeights[_N];
                neuronOut = _actFunc.Function(neuronOut);
            }
            
        }


    }
}
