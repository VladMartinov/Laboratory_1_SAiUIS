using System.Collections.Generic;

namespace Laboratory_1.Classes
{
    public enum ModelType
    {
        yKDefault,
        yKSquare,
        yKCube,
        yKolmogorovaGabor
    }

    internal class Model
    {
        public List<double> Weight { get; set; }
        public List<ushort> Combination { get; set; }
        public double Quality { get; set; }
        public ModelType Type { get; set; }
    }

    internal class Predict
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
