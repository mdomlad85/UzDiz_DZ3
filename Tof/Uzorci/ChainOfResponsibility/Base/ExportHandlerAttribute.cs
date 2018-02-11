using System;
using System.ComponentModel.Composition;

namespace Tof.Uzorci.ChainOfResponsibility
{
    //A custom MEF Export attribute
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportHandlerAttribute : ExportAttribute
    {
        public Type SuccessorOf { get; set; }

        public ExportHandlerAttribute()
            : base(typeof(IRequestHandler))
        {
        }

        public ExportHandlerAttribute(Type successorOf)
            : base(typeof(IRequestHandler))
        {
            this.SuccessorOf = successorOf;
        }
    }
}
