using System.IO;
using System.Threading.Tasks;

namespace UserCreator
{
    interface IUserDataEnterer
    {
        Task WriteDataToCsv(TextWriter streamWriter, string fieldName, object data);
    }
}