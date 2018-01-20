namespace Tof.Uzorci.Visitor
{
    /// <summary>
    /// The 'Element' abstract class
    /// </summary>
    public abstract class UredjajElement
    {
        public abstract void Accept(IUredjajVisitor visitor);
    }
}
