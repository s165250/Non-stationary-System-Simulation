using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class InOut
    {
        private List<double> input;
        private List<double> output;

        public InOut()
        {
            input = new List<double>();
            output = new List<double>();
        }
        public void AddInput(double inputValue)
        {
            input.Add(inputValue);
        }
        public void AddOutput(double inputValue)
        {
            output.Add(inputValue);
        }
        public List<double> getInput()
        {
            return input;
        }
        public List<double> getOutput()
        {
            return output;
        }
    }
}
