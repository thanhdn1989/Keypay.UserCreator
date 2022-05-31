using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UserCreator
{
    public class UserDataParser<T> : IUserDataEnterer
    {
        public static int nextId;

        public async Task WriteDataToCsv(TextWriter textWriter, string fieldName, object data)
        {
            await textWriter.WriteLineAsync($"{GetNextId()},{fieldName},{data}");
        }

        private int GetNextId()
        {
            return Interlocked.Increment(ref nextId);
        }

        public bool TryConvertData(string input, out T data)
        {
            try
            {
                var parseMethod = typeof(T).GetMethod("Parse", 0, new [] {typeof(string)});
                if(parseMethod != null) 
                {
                    data = (T)parseMethod.Invoke(null, new[] { input });
                    return true;
                }
                else
                {
                    data = (T)Convert.ChangeType(input, typeof(T));
                }
                return true;
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().ToString());
                data = default(T);
                Console.Out.WriteLine($"Could not convert {input} to {typeof(T).Name}!");
                return false;
            }
        }
    }
}