using System;

namespace Tof.Model
{
    [Serializable]
    public abstract class RootElementPrototype
    {
        private string _className;
        public string ClassName => _className;
        public int ID { get; set; }
        public string Naziv { get; set; }
        public Tip Tip { get; set; }

        public RootElementPrototype(string className)
        {
            _className = className;
        }

        public abstract RootElementPrototype Clone();
    }
}
