using System.Collections.Generic;
using FiltexNet.Builders.Memory.Types;

namespace net.Storages
{
    public class MemoryStorage
    {
        public List<Dictionary<string, object>> Query(MemoryExpression exp)
        {
            var results = new List<Dictionary<string, object>>();

            var id = 1;
            foreach (var v in Data.List)
            {
                if (exp.Fn(new Dictionary<string, object>
                    {
                        { "name", v.name },
                        { "version", v.version },
                        { "tags", v.tags },
                        { "status", v.status },
                        { "createdAt", v.createdAt },
                    } ))
                {
                    results.Add(new Dictionary<string, object>
                    {
                        { "id", id++ },
                        { "name", v.name },
                        { "version", v.version },
                        { "tags", v.tags },
                        { "status", v.status },
                        { "createdAt", v.createdAt },
                    });
                }
            }

            return results;
        }
    }
}