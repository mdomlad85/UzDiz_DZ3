using Tof.Model;

namespace Tof.Uzorci.Visitor
{
    /// <summary>
    /// Sučelje 'Visitor'
    /// </summary>
    public interface IUredjajVisitor
    {
        void Visit(Uredjaj uredjaj);
    }
}
