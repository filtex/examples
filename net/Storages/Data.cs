using System;
using System.Collections.Generic;

namespace net.Storages
{
    public class Data
    {
        public class Model
        {
            public string name { get; set; }
            public int version { get; set; }
            public string[] tags { get; set; }
            public bool status { get; set; }
            public DateTime createdAt { get; set; }
            
            public Model(string name, int version, string[] tags, bool status, DateTime createdAt)
            {
                this.name = name;
                this.version = version;
                this.tags = tags;
                this.status = status;
                this.createdAt = createdAt;
            }
        }

        public static List<Model> List = new List<Model>()
        {
            new Model("Filtex GO", 1, new [] { "filtex", "go", "backend" }, true, DateTime.Now.AddDays(-5)),
            new Model("Filtex JS", 1, new [] { "filtex", "js", "backend" }, true, DateTime.Now.AddDays(-4)),
            new Model("Filtex NET", 1, new [] { "filtex", "net", "backend" }, true, DateTime.Now.AddDays(-3)),
            new Model("Filtex UI", 1, new [] { "filtex", "js", "frontend" }, true, DateTime.Now.AddDays(-2)),
            new Model("Filtex RUST", 0, new [] { "filtex", "rust", "backend" }, false, DateTime.Now.AddDays(-1)),
            new Model("Filtex JAVA", 0, new [] { "filtex", "java", "backend" }, false, DateTime.Now),
        };
    }
}