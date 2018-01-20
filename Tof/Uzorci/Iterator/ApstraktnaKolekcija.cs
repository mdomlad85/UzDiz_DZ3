
namespace Tof.Uzorci.Iterator
{
    /// <summary>
    /// 'Aggregate' sučelje
    /// </summary>
    public abstract class ApstraktnaKolekcija<T>
    { 
        public abstract ApstraktniIterator<T> CreateIterator();
    }
}
